using Dzaba.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.Utils;
using Dzaba.Mvvm.Windows;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MembersViewModel : BaseViewModel, IParameterized
    {
        private readonly IInteractionService interaction;
        private readonly IFamilyMembersDal familyMembersDal;

        public MembersViewModel(IInteractionService interaction,
            IFamilyMembersDal familyMembersDal)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(familyMembersDal, nameof(familyMembersDal));

            this.interaction = interaction;
            this.familyMembersDal = familyMembersDal;
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

        private async void OnLoaded()
        {
            try
            {
                Loading = true;
                var dict = await familyMembersDal.GetMemberNamesAsync(Id);
                Members = new ConcurrentObservableCollection<KeyValuePair<int, string>>(dict);
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

        public void SetParameter(object parameter)
        {
            Id = (int)parameter;
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

        private ConcurrentObservableCollection<KeyValuePair<int, string>> _members;
        public ConcurrentObservableCollection<KeyValuePair<int, string>> Members
        {
            get { return _members; }
            private set
            {
                _members = value;
                RaisePropertyChanged();
            }
        }

        private int? _selectedMemberId;
        public int? SelectedMemberId
        {
            get { return _selectedMemberId; }
            set
            {
                _selectedMemberId = value;
                RaisePropertyChanged();
            }
        }

        private string _new;
        public string New
        {
            get { return _new; }
            set
            {
                _new = value;
                RaisePropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _addCommand;

        public DelegateCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new DelegateCommand(OnAdd, () => !string.IsNullOrWhiteSpace(New));
                }

                return _addCommand;
            }
        }

        private async void OnAdd()
        {
            try
            {
                Loading = true;
                var id = await familyMembersDal.AddMemeberAsync(Id, New);          
                Members.Add(new KeyValuePair<int, string>(id, New));
                New = null;
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

        private DelegateCommand<int> _deleteMemberCommand;

        public DelegateCommand<int> DeleteMemberCommand
        {
            get
            {
                if (_deleteMemberCommand == null)
                {
                    _deleteMemberCommand = new DelegateCommand<int>(OnDelete);
                }

                return _deleteMemberCommand;
            }
        }

        private async void OnDelete(int id)
        {
            try
            {
                if (!interaction.YesNoQuestion("Czy na pewno chcesz usunąć członka rodziny?"))
                {
                    return;
                }

                Loading = true;
                await familyMembersDal.DeleteMemberAsync(id);
                Members.RemoveWhere(m => m.Key == id);
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
