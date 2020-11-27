using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "DPETask")]
    public class DPETask
    {
       

        public DPETask()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long id { get; set; }

           public string taskName { get; set; }

           public string taskDesc { get; set; }

           public string taskUrl { get; set; }
           public int? taskLevel { get; set; }
           public int minNum { get; set; }
    

           public DateTime? createTime { get; set; }

           public int? taskvalue { get; set; }
    }
}