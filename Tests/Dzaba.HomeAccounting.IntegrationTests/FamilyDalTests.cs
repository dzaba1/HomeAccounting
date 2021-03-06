﻿using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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

            var members = await Container.GetRequiredService<IFamilyMembersDal>().GetMemberNamesAsync(id);
            members.Count.Should().Be(2);
        }
    }
}
