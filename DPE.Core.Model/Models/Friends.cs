using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace DPE.Core.Model.Models
{
    [SugarTable("Friends")]
    public class Friends
    {
        public int uId { get; set; }

        public string uNickName { get; set; }

        public int sex { get; set; }

        public int PCount { get; set; }

        public int Rate { get; set; }
    }
}
