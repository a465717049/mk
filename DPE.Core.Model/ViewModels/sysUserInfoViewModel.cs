using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class sysUserInfoViewModel
    {
        public string LoginName { get; set; }
        public string uLoginPWD { get; set; }
        public string uRealName { get; set; }
        public int uStatus { get; set; }
        public string uRemark { get; set; }
        public System.DateTime uCreateTime { get; set; }

    }
}
