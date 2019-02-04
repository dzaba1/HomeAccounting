using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Dzaba.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.Engine
{
    public interface IIncomeEngine
    {
        Task<FamilyReport> CalculateAsync(int familyId, YearAndMonth start, YearAndMonth end);
    }

    internal sealed class IncomeEngine : IIncomeEngine
    {
        private readonly MonthAndYearComparer comparer = new MonthAndYearComparer();
        private readonly IMonthDataDal monthDataDal;
        private readonly IFamilyDal familyDal;
        private readonly IScheduledOperationDal scheduledOperationDal;
        private readonly IOperationDal operationDal;

        public IncomeEngine(IMonthDataDal monthDataDal,
            IFamilyDal familyDal,
            IScheduledOperationDal scheduledOperationDal,
            IOperationDal operationDal)
        {
            Require.NotNull(monthDataDal, nameof(monthDataDal));
            Require.NotNull(familyDal, nameof(familyDal));
            Require.NotNull(scheduledOperationDal, nameof(scheduledOperationDal));
            Require.NotNull(operationDal, nameof(operationDal));

            this.monthDataDal = monthDataDal;
            this.familyDal = familyDal;
            this.scheduledOperationDal = scheduledOperationDal;
            this.operationDal = operationDal;
        }

        public async Task<FamilyReport> CalculateAsync(int familyId, YearAndMonth start, YearAndMonth end)
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

        private async Task<MonthReport[]> CalculateReportsAsync(int familyId, YearAndMonth start, YearAndMonth end)
        {
            var familyMonths = await GetMonthsAsync(familyId, start, end);
            var scheduledOperations = await scheduledOperationDal.GetAllAsync(familyId);
            var memberNames = await familyDal.GetMemberNamesAsync(familyId);

            decimal sum = 0;
            var result = new List<MonthReport>();
            foreach (var month in EnumerateMonths(start, end))
            {
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

                decimal income = 0;
                var predict = !familyMonths.TryGetValue(month, out MonthData monthData);

                if (!predict && monthData.IncomeOverride.HasValue)
                {
                    income = monthData.IncomeOverride.Value;
                }
                else
                {
                    income = CheckScheduledOperations(scheduledOperations, month, monthReport);
                    income = await CheckOperationOverridesAsync(familyId, month, income, scheduledOperations, monthReport);
                    income += await CheckOperationsAsync(familyId, month, monthReport);
                    monthReport.Income = income;
                }

                if (!predict)
                {
                    sum = CheckTotalOverride(sum, income, monthData);
                }
                else
                {
                    sum += income;
                }

                monthReport.Sum = sum;
                result.Add(monthReport);
            }

            return result.ToArray();
        }

        private async Task<decimal> CheckOperationsAsync(int familyId, YearAndMonth month, MonthReport report)
        {
            var operations = await operationDal.GetOperationsAsync(familyId, month);
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

        private async Task<decimal> CheckOperationOverridesAsync(int familyId, YearAndMonth month, decimal income, ScheduledOperation[] scheduledOperations, MonthReport report)
        {
            var currentIncome = income;
            var overrides = await scheduledOperationDal.GetOverridesForMonthAsync(familyId, month);
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

        private decimal CheckScheduledOperations(ScheduledOperation[] scheduledOperations, YearAndMonth month, MonthReport report)
        {
            var monthlyScheduledOperations = scheduledOperations.Where(o => IsActive(o, month.ToDateTime()));
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

        private static decimal CheckTotalOverride(decimal currentSum, decimal income, MonthData month)
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

        private async Task<IReadOnlyDictionary<YearAndMonth, MonthData>> GetMonthsAsync(int familyId, YearAndMonth start, YearAndMonth end)
        {
            var months = await monthDataDal.GetMonthsAsync(familyId, start, end);
            return months.ToDictionary(m => m.YearAndMonth);
        }

        private IEnumerable<YearAndMonth> EnumerateMonths(YearAndMonth start, YearAndMonth end)
        {
            var current = start;
            while (end >= current)
            {
                yield return current;
                current = current.AddMonths(1);
            }
        }
    }
}
