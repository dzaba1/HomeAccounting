using System;
using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class MonthReport
    {
        public DateTime Date { get; set; }

        public decimal Income { get; set; }

        public decimal Sum { get; set; }

        public List<OperationReport> Operations { get; set; } = new List<OperationReport>();

        public List<FamilyMemberReport> MembersReports { get; set; } = new List<FamilyMemberReport>();
    }
}
