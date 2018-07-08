using Dzaba.HomeAccounting.DataBase.EntityFramework;
using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Dzaba.HomeAccounting.DataBase.Contracts;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class DbInitalizerTests : IntegrationTestFixutre
    {
        private IDbInitializer CreateSut()
        {
            return Container.GetRequiredService<IDbInitializer>();
        }

        [Test]
        public void InitializeAsync_WhenCalled_ThenItMakesADb()
        {
            var sut = CreateSut();
            sut.InitializeAsync().Wait();
        }
    }
}
