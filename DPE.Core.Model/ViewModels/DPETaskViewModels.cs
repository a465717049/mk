using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
   public  class DPETaskViewModels
    {
        public int num { get; set; }
        public List<DPETaskViewModelsList> list { get; set; }
    }
    public class DPETaskViewModelsList
    {
        public long id { get; set; }

        public string name { get; set; }

        public string url { get; set; }

        public int num { get; set; }

        public int status { get; set; }

        public int value { get; set; }

    }

}
