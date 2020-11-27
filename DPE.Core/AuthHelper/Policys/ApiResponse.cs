using DPE.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.AuthHelper.Policys
{
    public class ApiResponse
    {
        public int code { get; set; } = 404;
        public string Value { get; set; } = "No Found";
        public MessageModel<string> MessageModel = new MessageModel<string>() { };

        public ApiResponse(StatusCode apiCode, string msg = null)
        {
            switch (apiCode)
            {
                case StatusCode.CODE401:
                    {
                        code = 401;
                        Value = "很抱歉，您無權訪問此功能，請確保已經登錄!";
                    }
                    break;
                case StatusCode.CODE403:
                    {
                        code = 403;
                        Value = "很抱歉，您的訪問權限等級不足，請聯係客服授權!";
                    }
                    break;
                case StatusCode.CODE429:
                    {
                        code = 429;
                        Value = "很抱歉，您的訪問過於頻繁，請稍後重試!";
                    }
                    break;
                case StatusCode.CODE500:
                    {
                        code = 500;
                        Value = msg;
                    }
                    break;
            }

            MessageModel = new MessageModel<string>()
            {
                code = code,
                msg = Value,
                success = false
            };
        }
    }

    public enum StatusCode
    {
        CODE401,
        CODE403,
        CODE404,
        CODE429,
        CODE500
    }

}
