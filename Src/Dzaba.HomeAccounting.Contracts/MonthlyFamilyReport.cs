using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class MonthlyFamilyReport
    {
        public KeyValuePair<int, string> Family { get; set; }
        public MonthReport[] Reports { get; set; }
    }
}
