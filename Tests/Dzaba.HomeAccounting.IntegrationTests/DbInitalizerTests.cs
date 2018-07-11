using NUnit.Framework;
using Dzaba.HomeAccounting.DataBase.Contracts;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class DbInitalizerTests : IntegrationTestFixutre<IDbInitializer>
    {
        [Test]
        public async Task InitializeAsync_WhenCalled_ThenItMakesADb()
        {
            var sut = CreateSut();
            await sut.InitializeAsync();
        }
    }
}
