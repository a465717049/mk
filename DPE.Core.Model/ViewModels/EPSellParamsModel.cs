using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class EPSellParamsModel
    {

        public long uID { get; set; }
        public string key { get; set; }
        public decimal? epAmount { get; set; }
        public string usdtAddress { get; set; }
        public string trcAddress { get; set; }
        public int qId { get; set; }

        public string answer { get; set; }

        public string idcard { get; set; }
        public string password { get; set; }
        public string gCode { get; set; }
        public string bankNumber { get; set; }

        public string phone { get; set; }
        public string bankName { get; set; }
        public string bankBranchName { get; set; }
        public int? receiveType { get; set; }
        public string alipay { get; set; }
        public string imagesbase { get; set; }
    }
}
