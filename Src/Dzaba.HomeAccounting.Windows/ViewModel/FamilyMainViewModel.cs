using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Navigation;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class FamilyMainViewModel : BaseViewModel, INavigatable
    {
        private int familyId;

        public void OnNavigate(object parameter)
        {
            familyId = (int) parameter;
        }
    }
}
