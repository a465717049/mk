using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserData")]
    public class UserData
    {
        public UserData()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long uID { get; set; }

           public decimal? weeklyMoney { get; set; }

           public int? friends { get; set; }

           public int? subAccount { get; set; }

           public long? LProfit { get; set; }

           public int? invites { get; set; }

           public long? RProfit { get; set; }

           public long? XProfit { get; set; }

           public long? LCount { get; set; }

           public long? RCount { get; set; }
    }
}