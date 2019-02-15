using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;

namespace Dzaba.HomeAccounting.Engine
{
    internal sealed class CurrentMonthData
    {
        public YearAndMonth Month { get; set; }
        public MonthData MonthData { get; set; }
        public MonthReport MonthReport { get; set; }
    }
}
