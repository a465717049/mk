using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class EmailListViewModels
    {
        public List<EmailListViewModelsList> list { get; set; }
    }

    public class EmailListViewModelsList
    {
        public long id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public long time { get; set; }

        public int status { get; set; }

        public string source { get; set; }

    }
}
