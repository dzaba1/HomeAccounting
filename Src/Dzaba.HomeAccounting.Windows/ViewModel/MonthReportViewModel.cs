using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.Mvvm;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MonthReportViewModel : BaseViewModel
    {
        public MonthReportViewModel(MonthReport report)
        {
            Require.NotNull(report, nameof(report));

            Report = report;
        }

        public MonthReport Report { get; }

        private MonthData _data;
        public MonthData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged();
            }
        }

        public YearAndMonth Date => Report.Date;

        private bool _isNew;
        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                RaisePropertyChanged();
            }
        }

        public string Notes
        {
            get { return Data.Notes; }
            set
            {
                Data.Notes = value;
                RaisePropertyChanged();
            }
        }

        public decimal? IncomeOverride
        {
            get => Data.IncomeOverride;
            set
            {
                Data.IncomeOverride = value;
                RaisePropertyChanged();
            }
        }

        public decimal? SumOverride
        {
            get => Data.TotalOverride;
            set
            {
                Data.TotalOverride = value;
                RaisePropertyChanged();
            }
        }
    }
}
