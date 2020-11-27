using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserFeedback")]
    public class UserFeedback
    {
        public UserFeedback()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id { get; set; }

           public long uId { get; set; }

           public string Message { get; set; }

           public string MessageImgUrl { get; set; }

           public DateTime? CreateTime { get; set; }

           public int? IsReply { get; set; }
    }
}