using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class DPEActivitiesViewModels
    {
        public List<DPEActivitiesViewModelsList> list { get; set; }

    }
    public class DPEActivitiesViewModelsList 
    {

        public int id { get; set; }

        public string name { get; set; }

        public int num { get; set; }

        public int gold { get; set; }

        public int status { get; set; }
    }
}
