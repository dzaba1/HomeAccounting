using Dzaba.HomeAccounting.DataBase.EntityFramework;
using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Dzaba.HomeAccounting.DataBase.Contracts;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class DbInitalizerTests
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

        private IDbInitializer CreateSut()
        {
            return container.GetRequiredService<IDbInitializer>();
        }

        [Test]
        public void InitializeAsync_WhenCalled_ThenItMakesADb()
        {
            var sut = CreateSut();
            sut.InitializeAsync().Wait();
        }
    }
}
