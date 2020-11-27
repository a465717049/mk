using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserEmail")]
    public class UserEmail
    {
        public UserEmail()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long eid { get; set; }

           [SugarColumn(IsPrimaryKey=true)]
           public long uId { get; set; }

           public int? status { get; set; }

           public int? isDelete { get; set; }
    }
}