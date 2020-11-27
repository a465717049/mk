using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "DPERecords")]
    public class DPERecords
    {
        public DPERecords()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long Id { get; set; }

           public long? uID { get; set; }

           public decimal? amount { get; set; }

           public int? status { get; set; }

           public int? tradeType { get; set; }

           public int scount { get; set; }

           public decimal? price { get; set; }

           public DateTime? createTime { get; set; }
    }
}