using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DPE.Core.Common.Helper
{
    public class CheckVeridenNo
    {
        private const String host = "http://eid.shumaidata.com/";
        private const String path = "eid/check";
        private const String method = "POST";
        private const String appcode = "cfccd7dd0d134c3bb89659024858053d";

        public static bool ckidcard(string  idcard,string name)
        {
            try
            {
                String querys = "";
                String bodys = string.Format("idcard={0}&name={1}", idcard, name);//身份证和名字
                String url = host + path;
                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                if (0 < querys.Length)
                {
                    url = url + "?" + querys;
                }

                if (host.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                }
                httpRequest.Method = method;
                httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
                //根据API的要求，定义相对应的Content-Type
                httpRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                if (0 < bodys.Length)
                {
                    byte[] data = Encoding.UTF8.GetBytes(bodys);
                    using (Stream stream = httpRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                }

                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                string str = reader.ReadToEnd();
                if (str.IndexOf("\"一致") > 0&& str.IndexOf("\"不一致") <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {

                return false;
            }

        }



        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

    }
}
