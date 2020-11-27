using DPE.Core.Common.Helper;
using DPE.Core.Common.HttpContextUser;
using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class EPServices : BaseServices<EP>, IEPServices
    {
        private readonly IEPRepository _dal;
        private readonly IUser _user;

        private readonly IEPexchangeRepository _exchangedal;
        private readonly IAnswerRepository _answerdal;
        private readonly IsysUserInfoRepository _sysUserInfodal;
        private readonly IEPRecordsRepository _ePRecordsdal;
        private readonly IUserInfoRepository _userinfodal;
        private readonly ISettingsRepository _settingsdal;
        private readonly IUnitOfWork _iunitofwork;
        public EPServices(IEPRepository dal, IUnitOfWork iunitofwork, IEPexchangeRepository exchangedal, IAnswerRepository answerdal, IsysUserInfoRepository sysUserInfodal, 
            IEPRecordsRepository ePRecordsdal,  IUserInfoRepository userinfodal, ISettingsRepository settingsdal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            this._iunitofwork = iunitofwork;
            this._exchangedal = exchangedal;
            this._answerdal = answerdal;
            this._sysUserInfodal = sysUserInfodal;
            this._sysUserInfodal = sysUserInfodal;
            this._ePRecordsdal = ePRecordsdal;
            this._userinfodal = userinfodal;
            this._settingsdal = settingsdal;
        }

        public async Task<string> checkEPUser(long uid, long parentId, string type)
        {
            SugarParameter[] parameters = new SugarParameter[]
            {
                 new SugarParameter("@parentId",parentId),
                 new SugarParameter("@uid",uid),
                 new SugarParameter("@type",type),
            };

            try
            {
                var i = await _dal.QueryProcSingin("Sp_CheckUser", parameters);
               
                return i;
            }
            catch { return ""; }

        }

        /// <summary>
        /// 出售EP
        /// </summary>
        /// <param name="ePSellParams"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> EPSell(EPSellParamsModel ePSellParams)
        {
            string sql = string.Format("select uid from PassEP where uid={0}", ePSellParams.uID);
            var pass = await _dal.QuerySql(sql);

            MessageModel<string> result = new MessageModel<string>();

            if (pass.Count>0)
            {
                result.code = 50003;//交易密碼不正確！請重試！！
                result.success = false;
                result.msg = "此賬號EP出書已鎖定！請先解除鎖定";
                return result;
            }
             sql = string.Format("select 1 from EpRecords where uid={0}", ePSellParams.uID);
            var count = await _dal.QuerySql(sql);

            if (count.Count ==1 && Math.Abs(ePSellParams.epAmount.Value) < 300)
            {
                result.code = 50003;//交易密碼不正確！請重試！！
                result.success = false;
                result.msg = "賬戶第二次出售金額必須大於300EP";
                return result;
            }
            if (count.Count >1 && Math.Abs( ePSellParams.epAmount.Value) < 500)
            {
                result.code = 50003;//交易密碼不正確！請重試！！
                result.success = false;
                result.msg = "賬戶兩次以上出售金額必須大於500EP";
                return result;
            }
         
            //验证交易密码
            var user = (await _sysUserInfodal.Query(d => d.uID == ePSellParams.uID && d.uTradPWD == MD5Helper.MD5Encrypt32(ePSellParams.password) && d.isDelete == false)).ToList().FirstOrDefault();
            if (user == null)
            {
                result.code = 50003;//交易密碼不正確！請重試！！
                result.success = false;
                result.msg = "交易密碼不正確！請重試！！";
                return result;
            }
            //验证谷歌
            var gCode = ePSellParams.gCode;
            var keys = MD5Helper.MD5Encrypt32(user.googleKey);
            GoogleAuthenticator authenticator = new GoogleAuthenticator(30, keys);
            string code = authenticator.GenerateCode();
            if (gCode != code)
            {
                result.code = 50004;//Google驗證碼校驗錯誤！請重新輸入！
                result.success = false;
                result.msg = "谷歌驗證碼校驗錯誤！請重新輸入！";
                return result;
            }
            //汇率
            var settings=(await _settingsdal.Query()).FirstOrDefault();
            decimal rate = 0;
            decimal usdRate = 6.95M;
            decimal cnRate = 6.175M;
            if (settings != null)
            {
                usdRate = settings.ComRate.ObjToDecimal();
                cnRate = settings.CNYRate.ObjToDecimal();
                rate = cnRate / usdRate;
            }
          
            //插入销售记录
            EPRecords epmRecordsModel = new EPRecords()
            {
                uID = ePSellParams.uID,
                amount = 0-(ePSellParams.epAmount).ObjToDecimal(),
                rate = rate,
                RMBrate = cnRate,
                USDTrate= usdRate,
                status = 1,
                statusName = "出售中",
                usdtAddress = ePSellParams.usdtAddress,
                trcAddress = ePSellParams.trcAddress,
                receiveType = 2,
                phone = ePSellParams.phone,
                createTime=DateTime.Now
            };
            //判断金币是否足够
            var epamount = (await (new UserInfoServices(_userinfodal).GetUserInfo(ePSellParams.uID)))?.EP.ObjToDecimal();
            if (epamount <= 0|| epamount< ePSellParams.epAmount)
            {
                result.code = 50005;//可出售EP金額不足！
                result.success = false;
                result.msg = "可出售EP金額不足！";
                return result;
            }
            //if (epamount <= 0 || epamount < ePSellParams.epAmount+5)
            //{
            //    result.code = 50005;//可出售EP金額不足！
            //    result.success = false;
            //    result.msg = "EP出售所需手续费不足！";
            //    return result;
            //}
            _iunitofwork.BeginTran();
            try
            {
                var exrowsid = await _ePRecordsdal.Add(epmRecordsModel);
                if (exrowsid > 0)
                {
                    EPexchange pexchange = new EPexchange()
                    {
                        uID = ePSellParams.uID,
                        amount = 0 - (ePSellParams.epAmount).ObjToDecimal(),
                        lastTotal = epamount,
                        fromID = 0,
                        recordID = exrowsid,
                        scount = 0,
                        stype = 15,
                        price = 0,
                        createTime = DateTime.Now,
                        remark = "出售EP"
                    };
                    //EPexchange xexchange = new EPexchange()
                    //{
                    //    uID = ePSellParams.uID,
                    //    amount = -5,
                    //    lastTotal = epamount,
                    //    fromID = 0,
                    //    recordID = exrowsid,
                    //    scount = 0,
                    //    stype = 21,
                    //    price = 0,
                    //    createTime = DateTime.Now,
                    //    remark = "手續費"
                    //};
                    var changerowid = await _exchangedal.Add(pexchange);
                    //var xrowid = await _exchangedal.Add(xexchange);
                    if (changerowid > 0)
                    {
                        var epentity = (await _dal.QueryById(ePSellParams.uID));
                        if (epentity != null)
                        {
                            epentity.amount = epentity.amount - Math.Abs(ePSellParams.epAmount.ObjToDecimal());
                            await _dal.Update(epentity);

                            _iunitofwork.CommitTran();//提交事务
                        }
                        else
                        {
                            _iunitofwork.RollbackTran();//事务回滚
                            result.code = 50006;
                            result.success = false;
                            result.msg = "提交失败请稍后再试！";
                            return result;
                        }
                    }
                    else
                    {
                        _iunitofwork.RollbackTran();//事务回滚
                        result.code = 50006;
                        result.success = false;
                        result.msg = "提交失败请稍后再试！";
                        return result;
                    }
                }
                else
                {
                    _iunitofwork.RollbackTran();//事务回滚
                    result.code = 50006;
                    result.success = false;
                    result.msg = "提交失败请稍后再试！";
                    return result;
                }
            }catch(Exception ex)
            {
                _iunitofwork.RollbackTran();//事务回滚
                result.code = 50006;
                result.success = false;
                result.msg = "提交失败请稍后再试！";
                return result;

            }
            result.code = 0;
            result.success = true;
            return result;
        }

        /// <summary>
        /// EP转RP、SP
        /// </summary>
        /// <param name="dType"></param>
        /// <param name="password"></param>
        /// <param name="amount"></param>
        /// <param name="uID"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> EPTransOtherType(string oType, string dType, string password, string googlekey,decimal amount,long uid)
        {
            MessageModel<string> result = new MessageModel<string>();
            var user = (await _sysUserInfodal.Query(d => d.uID == uid && d.uTradPWD == MD5Helper.MD5Encrypt32(password) && d.isDelete == false)).ToList().FirstOrDefault();

            if (user == null)
            {
                result.code = 50003;//交易密碼不正確！請重試！！
                result.success = false;
                return result;
            }



            if (oType !="EP" && oType!="RP")
            {
                result.code = 50007;//轉換類型不存在!
                result.success = false;
                return result;
            }
            if (dType != "RP" && dType != "SP")
            {
                result.code = 50008;//被轉換類型不存在!
                result.success = false;
                return result;
            }
            if (oType == "RP" && dType == "RP")
            {
                result.code = 50008;//被轉換類型不存在!
                result.success = false;
                return result;
            }
           
            if (amount.ObjToDecimal() <= 0)
            {
                result.code = 50009;//轉換數量不能小於0!
                result.success = false;
                return result;
            }


            SugarParameter[] parameters = new SugarParameter[]
            {
                new SugarParameter("@uid", uid),
                new SugarParameter("@type",oType),
                new SugarParameter("@toType",dType),
                new SugarParameter("@Amount",amount)
            };
            var epentity= (await _dal.QueryProc("Process_Transform", parameters)).FirstOrDefault();
            if (epentity?.amount.ObjToDecimal() == 0)
            {
                result.code = 0;
                result.success = true;
            }
            else
            {
                result.code = Convert.ToInt32(epentity?.amount.ObjToDecimal());
                result.success = false;
            }
           
            return result;
        }
        
        public async Task<MessageModel<string>> TransToUser(string oType,long userID, string password, string googlekey, decimal amount,long uid)
        {
            MessageModel<string> result = new MessageModel<string>();
            var user = (await _sysUserInfodal.Query(d => d.uID == uid && d.uTradPWD == MD5Helper.MD5Encrypt32(password) && d.isDelete == false)).ToList().FirstOrDefault();
            if (user == null)
            {
                result.code = 50003;//交易密碼不正確！請重試！！
                result.success = false;
                return result;
            }

            var gCode = googlekey;
            var keys = MD5Helper.MD5Encrypt32(user.googleKey);
            GoogleAuthenticator authenticator = new GoogleAuthenticator(30,keys);
            string code = authenticator.GenerateCode();
            if (gCode != code)
            {
                result.code = 50004;//Google驗證碼校驗錯誤！請重新輸入！
                result.success = false;
                return result;
            }
            if (oType != "EP" && oType != "RP")
            {
                result.code = 50007;//轉換類型不存在!
                result.success = false;
                return result;
            }
          

            if (amount.ObjToDecimal() <= 0)
            {
                result.code = 50009;//轉換數量不能小於0!
                result.success = false;
                return result;
            }


            SugarParameter[] parameters = new SugarParameter[]
            {
                new SugarParameter("@uid",_user.ID),
                new SugarParameter("@type",oType),
                new SugarParameter("@touid",userID),
                new SugarParameter("@Amount",amount)
            };
            var epentity = (await _dal.QueryProc("Process_TransformToUser", parameters)).FirstOrDefault();
            if (epentity?.amount.ObjToDecimal() == 0)
            {
                result.code = 0;
                result.success = true;
            }
            else
            {
                result.code = Convert.ToInt32(epentity?.amount.ObjToDecimal());
                result.success = false;
            }

            return result;
        }
    }
}