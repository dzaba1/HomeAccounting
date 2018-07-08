using System;

namespace Dzaba.HomeAccounting.Contracts
{
    public class OperationReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsOverriden { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
