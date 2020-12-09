using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.Models
{
    public class AddNewUserModel
    {
        public long parentID { get; set; }
        public int idType { get; set; }
        public string idNumber { get; set; }
        public string uRealName { get; set; }
        public string bankCardName { get; set; }
        public string loginPass { get; set; }
        public decimal investmentAmount { get; set; }
        public int CountryPhoneCode { get; set; }
        public string MemberNo { get; set; }
        public string NickName { get; set; }
        public string googleCode { get; set; }
        public string TradePass { get; set; }
        public long TransUserID { get; set; }
        public int Jid { get; set; }
        public int L { get; set; }


        public string phone { get; set; }
        public string addr { get; set; }
    }
}
