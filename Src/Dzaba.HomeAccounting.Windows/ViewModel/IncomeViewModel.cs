using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.Engine;
using Dzaba.Mvvm;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class IncomeViewModel : BaseViewModel, IParameterized
    {
        private readonly IInteractionService interaction;
        private readonly IIncomeEngine incomeEngine;

        public IncomeViewModel(IInteractionService interaction,
            IIncomeEngine incomeEngine)
        {
            Require.NotNull(interaction, nameof(interaction));
            Require.NotNull(incomeEngine, nameof(incomeEngine));

            this.interaction = interaction;
            this.incomeEngine = incomeEngine;
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

        //private DelegateCommand _loadedCommand;
        //public DelegateCommand LoadedCommand
        //{
        //    get
        //    {
        //        if (_loadedCommand == null)
        //        {
        //            _loadedCommand = new DelegateCommand(OnLoaded);
        //        }
        //        return _loadedCommand;
        //    }
        //}

        //private async void OnLoaded()
        //{
        //    try
        //    {
        //        Loading = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        interaction.ShowError(ex, "Error");
        //    }
        //    finally
        //    {
        //        Loading = false;
        //    }
        //}

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
            }
        }

        private DelegateCommand _calculateCommand;
        public DelegateCommand CalculateCommand
        {
            get
            {
                if (_calculateCommand == null)
                {
                    _calculateCommand = new DelegateCommand(OnCalculate, () => From.HasValue && To.HasValue);
                }
                return _calculateCommand;
            }
        }

        private async void OnCalculate()
        {
            try
            {
                Loading = true;
                var report = await incomeEngine.CalculateAsync(Id, new YearAndMonth(From.Value), new YearAndMonth(To.Value));
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

        private MonthReport[] _report;
        public MonthReport[] Report
        {
            get { return _report; }
            private set
            {
                _report = value;
                RaisePropertyChanged();
            }
        }
    }
}
