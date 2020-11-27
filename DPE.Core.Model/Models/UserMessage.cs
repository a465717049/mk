using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserMessage")]
    public class UserMessage
    {
        public UserMessage()
        {
        }
        
           [SugarColumn(IsIdentity=true)]
           public long id { get; set; }

           [SugarColumn(IsPrimaryKey=true)]
           public long uID { get; set; }

           [SugarColumn(IsPrimaryKey=true)]
           public long messageID { get; set; }

           public DateTime? createTime { get; set; }

           public int? status { get; set; }
    }
}