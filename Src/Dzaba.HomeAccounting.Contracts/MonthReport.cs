using System.Collections.Generic;
using System.Diagnostics;

namespace Dzaba.HomeAccounting.Contracts
{
    [DebuggerDisplay("{Date} - {Sum}")]
    public class MonthReport
    {
        public YearAndMonth Date { get; set; }

        public decimal Income { get; set; }

        public decimal Sum { get; set; }

        public List<OperationReport> Operations { get; set; } = new List<OperationReport>();
    }
}
