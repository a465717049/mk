using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "SPexchange")]
    public class SPexchange
    {
        public SPexchange()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public long? uID { get; set; }

           public decimal? amount { get; set; }

           public decimal? lastTotal { get; set; }

           public long? fromID { get; set; }

           public int? scount { get; set; }

           public int? stype { get; set; }

           public decimal? price { get; set; }

           public DateTime? createTime { get; set; }

           public string remark { get; set; }
    }
}