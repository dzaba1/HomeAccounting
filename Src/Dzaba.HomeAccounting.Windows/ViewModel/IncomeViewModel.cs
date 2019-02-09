using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Engine;
using Dzaba.Mvvm;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class IncomeViewModel : BaseViewModel, IParameterized
    {
        private readonly IInteractionService interaction;
        private readonly IIncomeEngine incomeEngine;
        private readonly IMonthDataDal monthsDal;

        public IncomeViewModel(IInteractionService interaction,
            IIncomeEngine incomeEngine,
            IMonthDataDal monthsDal)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(incomeEngine, nameof(incomeEngine));
            Require.NotNull(monthsDal, nameof(monthsDal));

            this.interaction = interaction;
            this.incomeEngine = incomeEngine;
            this.monthsDal = monthsDal;
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

        private DateTime? _from;
        public DateTime? From
        {
            get { return _from; }
            set
            {
                _from = value;
                RaisePropertyChanged();
                CalculateCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _to;
        public DateTime? To
        {
            get { return _to; }
            set
            {
                _to = value;
                RaisePropertyChanged();
                CalculateCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _calculateCommand;
        public DelegateCommand CalculateCommand
        {
            get
            {
                if (_calculateCommand == null)
                {
                    _calculateCommand = new DelegateCommand(OnCalculate, CanCalculate);
                }
                return _calculateCommand;
            }
        }

        private bool CanCalculate()
        {
            return From.HasValue && To.HasValue;
        }

        private async Task RecalculateAsync()
        {
            var start = new YearAndMonth(From.Value);
            var end = new YearAndMonth(To.Value);

            SelectedReport = null;
            var monthsDict = (await monthsDal.GetMonthsAsync(Id, start, end))
                .ToDictionary(x => x.YearAndMonth);
            var report = await incomeEngine.CalculateAsync(Id, start, end);
            Report = report.Reports
                .Select(r => ToReportViewModel(r, monthsDict))
                .ToArray();
        }

        private async void OnCalculate()
        {
            try
            {
                Loading = true;
                await RecalculateAsync();
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

        private MonthReportViewModel ToReportViewModel(MonthReport report, IReadOnlyDictionary<YearAndMonth, MonthData> monthsDict)
        {
            var vm = new MonthReportViewModel(report);
            if (monthsDict.TryGetValue(report.Date, out var data))
            {
                vm.Data = data;
                vm.IsNew = false;
            }
            else
            {
                vm.Data = new MonthData
                {
                    FamilyId = Id,
                    YearAndMonth = report.Date
                };
                vm.IsNew = true;
            }
            return vm;
        }

        private MonthReportViewModel[] _report;
        public MonthReportViewModel[] Report
        {
            get { return _report; }
            private set
            {
                _report = value;
                RaisePropertyChanged();
            }
        }

        private MonthReportViewModel _selectedReport;
        public MonthReportViewModel SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsChangeEnabled));
            }
        }

        public bool IsChangeEnabled => SelectedReport != null;

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(OnSave, CanCalculate);
                }
                return _saveCommand;
            }
        }

        private async void OnSave()
        {
            try
            {
                Loading = true;

                if (SelectedReport.IsNew)
                {
                    await monthsDal.AddDataAsync(SelectedReport.Data);
                }
                else
                {
                    await monthsDal.UpdateDataAsync(SelectedReport.Data);
                }

                await RecalculateAsync();
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
