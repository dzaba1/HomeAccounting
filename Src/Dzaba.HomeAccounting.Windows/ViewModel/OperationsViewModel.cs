using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Windows.Model;
using Dzaba.HomeAccounting.Windows.View;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Windows;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class OperationsViewModel : BaseViewModel, IParameterized
    {
        private readonly IInteractionService interaction;
        private readonly IOperationDal operationDal;
        private readonly IScheduledOperationDal scheduledOperationDal;
        private readonly IFamilyMembersDal membersDal;
        private readonly INavigationFacade navigation;
        private IReadOnlyDictionary<int, string> members;

        public OperationsViewModel(IInteractionService interaction,
            IOperationDal operationDal,
            IScheduledOperationDal scheduledOperationDal,
            IFamilyMembersDal membersDal,
            INavigationFacade navigation)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(operationDal, nameof(operationDal));
            Require.NotNull(scheduledOperationDal, nameof(scheduledOperationDal));
            Require.NotNull(membersDal, nameof(membersDal));
            Require.NotNull(navigation, nameof(navigation));

            this.interaction = interaction;
            this.operationDal = operationDal;
            this.scheduledOperationDal = scheduledOperationDal;
            this.membersDal = membersDal;
            this.navigation = navigation;

            Editable = new OperationViewModel();
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

        private async Task<OperationViewModel[]> GetOperationsAsync()
        {
            var oper1 = await operationDal.GetOperationsAsync(Id);
            var oper2 = await scheduledOperationDal.GetAllAsync(Id);

            var operVm1 = oper1.Select(o => new OperationViewModel
            {
                Id = o.Id,
                Amount = o.Amount,
                Date = o.Date,
                Ends = null,
                Member = GetMemberPair(o.MemberId),
                Name = o.Name,
                Scheduled = false,
                DayDate = o.Date,
                HasConstantDate = o.HasConstantDate
            });
            var operVm2 = oper2.Select(o => new OperationViewModel
            {
                Id = o.Id,
                Amount = o.Amount,
                Date = o.Starts,
                Ends = o.Ends,
                Member = GetMemberPair(o.MemberId),
                Name = o.Name,
                Scheduled = true,
                DayDate = o.Starts.HasValue ? o.Starts.Value : (DateTime?)null,
                HasConstantDate = o.HasConstantDate
            });

            return operVm1.Concat(operVm2)
                .OrderBy(o => o.Date)
                .ToArray();
        }

        private MemberNamePair GetMemberPair(int? memberId)
        {
            if (memberId.HasValue)
            {
                var name = members[memberId.Value];
                return new MemberNamePair(memberId.Value, name);
            }

            return Consts.DefaultMemberPair;
        }

        private async void OnLoaded()
        {
            try
            {
                Loading = true;
                members = await membersDal.GetMemberNamesAsync(Id);

                Members = new ConcurrentObservableCollection<MemberNamePair>();
                Members.Add(Consts.DefaultMemberPair);
                foreach (var pair in members)
                {
                    Members.Add(new MemberNamePair(pair.Key, pair.Value));
                }

                Editable.Member = Members[0];
                await RefreshOperations();
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

        private async Task RefreshOperations()
        {
            SelectedOperation = null;
            Operations = new ConcurrentObservableCollection<OperationViewModel>(await GetOperationsAsync());
        }

        public void SetParameter(object parameter)
        {
            Id = (int)parameter;
        }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            private set
            {
                _loading = value;
                RaisePropertyChanged();
            }
        }

        private ConcurrentObservableCollection<OperationViewModel> _operations;
        public ConcurrentObservableCollection<OperationViewModel> Operations
        {
            get { return _operations; }
            private set
            {
                _operations = value;
                RaisePropertyChanged();
            }
        }

        private OperationViewModel _selectedOperation;
        public OperationViewModel SelectedOperation
        {
            get { return _selectedOperation; }
            set
            {
                _selectedOperation = value;
                RaisePropertyChanged();

                if (_selectedOperation == null)
                {
                    Editable = new OperationViewModel();
                }
                else
                {
                    Editable = value;
                }
            }
        }

        private OperationViewModel _editable;
        public OperationViewModel Editable
        {
            get { return _editable; }
            private set
            {
                if (_editable != value)
                {
                    if (_editable != null)
                    {
                        _editable.PropertyChanged -= RefreshButtons;
                    }

                    _editable = value;
                    RaisePropertyChanged();

                    if (_editable != null)
                    {
                        _editable.PropertyChanged += RefreshButtons;
                    }

                    RefreshButtons(null, null);
                }
            }
        }

        private void RefreshButtons(object sender, PropertyChangedEventArgs e)
        {
            AddCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
        }

        private ConcurrentObservableCollection<MemberNamePair> _members;
        public ConcurrentObservableCollection<MemberNamePair> Members
        {
            get { return _members; }
            private set
            {
                _members = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new DelegateCommand(OnAdd, CanAdd);
                }
                return _addCommand;
            }
        }

        private bool CanAdd()
        {
            if (!Editable.Scheduled)
            {
                return !string.IsNullOrWhiteSpace(Editable.Name) && Editable.Date.HasValue;
            }

            return !string.IsNullOrWhiteSpace(Editable.Name);
        }

        private async void OnAdd()
        {
            try
            {
                Loading = true;

                if (Editable.Scheduled)
                {
                    var entity = ToScheduledOperation();
                    Editable.Id = await scheduledOperationDal.AddScheduledOperationAsync(entity);
                }
                else
                {
                    var entity = ToOneTimeOperation();
                    Editable.Id = await operationDal.AddOperationAsync(entity);
                }

                SelectedOperation = null;
                await RefreshOperations();
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

        private ScheduledOperation ToScheduledOperation()
        {
            var entity = new ScheduledOperation
            {
                Amount = Editable.Amount,
                Starts = Editable.Date,
                Ends = Editable.Ends,
                FamilyId = Id,
                MemberId = Editable.Member.Id,
                Name = Editable.Name,
                HasConstantDate = Editable.HasConstantDate,
                Day = Editable.DayDate.HasValue ? Editable.DayDate.Value.Day : (int?)null
            };

            if (Editable.Id.HasValue)
            {
                entity.Id = Editable.Id.Value;
            }

            return entity;
        }

        private Operation ToOneTimeOperation()
        {
            var entity = new Operation
            {
                Amount = Editable.Amount,
                Date = Editable.Date.Value,
                FamilyId = Id,
                MemberId = Editable.Member.Id,
                Name = Editable.Name,
                HasConstantDate = Editable.HasConstantDate
            };

            if (Editable.Id.HasValue)
            {
                entity.Id = Editable.Id.Value;
            }

            return entity;
        }

        private DelegateCommand<OperationViewModel> _deleteCommand;
        public DelegateCommand<OperationViewModel> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new DelegateCommand<OperationViewModel>(OnDelete);
                }
                return _deleteCommand;
            }
        }

        private async void OnDelete(OperationViewModel operation)
        {
            try
            {
                if (!interaction.YesNoQuestion("Czy na pewno chesz usunąć operację?"))
                {
                    return;
                }

                Loading = true;

                if (operation.Scheduled)
                {
                    await scheduledOperationDal.DeleteAsync(operation.Id.Value);
                }
                else
                {
                    await operationDal.DeleteAsync(operation.Id.Value);
                }

                await RefreshOperations();
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

        private DelegateCommand _updateCommand;
        public DelegateCommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new DelegateCommand(OnUpdate, () => CanAdd() && Editable.Id.HasValue);
                }
                return _updateCommand;
            }
        }

        private async void OnUpdate()
        {
            try
            {
                if (!interaction.YesNoQuestion("Czy na pewno chesz zmienić operację?"))
                {
                    return;
                }

                Loading = true;

                if (Editable.Scheduled)
                {
                    var entity = ToScheduledOperation();
                    await scheduledOperationDal.UpdateAsync(entity);
                }
                else
                {
                    var entity = ToOneTimeOperation();
                    await operationDal.UpdateAsync(entity);
                }

                await RefreshOperations();
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

        private DelegateCommand<OperationViewModel> _overrideCommand;
        public DelegateCommand<OperationViewModel> OverrideCommand
        {
            get
            {
                if (_overrideCommand == null)
                {
                    _overrideCommand = new DelegateCommand<OperationViewModel>(OnOverride);
                }
                return _overrideCommand;
            }
        }

        private void OnOverride(OperationViewModel operation)
        {
            try
            {
                navigation.ShowView<OperationOverridesView>(operation.Name, operation.Id.Value);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
        }
    }
}
