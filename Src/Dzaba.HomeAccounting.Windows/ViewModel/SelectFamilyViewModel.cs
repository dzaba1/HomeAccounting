using Dzaba.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.Utils;
using Dzaba.HomeAccounting.Windows.View;
using Dzaba.Mvvm.Windows;
using Dzaba.Mvvm.Windows.Navigation;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class SelectFamilyViewModel : BaseViewModel
    {
        private readonly IInteractionService interaction;
        private readonly IFamilyDal familyDal;
        private readonly ILongOperationPopup longOperationPopup;
        private readonly INavigationService navigation;

        public SelectFamilyViewModel(IInteractionService interaction,
            IFamilyDal familyDal,
            ILongOperationPopup longOperationPopup,
            INavigationService navigation)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(familyDal, nameof(familyDal));
            Require.NotNull(longOperationPopup, nameof(longOperationPopup));
            Require.NotNull(navigation, nameof(navigation));

            this.interaction = interaction;
            this.familyDal = familyDal;
            this.longOperationPopup = longOperationPopup;
            this.navigation = navigation;
        }

        private string _newFamilyName;
        public string NewFamilyName
        {
            get { return _newFamilyName; }
            set
            {
                _newFamilyName = value;
                RaisePropertyChanged();
                NewCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _newCommand;
        public DelegateCommand NewCommand
        {
            get
            {
                if (_newCommand == null)
                {
                    _newCommand = new DelegateCommand(OnNewFamily, () => !string.IsNullOrWhiteSpace(NewFamilyName));
                }

                return _newCommand;
            }
        }

        private async void OnNewFamily()
        {
            try
            {
                longOperationPopup.OpenLongOperationPopup("Dodaję nową rodzinę...");

                var id = await familyDal.AddFamilyAsync(NewFamilyName);
                navigation.ShowView<FamilyMainView>(id);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                longOperationPopup.CloseLongOperationPopup();
            }
        }

        private DelegateCommand _onLoadedCommand;

        public DelegateCommand OnLoadedCommand
        {
            get
            {
                if (_onLoadedCommand == null)
                {
                    _onLoadedCommand = new DelegateCommand(OnLoaded);
                }

                return _onLoadedCommand;
            }
        }

        private async void OnLoaded()
        {
            try
            {
                longOperationPopup.OpenLongOperationPopup("Ładowanie...");
                var families = await familyDal.GetAllNamesAsync();
                Families = new ConcurrentObservableCollection<KeyValuePair<int, string>>(families);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                longOperationPopup.CloseLongOperationPopup();
            }
        }

        private ConcurrentObservableCollection<KeyValuePair<int, string>> _families;
        public ConcurrentObservableCollection<KeyValuePair<int, string>> Families
        {
            get { return _families; }
            private set
            {
                _families = value;
                RaisePropertyChanged();
            }
        }

        private int? _selectedFamilyId;
        public int? SelectedFamilyId
        {
            get { return _selectedFamilyId; }
            set
            {
                _selectedFamilyId = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand<int> _deleteFamilyCommand;

        public DelegateCommand<int> DeleteFamilyCommand
        {
            get
            {
                if (_deleteFamilyCommand == null)
                {
                    _deleteFamilyCommand = new DelegateCommand<int>(DeleteFamily);
                }

                return _deleteFamilyCommand;
            }
        }

        private async void DeleteFamily(int id)
        {
            try
            {
                if (!interaction.YesNoQuestion("Czy na pewno chcesz usunąć rodzinę?"))
                {
                    return;
                }
              
                longOperationPopup.OpenLongOperationPopup("Usuwanie rodziny...");
                await familyDal.DeleteFamilyAsync(id);

                Families.RemoveWhere(f => f.Key == id);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                longOperationPopup.CloseLongOperationPopup();
            }
        }
    }
}
