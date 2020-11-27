using System;
using System.Collections.Generic;
using System.Text;
using DPE.Core.Model.Models;

namespace DPE.Core.Model.ViewModels
{
    public class FriendsViewModel
    {
        public string uid { get; set; }

        public string nickname { get; set; }

        public int sex { get; set; }

        public int num { get; set; }
        
    }

    public class FriendsViewModelList 
    {
        public List<FriendsViewModel> list { get; set; }
    }
}
