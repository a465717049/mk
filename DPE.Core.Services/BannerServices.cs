
﻿using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class BannerServices : BaseServices<Banner>, IBannerServices
    {
        private readonly IBannerRepository _dal;
        public BannerServices(IBannerRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
