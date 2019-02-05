using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Navigation;
using Dzaba.Mvvm.Windows.Navigation;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class FamilyMainViewModel : BaseViewModel, INavigatable
    {
        private readonly INavigationService navigation;
        private readonly IInteractionService interaction;
        private readonly IFamilyDal familyDal;

        public FamilyMainViewModel(INavigationService navigation,
            IInteractionService interaction,
            IFamilyDal familyDal,
            ILongOperationPopup longOperationPopup)
        {
            Require.NotNull(navigation, nameof(navigation));
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(familyDal, nameof(familyDal));

            this.navigation = navigation;
            this.interaction = interaction;
            this.familyDal = familyDal;
        }

        public void OnNavigate(object parameter)
        {
            Id = (int) parameter;
        }

        private DelegateCommand _backCommand;
        public DelegateCommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new DelegateCommand(OnBack);
                }
                return _backCommand;
            }
        }

        private void OnBack()
        {
            try
            {
                navigation.ShowStartView();
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
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
                FamilyName = await familyDal.GetNameAsync(Id);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                LongOperation = false;
            }
        }

        private string _familyName;
        public string FamilyName
        {
            get { return _familyName; }
            private set
            {
                _familyName = value;
                RaisePropertyChanged();
            }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            private set
            {
                _id = value;
                RaisePropertyChanged();
            }
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
