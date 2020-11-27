using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "IRP")]
    public class IRP
    {
        public IRP()
        {
        }

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public long uID { get; set; }

        public decimal? amount { get; set; }
    }
}