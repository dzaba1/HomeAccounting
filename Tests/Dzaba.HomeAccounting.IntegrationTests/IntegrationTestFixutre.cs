using NUnit.Framework;
using System;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    public class IntegrationTestFixutre
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
    }
}
