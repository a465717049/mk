using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "SP")]
    public class SP
    {
        public SP()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long uID { get; set; }

           public decimal? amount { get; set; }
    }
}