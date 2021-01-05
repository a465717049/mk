using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "ShopList")]
    public class ShopList
    {
        public ShopList()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long id { get; set; }

           public string pName { get; set; }

           public string pDesc { get; set; }

           public string pIcon { get; set; }

           public int? pNum { get; set; }

           public decimal? price { get; set; }

           public int? priceType { get; set; }

           public DateTime? createTime { get; set; }

           public int? status { get; set; }

           public int? minLevel { get; set; }

           public int? ptype { get; set; }

          public int? Shopgroup { get; set; }

          public bool? isDelete { get; set; }
    }
}