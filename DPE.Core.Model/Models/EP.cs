using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "EP")]
    public class EP
    {
        public EP()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long uID { get; set; }

           public decimal? amount { get; set; }
    }
}