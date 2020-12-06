using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{

    [SugarTable("ShoppingCart")]
    public class ShoppingCart
    {
        public ShoppingCart()
        {
        }
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public long uid { get; set; }
        public long shopid { get; set; }
        public int shoptotalnum { get; set; }
    }
}
