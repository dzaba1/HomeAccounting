using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using FluentAssertions;
using NUnit.Framework;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class FamilyDalTests : IntegrationTestFixutre<IFamilyDal>
    {
        [Test]
        public void AddFamily_WhenFamilyAdded_ThenItCanBeSelected()
        {
            var name = "Smith";

            var sut = CreateSut();

            var id = sut.AddFamilyAsync(name, new[] { "Mark", "Alice" }).Result;
            sut.GetNameAsync(id).Result.Should().Be(name);

            var members = sut.GetMemberNamesAsync(id).Result;
            members.Count.Should().Be(2);
        }
    }
}
