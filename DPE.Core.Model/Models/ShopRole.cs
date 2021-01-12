using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("ShopRole")]
    public class ShopRole
    {
        public ShopRole()
        {
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        public long uId { get; set; }
        public long shopRoleId { get; set; }
        public DateTime? createTime { get; set; }
    }
}