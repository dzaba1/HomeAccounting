using Dzaba.HomeAccounting.Windows.ViewModel;

namespace Dzaba.HomeAccounting.Windows.Model
{
    internal sealed class MonthIncomesParameter
    {
        public MonthReportViewModel Report { get; set; }
        public decimal InitialSavings { get; set; }
        public int FamilyId { get; set; }
    }
}
