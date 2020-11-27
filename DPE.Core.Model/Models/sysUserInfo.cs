using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;


namespace DPE.Core.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "sysUserInfo")]
    public class sysUserInfo
    {
       

        public sysUserInfo()
        {
        }

        public sysUserInfo(string loginName, string loginPWD)
        {
            uLoginName = loginName;
            uLoginPWD = loginPWD;
            uRealName = uLoginName;
            uStatus = 0;
            uCreateTime = DateTime.Now;
            uUpdateTime = DateTime.Now;
            uLastErrTime = DateTime.Now;
            uErrorCount = 0;
            uNickName = string.Empty;
            uHeadImgUrl = string.Empty;
        }

        /// <summary>
        /// �û�ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public long uID { get; set; }
        /// <summary>
        /// ��¼�˺�
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string uLoginName { get; set; }
        /// <summary>
        /// ��¼����
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string uLoginPWD { get; set; }
        /// <summary>
        /// ��ʵ����
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string uRealName { get; set; }

        /// <summary>
        /// ͷ���ַ
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string uHeadImgUrl { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        public int uStatus { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string uRemark { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public System.DateTime uCreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public System.DateTime uUpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        ///����¼ʱ�� 
        /// </summary>
        public DateTime uLastErrTime { get; set; } = DateTime.Now;

        /// <summary>
        ///������� 
        /// </summary>
        public int uErrorCount { get; set; }

        // �Ա�
        [SugarColumn(IsNullable = true)]
        public int sex { get; set; } = 0;
        // ��ַ
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string addr { get; set; }

        // T��ַ
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string Taddr { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<int> RIDs { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<string> RoleNames { get; set; }

        public string uTradPWD { get; set; }
        public string messageToken { get; set; }

        
        public string uNickName { get; set; }

           public long? tid { get; set; }

           public long? jid { get; set; }

           public int? honorLevel { get; set; }

           public string googleKey { get; set; }

           public bool? isBindGoogle { get; set; }

         public int? isSetIDNumber { get; set; }

           public bool? isSetAnswer { get; set; }

           public int? IDType { get; set; }

           public string IDNumber { get; set; }

           public bool? isService { get; set; }

           public bool? isSon { get; set; }

           public long? fromMainID { get; set; }

           public bool isDelete { get; set; }
    }
}