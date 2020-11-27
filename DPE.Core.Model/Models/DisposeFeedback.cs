using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "DisposeFeedback")]
    public class DisposeFeedback
    {
        public DisposeFeedback()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public long? MessageUid { get; set; }

           public string Message { get; set; }

           public string MessageImgUrl { get; set; }

           public DateTime? CreateTime { get; set; }

           public bool? IsDelete { get; set; }

           public bool? IReply { get; set; }
    }
}