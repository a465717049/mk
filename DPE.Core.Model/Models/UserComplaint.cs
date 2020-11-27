using System;
using System.Linq;
using System.Text;
using SqlSugar;



namespace DPE.Core.Model.Models
{
    [SugarTable("UserComplaint")]
    public class UserComplaint
    {
        public UserComplaint()
        {
        }
        [SugarColumn(IsPrimaryKey = true)]
        public int id { get; set; }

        public long? uid { get; set; }

        public long? complaintuid { get; set; }

        public DateTime? createtime { get; set; }

        public string complaintuidtype { get; set; }
    }
}
