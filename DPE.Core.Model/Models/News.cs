using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("News")]
    public class News
    {
        public News()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }

        public string title { get; set; }
        public string message { get; set; }

        public string context { get; set; }

        public DateTime? createTime { get; set; }

        public int? isDelete { get; set; }

        public int? maxRole { get; set; }

        public int? minRole { get; set; }

        public string entitle { get; set; }
        public int? ntype { get; set; }

        public string encontext { get; set; }

        public string newsimg { get; set; }
    }
}