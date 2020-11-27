using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "Manure")]
    public class Manure
    {
        public Manure()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long uID { get; set; }

           public decimal? amount { get; set; }
    }
}