using System;
using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class OperationReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsOverriden { get; set; }
        public DateTime Date { get; set; }
        public KeyValuePair<int, string>? Member { get; set; }
    }
}
