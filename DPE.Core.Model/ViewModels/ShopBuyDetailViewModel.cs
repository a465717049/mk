using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class ShopBuyDetailViewModel
    {

        public List<ShopBuyDetailViewModelList> list { get; set; }
    }


    public class ShopBuyDetailViewModelList
    {
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
