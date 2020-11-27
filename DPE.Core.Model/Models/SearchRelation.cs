using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    [SugarTable("SearchRelation")]
    public class SearchRelation
    {
        public SearchRelation()
        {
        }
        // honorLevel InvestmentLevel,uNickName,a.jid,L,b.subAccount,b.friends,b.LProfit,b.RProfit,b.LCount,b.RCount a.uID,0 as ceng,0 as pos
        public int honorLevel { get; set; }
        public string InvestmentLevel { get; set; }
        public string uNickName { get; set; }
        public long jid { get; set; }
        public int L { get; set; }
        public int subAccount { get; set; }
        public int friends { get; set; }
        public long LProfit { get; set; }
        public long RProfit { get; set; }
        public int LCount { get; set; }
        public int RCount { get; set; }
        public long uID { get; set; }
        public int ceng { get; set; }
        public int pos { get; set; }
    }
}
