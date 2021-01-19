

using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DownExcelRecord")]
    public class DownExcelRecord
    {
        public DownExcelRecord()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }
        public string downtype { get; set; }
        public string downname { get; set; }
        public DateTime downdate { get; set; }
        public bool isdelete { get; set; }
    }
}