using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    public class IntegrationTestFixutre<T>
    {
        protected IServiceProvider Container { get; private set; }

        [SetUp]
        public void Setup()
        {
            DbUtils.Delete();
            Container = Bootstrapper.CreateContainer();
        }

        [TearDown]
        public void Cleanup()
        {
            DbUtils.Delete();
        }

        protected T CreateSut()
        {
            return Container.GetRequiredService<T>();
        }

        protected IInvokerDal GetInvokerDal()
        {
            return Container.GetRequiredService<IInvokerDal>();
        }
    }
}
