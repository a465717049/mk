using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DPE.Core.AuthHelper;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common;
using DPE.Core.Common.Helper;
using DPE.Core.Common.HttpContextUser;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DPE.Core.Controllers
{

    /// <summary>
    ///邮箱 银币管理【权限】D:\API\DPE\DPE.Core\AuthHelper\Policys\PermissionItem.cs
    /// </summary>
    [Produces("application/json")]
    [Route("api/Update")]
    public class UpdateController : Controller
    {


        public UpdateController()
        {

        }


        [HttpPost]
        [HttpGet]
        [Route("AppUpdate")]
        public decimal AppUpdate()
        {
            return Appsettings.app("AppVersion", "Version").ObjToDecimal();
        }

        /// <summary>
        /// 获取用户总收益
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [Route("getVerificationCode")]
        public MessageModel<dynamic> getVerificationCode()
        {

            var data = new MessageModel<dynamic>();
            var chkCode = this.RndNum(5);
            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;
            Random rnd = new Random();
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman" };
            //验证码的字符集，去掉了一些容易混淆的字符 

            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 12, (float)0);
            }
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Gif);
                var imgbyte = ms.ToArray();
                string imgstr = Convert.ToBase64String(imgbyte);

                string temp = imgstr.Substring(imgstr.Length - 16, 16);
                string temp2 = imgstr.Substring(0, imgstr.Length - 16);
                var vcode = temp2 + chkCode + temp;
                if (vcode.Length > 0)
                {

                    data.response = new { vcode };
                    data.success = true;
                }
                else
                {

                    data.success = false;
                    data.code = -1;
                }
            }
            catch (Exception)
            {

                data.success = false;
                data.code = -1;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
            return data;
        }
        private string RndNum(int VcodeNum)
        {
            //验证码可以显示的字符集合 
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
              ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
              ",R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组  
            string code = "";//产生的随机数 
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数 

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同 
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类 
                }
                int t = rand.Next(61);//获取随机数 
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);//如果获取的随机数重复，则递归调用 
                }
                temp = t;//把本次产生的随机数记录起来 
                code += VcArray[t];//随机数的位数加一 
            }
            return code;
        }

    }
}

