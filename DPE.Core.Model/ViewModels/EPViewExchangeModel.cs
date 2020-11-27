using System;
using System.Collections.Generic;
using System.Text;

namespace DPE.Core.Model.ViewModels
{
    public class EPViewExchangeModel
    {
        public int page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; } = 6;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int dataCount { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }
        public int ItemCount { get; set; }
        public int SubAmount { get; set; }
        public int AddAmount { get; set; }
        public List<EPViewDataExchangeModel> List { get; set; }
    }
    public class EPViewDataExchangeModel
    {
        public decimal EPAmount { get; set; }
        public string SurplusTotalAmount { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int OperationType { get; set; }
        public string Des { get; set; }
        public string Remark { get; set; }
        public string Time { get; set; }
    }
}
