using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Windows.Model;
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
        private IReadOnlyDictionary<int, string> members;

        public OperationsViewModel(IInteractionService interaction,
            IOperationDal operationDal,
            IScheduledOperationDal scheduledOperationDal,
            IFamilyMembersDal membersDal)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(operationDal, nameof(operationDal));
            Require.NotNull(scheduledOperationDal, nameof(scheduledOperationDal));
            Require.NotNull(membersDal, nameof(membersDal));

            this.interaction = interaction;
            this.operationDal = operationDal;
            this.scheduledOperationDal = scheduledOperationDal;
            this.membersDal = membersDal;

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
                Scheduled = false
            });
            var operVm2 = oper2.Select(o => new OperationViewModel
            {
                Id = o.Id,
                Amount = o.Amount,
                Date = o.Starts,
                Ends = o.Ends,
                Member = GetMemberPair(o.MemberId),
                Name = o.Name,
                Scheduled = true
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
                    
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void RefreshButtons(object sender, PropertyChangedEventArgs e)
        {
            AddCommand.RaiseCanExecuteChanged();
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
                    var entity = new ScheduledOperation
                    {
                        Amount = Editable.Amount,
                        Starts = Editable.Date,
                        Ends = Editable.Ends,
                        FamilyId = Id,
                        MemberId = Editable.Member.Id,
                        Name = Editable.Name
                    };
                    Editable.Id = await scheduledOperationDal.AddScheduledOperationAsync(entity);
                }
                else
                {
                    var entity = new Operation
                    {
                        Amount = Editable.Amount,
                        Date = Editable.Date.Value,
                        FamilyId = Id,
                        MemberId = Editable.Member.Id,
                        Name = Editable.Name
                    };
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
    }
}
