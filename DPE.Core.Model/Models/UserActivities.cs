using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserActivities")]
    public class UserActivities
    {
        public UserActivities()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long uid { get; set; }

           [SugarColumn(IsPrimaryKey=true)]
           public int actID { get; set; }

           public int? status { get; set; }
    }
}