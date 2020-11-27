using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "ADexchange")]
    public class ADexchange
    {
        public ADexchange()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long id { get; set; }

           public long uID { get; set; }

           public decimal? amount { get; set; }

           public decimal? lastTotal { get; set; }

           public long? fromID { get; set; }

           public int? stype { get; set; }

           public DateTime? createTime { get; set; }

           public string remark { get; set; }
    }
}