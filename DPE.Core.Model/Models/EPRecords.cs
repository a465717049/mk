using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("EPRecords")]
    public class EPRecords
    {
        public EPRecords()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }

        public long? uID { get; set; }

        public decimal? amount { get; set; }

        public decimal? rate { get; set; }

        public decimal? RMBrate { get; set; }

        public decimal? USDTrate { get; set; }

        public int? status { get; set; }

        public string statusName { get; set; }

        public DateTime? createTime { get; set; }

        public int? receiveType { get; set; }

        public DateTime? buyTime { get; set; }

        public DateTime? payTime { get; set; }

        public long? buyId { get; set; }

        public DateTime? confirmTime { get; set; }

        public string usdtAddress { get; set; }
        public string trcAddress { get; set; }

        public string phone { get; set; }


        [SugarColumn(IsIgnore=true)]
        public int tcount { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int fcount { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int appealcount { get; set; }
    }
}