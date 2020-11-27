using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class ExchangeViewModels
    {
        public List<ExchangeViewList> list { get; set; }

    }
    public class ExchangeViewList 
    {
        public long time { get; set; }

        public string msg { get; set; }
    }
}
