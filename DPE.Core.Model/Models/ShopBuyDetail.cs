﻿using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{

    [SugarTable("ShopBuyDetail")]
    public class ShopBuyDetail
    {
        public ShopBuyDetail()
        {
        }
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }
        public long shopid { get; set; }
        public long buyuid { get; set; }
        public decimal price { get; set; }
        public int buyNum { get; set; }
        public int status { get; set; }
        public string reamrk { get; set; }
        public DateTime createTime { get; set; }
    }
}
