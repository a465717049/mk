using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserTask")]
    public class UserTask
    {
        public UserTask()
        {
        }
        
           [SugarColumn(IsIdentity=true)]
           public long id { get; set; }

           [SugarColumn(IsPrimaryKey=true)]
           public long uID { get; set; }

           [SugarColumn(IsPrimaryKey=true)]
           public long taskID { get; set; }

           public DateTime? createTime { get; set; }

           public int? status { get; set; }
    }
}