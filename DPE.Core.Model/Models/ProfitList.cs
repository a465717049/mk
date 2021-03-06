using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "ProfitList")]
    public class ProfitList
    {
        public ProfitList()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public int pType { get; set; }

           public long amount { get; set; }

           public long uID { get; set; }

           public long? fromID { get; set; }

           public DateTime? createTime { get; set; }
    }
}