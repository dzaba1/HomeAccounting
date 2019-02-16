using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class DayReport
    {
        public int Day { get; set; }

        public decimal Income { get; set; }

        public decimal Sum { get; set; }

        public List<OperationReport> Operations { get; set; } = new List<OperationReport>();
    }
}
