using System;
using System.Linq;
using System.Text;
using SqlSugar;
namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("OpenShop")]
    public class OpenShop
    {
        public OpenShop()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long id { get; set; }

        public long openid { get; set; }

        public long applyid { get; set; }

        public string nickname { get; set; }

        public string username { get; set; }

        public string userphone { get; set; }

        public int openstatus { get; set; }

        public DateTime? createtime { get; set; }
    }
}