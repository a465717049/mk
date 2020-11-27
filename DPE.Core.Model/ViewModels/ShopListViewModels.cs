using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class ShopListViewModels
    {
      
        public List<ShopListViewModelsList> list { get; set; }
    }

    public class ShopListViewModelsList 
    {
        public int id { get; set; }
        public string name { get; set; }
        public int num { get; set; }
        public int own_num { get; set; }
        public int price { get; set; }
        public string icon_url { get; set; }

    }
}
