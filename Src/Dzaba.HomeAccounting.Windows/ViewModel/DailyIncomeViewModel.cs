using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.Engine;
using Dzaba.HomeAccounting.Windows.Model;
using Dzaba.Mvvm;
using Dzaba.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class DailyIncomeViewModel : BaseViewModel, IParameterized
    {
        private readonly IInteractionService interaction;
        private readonly IIncomeEngine incomeEngine;

        public DailyIncomeViewModel(IInteractionService interaction,
            IIncomeEngine incomeEngine)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(incomeEngine, nameof(incomeEngine));

            this.interaction = interaction;
            this.incomeEngine = incomeEngine;
        }

        private MonthIncomesParameter _parameter;
        public MonthIncomesParameter Parameter
        {
            get { return _parameter; }
            private set
            {
                _parameter = value;
                RaisePropertyChanged();
            }
        }

        public void SetParameter(object parameter)
        {
            Parameter = (MonthIncomesParameter)parameter;
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

        private DayReport[] _report;
        public DayReport[] Report
        {
            get { return _report; }
            private set
            {
                _report = value;
                RaisePropertyChanged();
            }
        }

        private DayReport _selectedReport;
        public DayReport SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
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

                var report = await incomeEngine.CalculateDailyAsync(Parameter.FamilyId, Parameter.Report.Date, Parameter.InitialSavings);
                Report = report.Reports;
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
