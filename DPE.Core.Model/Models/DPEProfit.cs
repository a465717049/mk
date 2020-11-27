using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>

    [SugarTable( "DPEProfit")]
    public class DPEProfit
    {
        public DPEProfit()
        {
        }
        
           public int id { get; set; }

           public int? mouth { get; set; }

           public decimal? rate { get; set; }
    }
}