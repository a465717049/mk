using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "BuyDPEHistory")]
    public class BuyDPEHistory
    {
        public BuyDPEHistory()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long id { get; set; }

           public long? uID { get; set; }

           public decimal? amount { get; set; }

           public DateTime? historyTime { get; set; }

           public DateTime? createTime { get; set; }
    }
}