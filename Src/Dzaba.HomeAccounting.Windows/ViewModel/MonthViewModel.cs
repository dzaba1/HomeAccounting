using Dzaba.Mvvm;
using Dzaba.Mvvm.Navigation;

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
        }
    }
}
