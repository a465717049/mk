using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("AD")]
    public class AD
    {
        public AD()
        {
        }
        [SugarColumn(IsPrimaryKey = true)]
        public long uID { get; set; }

        public decimal? amount { get; set; }
    }
}