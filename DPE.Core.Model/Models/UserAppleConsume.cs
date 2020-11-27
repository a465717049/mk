using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserAppleConsume")]
    public class UserAppleConsume
    {
        public UserAppleConsume()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public long uID { get; set; }

           public int? sType { get; set; }

           public decimal? amount { get; set; }

           public int? status { get; set; }
    }
}