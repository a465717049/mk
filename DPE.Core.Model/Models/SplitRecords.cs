using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "SplitRecords")]
    public class SplitRecords
    {
        public SplitRecords()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long id { get; set; }

           public long uID { get; set; }

           public int? splitCount { get; set; }

           public decimal? splitRate { get; set; }

           public decimal? splitBefore { get; set; }

           public decimal? splitLater { get; set; }

           public DateTime? createTime { get; set; }
    }
}