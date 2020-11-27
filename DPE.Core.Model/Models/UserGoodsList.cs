using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "UserGoodsList")]
    public class UserGoodsList
    {
        public UserGoodsList()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public long uID { get; set; }

           public long shopID { get; set; }

           public int? num { get; set; }

           public decimal? price { get; set; }

           public DateTime? createTime { get; set; }

           public int? status { get; set; }

           public string address { get; set; }

           public string phone { get; set; }
    }
}