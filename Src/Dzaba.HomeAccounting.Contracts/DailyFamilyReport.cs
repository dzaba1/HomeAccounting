using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class DailyFamilyReport
    {
        public KeyValuePair<int, string> Family { get; set; }
        public YearAndMonth Month { get; set; }
        public DayReport[] Reports { get; set; }
    }
}
