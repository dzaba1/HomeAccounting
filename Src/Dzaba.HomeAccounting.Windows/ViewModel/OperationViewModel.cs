using System;
using System.Collections.Generic;
using Dzaba.HomeAccounting.Windows.Model;
using Dzaba.Mvvm;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class OperationViewModel : BaseViewModel
    {
        private int? _id;
        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanChangeType));
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        private MemberNamePair _member = Consts.DefaultMemberPair;
        public MemberNamePair Member
        {
            get { return _member; }
            set
            {
                _member = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? _ends;
        public DateTime? Ends
        {
            get { return _ends; }
            set
            {
                _ends = value;
                RaisePropertyChanged();
            }
        }

        private bool _scheduled;
        public bool Scheduled
        {
            get { return _scheduled; }
            set
            {
                _scheduled = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DayDateEnabled));
            }
        }

        private bool _hasConstantDate;
        public bool HasConstantDate
        {
            get { return _hasConstantDate; }
            set
            {
                _hasConstantDate = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DayDateEnabled));
            }
        }

        private DateTime? _dayDate;
        public DateTime? DayDate
        {
            get { return _dayDate; }
            set
            {
                _dayDate = value;
                RaisePropertyChanged();
            }
        }

        public bool DayDateEnabled => Scheduled && HasConstantDate;

        public bool CanChangeType => !Id.HasValue;
    }
}
