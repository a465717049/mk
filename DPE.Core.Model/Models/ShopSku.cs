using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("ShopSku")]
    public class ShopSku
    {
        public ShopSku()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }
        public long   shopid     {get;set;}
        public string skuIcon    {get;set;}
        public string skuname    {get;set;}
        public string   skudesc    {get;set;}
        public DateTime createtime { get; set; }
        public bool isdelete { get; set; }
    }
}