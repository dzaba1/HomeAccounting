using Dzaba.HomeAccounting.Contracts;
using Dzaba.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class IncomesViewModel : BaseViewModel
    {
        public IncomesViewModel(string header, IEnumerable<OperationReport> operations)
        {
            Header = header;
            Operations = operations.ToArray();
        }

        private string _header;
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                RaisePropertyChanged();
            }
        }

        private OperationReport[] _operations;
        public OperationReport[] Operations
        {
            get { return _operations; }
            set
            {
                _operations = value;
                RaisePropertyChanged();
            }
        }

        public decimal Sum => Operations.Sum(o => o.Amount);
    }
}
