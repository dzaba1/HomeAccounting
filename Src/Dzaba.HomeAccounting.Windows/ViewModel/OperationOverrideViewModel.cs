using Dzaba.HomeAccounting.Contracts;
using Dzaba.Mvvm;
using System;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class OperationOverrideViewModel : BaseViewModel
    {
        private DateTime? _date;
        public DateTime? Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged();
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }

        public YearAndMonth YearAndMonth => Date.HasValue ? new YearAndMonth(Date.Value) : new YearAndMonth();
    }
}
