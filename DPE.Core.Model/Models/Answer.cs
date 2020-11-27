using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "Answer")]
    public class Answer
    {
        public Answer()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public long uID { get; set; }

           public int qID { get; set; }

           public string answer { get; set; }

           public DateTime? createTime { get; set; }
    }
}