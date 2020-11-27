using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "Split")]
    public class Split
    {
        public Split()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public long uID { get; set; }

           public int? PCount { get; set; }

           public DateTime? createTime { get; set; }
    }
}