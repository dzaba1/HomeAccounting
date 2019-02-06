using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.Mvvm;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MonthDataViewModel : BaseViewModel
    {
        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged();
            }
        }
    }
}
