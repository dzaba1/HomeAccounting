using NUnit.Framework;
using Dzaba.HomeAccounting.DataBase.Contracts;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class DbInitalizerTests : IntegrationTestFixutre<IDbInitializer>
    {
        [Test]
        public void InitializeAsync_WhenCalled_ThenItMakesADb()
        {
            var sut = CreateSut();
            sut.InitializeAsync().Wait();
        }
    }
}
