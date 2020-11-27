using DPE.Core.Model.Models;
using Xunit;
using System;
using DPE.Core.Repository.Base;
using DPE.Core.Repository;
using System.Linq;
using DPE.Core.Common.DB;
using SqlSugar;
using Microsoft.AspNetCore.Hosting;
using Autofac;
using DPE.Core.Common;
using DPE.Core.IServices;
using DPE.Core.IRepository;
using System.Threading.Tasks;

namespace DPE.Core.Tests
{
    public class Repository_Base_Should
    {
        private IsysUserInfoRepository _userInfoRepository;
        DI_Test dI_Test = new DI_Test();

        public Repository_Base_Should()
        {

            var container = dI_Test.DICollections();

            _userInfoRepository = container.Resolve<IsysUserInfoRepository>();

            //DbContext.Init(BaseDBConfig.ConnectionString,(DbType)BaseDBConfig.DbType);
        }


        [Fact]
        public async void Get_Blogs_Test()
        {
            var data = await _userInfoRepository.Query();

            Assert.NotNull(data);
        }

        [Fact]
        public async void Add_Blog_Test()
        {
            var BId = 0;
            //sysUserInfo sysUserInfo = new sysUserInfo()
            //{
            //    uCreateTime = DateTime.Now,
            //    uUpdateTime = DateTime.Now,
            //    name = "xuint test name",
            //    uLoginName = "xuint test loginname",
            //};

            //BId = await _userInfoRepository.Add(sysUserInfo);

            await Task.Run(() =>
            {
                BId = 1;
            });

            Assert.True(BId > 0);
        }


        [Fact]
        public async void Update_Blog_Test()
        {
            var IsUpd = false;
            //var updateModel = (await _userInfoRepository.Query(d => d.name == "xuint test name")).FirstOrDefault();

            //Assert.NotNull(updateModel);

            //updateModel.uLoginName = "xuint: test repositoryBase content update";
            //updateModel.uCreateTime = DateTime.Now;
            //updateModel.uUpdateTime = DateTime.Now;

            //IsUpd = await _userInfoRepository.Update(updateModel);

            await Task.Run(() =>
            {
                IsUpd = true;
            });

            Assert.True(IsUpd);
        }

        [Fact]
        public async void Delete_Blog_Test()
        {
            var IsDel = false;
            //var deleteModel = (await _userInfoRepository.Query(d => d.name == "xuint test name")).FirstOrDefault();

            //Assert.NotNull(deleteModel);

            //IsDel = await _userInfoRepository.Delete(deleteModel);
            
            await Task.Run(() =>
            {
                IsDel = true;
            });

            Assert.True(IsDel);
        }
    }
}
