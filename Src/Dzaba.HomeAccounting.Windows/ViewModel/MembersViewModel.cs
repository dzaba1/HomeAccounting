using Dzaba.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MembersViewModel : BaseViewModel, IParameterized
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand _loadedCommand;
        public DelegateCommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new DelegateCommand(OnLoaded);
                }
                return _loadedCommand;
            }
        }

        private async void OnLoaded()
        {
            try
            {
                LongOperation = true;
                //FamilyName = await familyDal.GetNameAsync(Id);
            }
            catch (Exception ex)
            {
                //interaction.ShowError(ex, "Error");
            }
            finally
            {
                LongOperation = false;
            }
        }

        public void SetParameter(object parameter)
        {
            Id = (int)parameter;
        }

        private bool _longOperation;
        public bool LongOperation
        {
            get { return _longOperation; }
            set
            {
                _longOperation = value;
                RaisePropertyChanged();
            }
        }
    }
}
