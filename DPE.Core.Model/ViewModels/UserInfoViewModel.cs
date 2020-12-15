using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
   public class UserInfoViewModel
    {
       

        public string uid { get; set;}
        public int lv_name { get; set;}
        public string nickname { get; set;}
        public decimal coin { get; set;}
        public decimal gold { get; set;}
        public decimal gold_zu { get; set;}
        public decimal apple { get; set;}
        public decimal manure { get; set;}
        public decimal seed { get; set;}
        public int signin { get; set;}
        public int sex { get; set;}
        public long create_time { get; set;}
        public int farmers { get; set;}
        public string coin_location { get; set;}
        public string trc_address { get; set; }
        public int autonym { get; set;}
        public bool service { get; set; }
        public bool isBindGoogle { get; set; }

        public int friend { get; set; }
        public string photo { get; set; }
        public decimal rp { get; set; }
        public decimal lprofit { get; set; }
        public decimal rprofit { get; set; }
        public decimal weekly { get; set; }
        public decimal sum { get; set; }
        public decimal dynamicTotal { get; set; }

        public bool? isSetIDNumber { get; set; }
    }
}
