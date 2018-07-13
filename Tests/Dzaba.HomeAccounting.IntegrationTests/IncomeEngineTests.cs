using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.Engine;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    [TestFixture]
    public class IncomeEngineTests : IntegrationTestFixutre<IIncomeEngine>
    {
        [Test]
        public async Task CalculateAsync_WhenOperationScheduled_ThenItComputesIncome()
        {
            var invoker = GetInvokerDal();
            var familyId = await invoker.AddSmithFamilyAsync();
            await invoker.AddScheduledOperationAsync(familyId, "Family income", 10);

            var today = new YearAndMonth(DateTime.Today);

            var sut = CreateSut();
            var result = await sut.CalculateAsync(familyId, today, today.AddMonths(2));
            result.FamilyId.Should().Be(familyId);
            result.FamilyName.Should().Be("Smith");
            result.Reports.Length.Should().Be(3);
            result.Reports.Last().Sum.Should().Be(30);
            result.Reports.All(x => x.Income == 10).Should().BeTrue();
            result.Reports.All(x => x.Operations.Count == 1).Should().BeTrue();
        }
    }
}
