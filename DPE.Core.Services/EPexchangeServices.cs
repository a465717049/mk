using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace DPE.Core.Services
{
    public partial class EPexchangeServices : BaseServices<EPexchange>, IEPexchangeServices
    {
        private readonly IEPexchangeRepository _dal;
        public EPexchangeServices(IEPexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<EPexchange>> GetEPExchange(long uid)
        {
            var userList = await base.Query(a => a.uID == uid && a.stype != 0);
            return userList;
        }

        public async Task<List<EPexchange>> GetEPGoldExchange(long uid)
        {
            var userList = await base.Query(a => a.uID == uid && a.stype == 0);
            return userList;
        }

        public async Task<MessageModel<dynamic>> GetEPExchangePageList(int stype, int pageSize, int pageIndex, long uID)
        {
            Expression<Func<EPexchange, bool>> exp = t => t.uID == uID && t.stype == (stype == 0 ? t.stype : stype);
            var list = (await base.QueryPage(exp, pageIndex, pageSize, "id asc"));
            EPViewExchangeModel retlist = new EPViewExchangeModel();
            retlist.page = list.page;
            retlist.pageCount = list.pageCount;
            retlist.dataCount = list.dataCount;
            retlist.PageSize = list.PageSize;
            retlist.ItemCount = 0;
            retlist.SubAmount = 0;
            retlist.AddAmount = 0;
            List<EPViewDataExchangeModel> vlist = new List<EPViewDataExchangeModel>();
            foreach (EPexchange item in list.data)
            {
                EPViewDataExchangeModel vmodel = new EPViewDataExchangeModel();
                vmodel.EPAmount = item.amount.ObjToDecimal();
                vmodel.SurplusTotalAmount = item.lastTotal.ObjToDecimal().ToString();
                vmodel.Type = 0;
                if (item.stype == 1 || item.stype == 2 || item.stype == 3)
                {
                    vmodel.TypeName = "分潤獎勵";
                } else if (stype == 15)
                {
                    vmodel.TypeName = "EP出售";
                }
                else if (stype == 9)
                {
                    vmodel.TypeName = "'EP兌換";
                }
                else if (stype == 14)
                {
                    vmodel.TypeName = "EP轉出";
                }
                else if (stype == 17)
                {
                    vmodel.TypeName = "歸總";
                }

                vmodel.OperationType = item.amount.ObjToDecimal() > 0 ? 1 : 2;
                vmodel.Des = item.remark;
                vmodel.Remark = item.remark;
                vmodel.Time = item.createTime.ToString();
                vlist.Add(vmodel);
            }
            retlist.List = vlist;
            return new MessageModel<dynamic>()
            {
                msg = "获取成功",
                success = true,
                response = retlist
            };

        }

        public async Task<MessageModel<string>> ProcessTransformToUser(long uid, string type, decimal amount, long touid)
        {
            MessageModel<string> result = new MessageModel<string>();

            SugarParameter[] parameters = new SugarParameter[]
            {
                 new SugarParameter("@uid",uid),
                new SugarParameter("@type",type),
                new SugarParameter("@amount",amount),
                  new SugarParameter("@touid",touid)
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

        public async Task<bool> RoolBackThisTran(long id, string type)
        {

            try
            {
                SugarParameter[] parameters = new SugarParameter[2];
                parameters[0] = new SugarParameter("@id", id);
                parameters[1] = new SugarParameter("@type", type);
                var result = await _dal.QueryProcTable("Process_RollBackTransformToUser", parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToInt32(result.Rows[0]["resultnumber"]) == 0 ? true : false;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }

        }


    }
}