using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "BuyDPEList")]
    public class BuyDPEList
    {
        public BuyDPEList()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true)]
           public long id { get; set; }

           public long? uID { get; set; }

           public decimal? amount { get; set; }

           public DateTime? createTime { get; set; }
    }
}