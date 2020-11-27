using DPE.Core.Common;
using DPE.Core.Controllers;
using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using Moq;
using Xunit;
using System;
using Autofac;

namespace DPE.Core.Tests
{
    public class Redis_Should
    {
        DI_Test dI_Test = new DI_Test();

        public Redis_Should()
        {
            //var container = dI_Test.DICollections();
            //_redisCacheManager = container.Resolve<IRedisCacheManager>();

        }

        [Fact]
        public void Connect_Redis_Test()
        {

            //var redisBlogCache = _redisCacheManager.Get<object>("Redis.Blog");

            //Assert.Null(redisBlogCache);
        }

    }
}
