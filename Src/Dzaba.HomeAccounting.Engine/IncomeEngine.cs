using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.Engine
{
    public interface IIncomeEngine
    {
        Task<FamilyReport> CalculateAsync(int familyId, DateTime start, DateTime end);
    }

    internal sealed class IncomeEngine : IIncomeEngine
    {
        private readonly MonthAndYearComparer comparer = new MonthAndYearComparer();
        private readonly IMonthDal monthDal;
        private readonly IFamilyDal familyDal;
        private readonly IScheduledOperationDal scheduledOperationDal;
        private readonly IOperationDal operationDal;

        public IncomeEngine(IMonthDal monthDal,
            IFamilyDal familyDal,
            IScheduledOperationDal scheduledOperationDal,
            IOperationDal operationDal)
        {
            Require.NotNull(monthDal, nameof(monthDal));
            Require.NotNull(familyDal, nameof(familyDal));
            Require.NotNull(scheduledOperationDal, nameof(scheduledOperationDal));
            Require.NotNull(operationDal, nameof(operationDal));

            this.monthDal = monthDal;
            this.familyDal = familyDal;
            this.scheduledOperationDal = scheduledOperationDal;
            this.operationDal = operationDal;
        }

        public async Task<FamilyReport> CalculateAsync(int familyId, DateTime start, DateTime end)
        {
            var reportTask = CalculateReportsAsync(familyId, start, end);
            var familyTask = familyDal.GetNameAsync(familyId);

            return new FamilyReport
            {
                Reports = await reportTask,
                FamilyId = familyId,
                FamilyName = await familyTask
            };
        }

        private async Task<MonthReport[]> CalculateReportsAsync(int familyId, DateTime start, DateTime end)
        {
            var familyMonths = await GetMonthsAsync(familyId, start, end);
            var scheduledOperations = await scheduledOperationDal.GetAllAsync(familyId);
            var memberNames = await familyDal.GetMemberNamesAsync(familyId);

            decimal sum = 0;
            var result = new List<MonthReport>();
            foreach (var month in EnumerateMonths(start, end))
            {
                //decimal income = 0;
                var monthReport = new MonthReport
                {
                    Date = month,
                    MembersReports = memberNames
                        .Select(m => new FamilyMemberReport
                        {
                            Id = m.Key,
                            Name = m.Value
                        })
                        .ToList()
                };

                if (familyMonths.TryGetValue(month, out Month familyMonth))
                {
                    decimal income = 0;

                    if (familyMonth.IncomeOverride.HasValue)
                    {
                        income = familyMonth.IncomeOverride.Value;
                    }
                    else
                    {
                        income = CheckScheduledOperations(scheduledOperations, month, monthReport);
                        income = await CheckOperationOverridesAsync(familyMonth, income, scheduledOperations, monthReport);
                        income += await CheckOperationsAsync(familyMonth, monthReport);
                    }

                    sum = CheckTotalOverride(sum, income, familyMonth);
                    monthReport.Income = income;
                }
                else
                {
                    var income = CheckScheduledOperations(scheduledOperations, month, monthReport);
                    monthReport.Income = income;
                    sum += income;
                }

                monthReport.Sum = sum;
                result.Add(monthReport);
            }

            return result.ToArray();
        }

        private async Task<decimal> CheckOperationsAsync(Month month, MonthReport report)
        {
            var operations = await operationDal.GetOperationsAsync(month.Id);
            return CheckOperations(operations, report);
        }

        private decimal CheckOperations(IEnumerable<IOperation> operations, MonthReport report)
        {
            decimal income = 0;

            foreach (var operation in operations)
            {
                if (operation.MemberId.HasValue)
                {
                    var memberReport = report.MembersReports.FirstOrDefault(m => m.Id == operation.MemberId.Value);
                    if (memberReport != null)
                    {
                        memberReport.Income += operation.Amount;
                        memberReport.Operations.Add(operation.ToOperationReport());
                    }
                }
                else
                {
                    report.Operations.Add(operation.ToOperationReport());
                }

                income += operation.Amount;
            }

            return income;
        }

        private async Task<decimal> CheckOperationOverridesAsync(Month month, decimal income, ScheduledOperation[] scheduledOperations, MonthReport report)
        {
            var currentIncome = income;
            var overrides = await scheduledOperationDal.GetOverridesForMonth(month.Id);
            foreach (var overrideEntry in overrides)
            {
                if (overrideEntry.MemberId.HasValue)
                {
                    var memberReport = report.MembersReports.FirstOrDefault(m => m.Id == overrideEntry.MemberId.Value);
                    if (memberReport != null)
                    {
                        memberReport.Income = OverrideAmount(memberReport.Income, overrideEntry);
                        OverrideOperation(memberReport.Operations, overrideEntry);
                    }
                }
                else
                {
                    OverrideOperation(report.Operations, overrideEntry);
                }

                currentIncome = OverrideAmount(currentIncome, overrideEntry);
            }

            return currentIncome;
        }

        private decimal OverrideAmount(decimal amount, OperationOverride overrideEntry)
        {
            var currentAmount = amount;
            currentAmount -= overrideEntry.OriginalAmount;
            currentAmount += overrideEntry.Amount;
            return currentAmount;
        }

        private void OverrideOperation(List<OperationReport> operations, OperationOverride overrideEntry)
        {
            var operation = operations.FirstOrDefault(o => o.Id == overrideEntry.OperationId);
            if (operation != null)
            {
                operation.Amount = overrideEntry.Amount;
                operation.IsOverriden = true;
            }
        }

        private decimal CheckScheduledOperations(ScheduledOperation[] scheduledOperations, DateTime month, MonthReport report)
        {
            var monthlyScheduledOperations = scheduledOperations.Where(o => IsActive(o, month));
            return CheckOperations(monthlyScheduledOperations, report);
        }

        private bool IsActive(ScheduledOperation operation, DateTime month)
        {
            bool isActive = true;

            if (operation.Starts.HasValue)
            {
                isActive = comparer.Compare(month, operation.Starts.Value) >= 0;
            }

            if (isActive && operation.Ends.HasValue)
            {
                isActive = comparer.Compare(month, operation.Ends.Value) <= 0;
            }

            return isActive;
        }

        private static decimal CheckTotalOverride(decimal currentSum, decimal income, Month month)
        {
            var sum = currentSum;

            if (month.TotalOverride.HasValue)
            {
                sum = month.TotalOverride.Value;
            }
            else
            {
                sum += income;
            }

            return sum;
        }

        private async Task<IReadOnlyDictionary<DateTime, Month>> GetMonthsAsync(int familyId, DateTime start, DateTime end)
        {
            var months = await monthDal.GetMonthsAsync(familyId, start, end);
            return months.ToDictionary(m => m.Date.Date, comparer);
        }

        private IEnumerable<DateTime> EnumerateMonths(DateTime start, DateTime end)
        {
            var current = start;
            while (end >= current)
            {
                yield return current.Date;
                current.AddMonths(1);
            }
        }
    }
}
