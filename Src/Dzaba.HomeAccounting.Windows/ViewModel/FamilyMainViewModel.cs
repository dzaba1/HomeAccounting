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
        private readonly ILongOperationPopup longOperationPopup;
        private int familyId;

        public FamilyMainViewModel(INavigationService navigation,
            IInteractionService interaction,
            IFamilyDal familyDal,
            ILongOperationPopup longOperationPopup)
        {
            Require.NotNull(navigation, nameof(navigation));
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(familyDal, nameof(familyDal));
            Require.NotNull(longOperationPopup, nameof(longOperationPopup));

            this.navigation = navigation;
            this.interaction = interaction;
            this.familyDal = familyDal;
            this.longOperationPopup = longOperationPopup;
        }

        public void OnNavigate(object parameter)
        {
            familyId = (int) parameter;
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
                longOperationPopup.Open("Ładowanie...");
                FamilyName = await familyDal.GetNameAsync(familyId);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                longOperationPopup.Close();
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
    }
}
