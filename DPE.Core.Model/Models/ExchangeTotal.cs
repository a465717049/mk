using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{

    [SugarTable("ExchangeTotal")]
    public class ExchangeTotal
    {
        public ExchangeTotal()
        {
        }


        public long? uid { get; set; }

        public decimal? amount { get; set; }

        public decimal? lastTotal { get; set; }

        public string remark { get; set; }

        public DateTime? createTime { get; set; }

        public string extype { get; set; }

        public int stype { get; set; }

    }
}
