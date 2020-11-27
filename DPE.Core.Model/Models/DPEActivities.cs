using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "DPEActivities")]
    public class DPEActivities
    {
        public DPEActivities()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int id { get; set; }

           public string title { get; set; }

           public string context { get; set; }

           public decimal? amount { get; set; }

           public int? honorLevel { get; set; }

           public int? max { get; set; }

           public int? qty { get; set; }

           public string lang { get; set; }

           public int? status { get; set; }
    }
}