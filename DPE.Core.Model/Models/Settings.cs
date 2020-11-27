using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Settings")]
    public class Settings
    {
        public Settings()
        {
        }

        public decimal? EPRate { get; set; }

        public decimal? SPRate { get; set; }

        public decimal? ComRate { get; set; }

        public decimal? CNYRate { get; set; }

        public int? JDMaxLevel { get; set; }

        public int? LDMaxLevel { get; set; }

        public int? DPMaxLevel { get; set; }

        public decimal? JDRate { get; set; }

        public decimal? LDRate { get; set; }

        public decimal? DPRate { get; set; }

        public string InvitationUrl { get; set; }
    }
}