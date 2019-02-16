using Dzaba.HomeAccounting.Windows.Model;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Navigation;
using System.Linq;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MonthViewModel : BaseViewModel, INavigatable
    {
        private MonthIncomesParameter _monthParameter;
        public MonthIncomesParameter MonthParameter
        {
            get { return _monthParameter; }
            private set
            {
                _monthParameter = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Report));
            }
        }

        public MonthReportViewModel Report => MonthParameter.Report;

        public void OnNavigate(object parameter)
        {
            MonthParameter = (MonthIncomesParameter)parameter;
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
