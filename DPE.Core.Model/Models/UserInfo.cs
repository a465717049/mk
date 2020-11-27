using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserInfo")]
    public class UserInfo
    {
        public UserInfo()
        {
        }
        
           public long uID { get; set; }

           public int? honorLevel { get; set; }

           public string uNickName { get; set; }
           public string Photo { get; set; }

            public bool? isService { get; set; }

           public bool? isSetIDNumber { get; set; }

           public bool? isSetAnswer { get; set; }

           public bool? isSon { get; set; }

           public int uStatus { get; set; }

            public string addr { get; set; }
            public string trcAddress { get; set; }

        public DateTime uCreateTime { get; set; }

           public int Signin { get; set; }
           public bool isBindGoogle { get; set; }

          public decimal? weeklyMoney { get; set; }

           public int? sex { get; set; }
           public int? friends { get; set; }

             public int? subAccount { get; set; }

           public long? LCount { get; set; }

           public long? RCount { get; set; }

           public long? LProfit { get; set; }

           public long? RProfit { get; set; }

           public decimal? DPE { get; set; }
           public decimal? SP { get; set; }

           public decimal? EP { get; set; }

           public decimal? IRP { get; set; }

           public decimal? AD { get; set; }

           public decimal? Manure { get; set; }

           public decimal? RP { get; set; }
           public decimal? Sum { get; set; }
           public decimal? DynamicTotal { get; set; }

           public long? jid { get; set; }
           public long? tid { get; set; }

           public long? isDelete { get; set; }

    }
}