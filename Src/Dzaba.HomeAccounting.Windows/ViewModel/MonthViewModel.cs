using Dzaba.Mvvm;
using Dzaba.Mvvm.Navigation;
using System.Linq;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MonthViewModel : BaseViewModel, INavigatable
    {
        private MonthReportViewModel _report;
        public MonthReportViewModel Report
        {
            get { return _report; }
            private set
            {
                _report = value;
                RaisePropertyChanged();
            }
        }

        public void OnNavigate(object parameter)
        {
            Report = (MonthReportViewModel)parameter;
            Incomes = new IncomesViewModel("Przychody", Report.Report.Operations.Where(o => o.Amount >= 0).OrderBy(o => o.Date));
            Expenses = new IncomesViewModel("Wydatki", Report.Report.Operations.Where(o => o.Amount < 0).OrderBy(o => o.Date));
        }

        private IncomesViewModel _incomes;
        public IncomesViewModel Incomes
        {
            get { return _incomes; }
            private set
            {
                _incomes = value;
                RaisePropertyChanged();
            }
        }

        private IncomesViewModel _expenses;
        public IncomesViewModel Expenses
        {
            get { return _expenses; }
            private set
            {
                _expenses = value;
                RaisePropertyChanged();
            }
        }
    }
}
