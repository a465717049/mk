using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>

    [SugarTable( "UserAppleStatus")]
    public class UserAppleStatus
    {
        public UserAppleStatus()
        {
        }
        
        [SugarColumn(IsPrimaryKey=true,IsIdentity =true)]
        public int id { get; set; }

        public long uID { get; set; }

           public decimal? amount { get; set; }

           public int? Status { get; set; }
    }
}