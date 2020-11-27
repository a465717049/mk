using AutoMapper;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;

namespace DPE.Core.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<sysUserInfo, sysUserInfoViewModel>().ForMember(d => d.LoginName, o => o.MapFrom(s => s.uLoginName));
            CreateMap<sysUserInfoViewModel, sysUserInfo>().ForMember(d => d.uLoginName, o => o.MapFrom(s => s.LoginName));
        }
    }
}
