using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "AdminReply")]
    public class AdminReply
    {
        public AdminReply()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id { get; set; }

           public int UserFeedbackId { get; set; }

           public string ReplyMessage { get; set; }

           public DateTime ReplyTime { get; set; }
    }
}