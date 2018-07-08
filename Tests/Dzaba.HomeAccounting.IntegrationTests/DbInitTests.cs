using Dzaba.HomeAccounting.DataBase.EntityFramework;
using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class DbInitTests
    {
        public IServiceProvider container;

        [SetUp]
        public void Setup()
        {
            DbUtils.Delete();
            container = Bootstrapper.CreateContainer();
        }

        [TearDown]
        public void Cleanup()
        {
            DbUtils.Delete();
        }

        [Test]
        public void DbCanBeInitialized()
        {
            using (var db = container.GetService<DatabaseContext>())
            {
                db.Database.EnsureCreated().Should().BeTrue();
            }
        }
    }
}
