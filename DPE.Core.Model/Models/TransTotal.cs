using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "TransTotal", "DB_MSSQL_dpepie_Master")]
    public class TransTotal
    {
        public TransTotal()
        {
        }
        
           public long? Total { get; set; }
    }
}