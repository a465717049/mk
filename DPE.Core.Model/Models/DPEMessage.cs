using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "DPEMessage")]
    public class DPEMessage
    {
        public DPEMessage()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public string title { get; set; }

           public string content { get; set; }

           public DateTime? createTime { get; set; }

           public int? status { get; set; }

        public string entitle { get; set; }

        public string encontent { get; set; }

    }
}