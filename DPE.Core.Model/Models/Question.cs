using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Question")]
    public class Question
    {
        public Question()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Question_CN { get; set; }

        public string Question_EN { get; set; }
    }
}