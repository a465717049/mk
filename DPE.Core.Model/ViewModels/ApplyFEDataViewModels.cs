using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    /// <summary>
    /// 请求扩建农场ViewModel
    /// </summary>
    public class ApplyFEDataViewModels
    {
        /// <summary>
        /// 剩余天数
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// 扩建需要的种子
        /// </summary>
        public int seed { get; set; }
    }
}
