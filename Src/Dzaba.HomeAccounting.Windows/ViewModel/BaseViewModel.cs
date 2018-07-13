using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(property))
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
