using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("ShopSkuDetail")]
    public class ShopSkuDetail
    {
        public ShopSkuDetail()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }

        public long skuid { get; set; }
        public string detailname { get; set; }
        public int detailnum { get; set; }
        public decimal detailprice { get; set; }
        public string detailicon { get; set; }
        public string detaildesc { get; set; }
        public DateTime createtime { get; set; }

        public bool isdelete { get; set; }
    }
}