using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Engine
{
    internal sealed class IncomeBuilderData
    {
        public int FamilyId { get; set; }
        public IReadOnlyDictionary<int, string> MemberNames { get; set; }
        public ScheduledOperation[] AllScheduledOperations { get; set; }
    }
}
