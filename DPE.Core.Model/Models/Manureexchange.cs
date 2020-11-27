using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "Manureexchange")]
    public class Manureexchange
    {
        public Manureexchange()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity =true)]
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