using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "Banner")]
    public class Banner
    {
        public Banner()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public string cnText { get; set; }

           public string enText { get; set; }

           public DateTime? createtime { get; set; }

           public int? status { get; set; }
    }
}