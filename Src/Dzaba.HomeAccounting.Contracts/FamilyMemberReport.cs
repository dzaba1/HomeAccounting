using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class FamilyMemberReport
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Income { get; set; }

        public List<OperationReport> Operations { get; set; } = new List<OperationReport>();
    }
}
