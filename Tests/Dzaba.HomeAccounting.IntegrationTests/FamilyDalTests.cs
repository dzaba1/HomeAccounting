using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class FamilyDalTests : IntegrationTestFixutre<IFamilyDal>
    {
        [Test]
        public async Task AddFamily_WhenFamilyAdded_ThenItCanBeSelected()
        {
            var name = "Smith";

            var sut = CreateSut();

            var id = await sut.AddFamilyAsync(name, new[] { "Mark", "Alice" });
            sut.GetNameAsync(id).Result.Should().Be(name);

            var members = sut.GetMemberNamesAsync(id).Result;
            members.Count.Should().Be(2);
        }
    }
}
