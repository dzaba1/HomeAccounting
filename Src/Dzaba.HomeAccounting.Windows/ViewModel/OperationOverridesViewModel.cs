using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Windows.View;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Navigation;
using Dzaba.Mvvm.Windows.Navigation;
using Dzaba.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class OperationOverridesViewModel : BaseViewModel, INavigatable
    {
        private readonly IInteractionService interaction;
        private readonly IScheduledOperationDal operationDal;
        private readonly INavigationService navigation;

        public OperationOverridesViewModel(IInteractionService interaction,
            IScheduledOperationDal operationDal,
            INavigationService navigation)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(operationDal, nameof(operationDal));
            Require.NotNull(navigation, nameof(navigation));

            this.interaction = interaction;
            this.operationDal = operationDal;
            this.navigation = navigation;

            Editable = new OperationOverrideViewModel();
        }

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

        public void OnNavigate(object parameter)
        {
            Id = (int)parameter;
        }

        private async void OnLoaded()
        {
            try
            {
                Loading = true;

                Operation = await operationDal.GetAsync(Id);
                await RefreshOverrides();
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                Loading = false;
            }
        }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                RaisePropertyChanged();
            }
        }

        private OperationOverrideViewModel[] _overrides;
        public OperationOverrideViewModel[] Overrides
        {
            get { return _overrides; }
            private set
            {
                _overrides = value;
                RaisePropertyChanged();
            }
        }

        private async Task RefreshOverrides()
        {
            SelectedOverride = null;
            Overrides = (await operationDal.GetOverridesForOperationAsync(Id))
                .Select(o => new OperationOverrideViewModel
                {
                    Date = o.YearAndMonth.ToDateTime(),
                    Amount = o.Amount
                })
                .ToArray();
        }

        private OperationOverrideViewModel _selectedOverride;
        public OperationOverrideViewModel SelectedOverride
        {
            get { return _selectedOverride; }
            set
            {
                _selectedOverride = value;
                RaisePropertyChanged();
                
                if (_selectedOverride == null)
                {
                    Editable = new OperationOverrideViewModel();
                }
                else
                {
                    Editable = _selectedOverride;
                }
            }
        }

        private ScheduledOperation _operation;
        public ScheduledOperation Operation
        {
            get { return _operation; }
            private set
            {
                _operation = value;
                RaisePropertyChanged();
            }
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
                navigation.ShowView<FamilyMainView>(Operation.FamilyId);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
        }

        private OperationOverrideViewModel _editable;
        public OperationOverrideViewModel Editable
        {
            get { return _editable; }
            set
            {
                if (_editable != null)
                {
                    _editable.PropertyChanged -= OnEditablePropertyChanged;
                }

                _editable = value;
                RaisePropertyChanged();

                if (_editable != null)
                {
                    _editable.PropertyChanged += OnEditablePropertyChanged;
                }
            }
        }

        private void OnEditablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OverrideCommand.RaiseCanExecuteChanged();
        }

        private DelegateCommand _overrideCommand;
        public DelegateCommand OverrideCommand
        {
            get
            {
                if (_overrideCommand == null)
                {
                    _overrideCommand = new DelegateCommand(OnOverride, CanOverride);
                }
                return _overrideCommand;
            }
        }

        private bool CanOverride()
        {
            return Editable != null && Editable.Date.HasValue && Overrides.All(o => o.YearAndMonth != Editable.YearAndMonth);
        }

        private async void OnOverride()
        {
            try
            {
                Loading = true;

                await operationDal.OverrideAsync(Editable.YearAndMonth, Operation.Id, Editable.Amount);
                await RefreshOverrides();
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
