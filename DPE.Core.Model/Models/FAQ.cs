using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "FAQ")]
    public class FAQ
    {
        public FAQ()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int id { get; set; }

           public string Questions { get; set; }

           public string Answer { get; set; }
    }
}