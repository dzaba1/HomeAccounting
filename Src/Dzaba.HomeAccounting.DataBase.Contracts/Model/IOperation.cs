using Dzaba.HomeAccounting.Contracts;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    public interface IOperation : IEntity<int>
    {
        int FamilyId { get; set; }

        Family Family { get; set; }

        decimal Amount { get; set; }

        string Name { get; set; }

        int? MemberId { get; set; }

        FamilyMember Member { get; set; }

        OperationReport ToOperationReport(YearAndMonth currentMonth);
    }
}
