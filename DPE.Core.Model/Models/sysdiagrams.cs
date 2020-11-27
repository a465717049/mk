using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "sysdiagrams")]
    public class sysdiagrams
    {
        public sysdiagrams()
        {
        }
        
           public string name { get; set; }

           public int principal_id { get; set; }

           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int diagram_id { get; set; }

           public int? version { get; set; }

           public byte[] definition { get; set; }
    }
}