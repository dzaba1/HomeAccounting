using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
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
        Task<FamilyReport> CalculateAsync(int familyId, YearAndMonth month);
    }

    internal sealed class IncomeEngine : IIncomeEngine
    {
        private readonly MonthAndYearComparer comparer = new MonthAndYearComparer();
        private readonly IMonthDataDal monthDataDal;
        private readonly IFamilyDal familyDal;
        private readonly IScheduledOperationDal scheduledOperationDal;
        private readonly IOperationDal operationDal;
        private readonly IFamilyMembersDal familyMembersDal;

        public IncomeEngine(IMonthDataDal monthDataDal,
            IFamilyDal familyDal,
            IScheduledOperationDal scheduledOperationDal,
            IOperationDal operationDal,
            IFamilyMembersDal familyMembersDal)
        {
            Require.NotNull(monthDataDal, nameof(monthDataDal));
            Require.NotNull(familyDal, nameof(familyDal));
            Require.NotNull(scheduledOperationDal, nameof(scheduledOperationDal));
            Require.NotNull(operationDal, nameof(operationDal));
            Require.NotNull(familyMembersDal, nameof(familyMembersDal));

            this.monthDataDal = monthDataDal;
            this.familyDal = familyDal;
            this.scheduledOperationDal = scheduledOperationDal;
            this.operationDal = operationDal;
            this.familyMembersDal = familyMembersDal;
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

        public async Task<FamilyReport> CalculateAsync(int familyId, YearAndMonth month)
        {
            var reportTask = CalculateReportAsync(familyId, month);
            var familyTask = familyDal.GetNameAsync(familyId);

            return new FamilyReport
            {
                Reports = new[] { await reportTask },
                FamilyId = familyId,
                FamilyName = await familyTask
            };
        }

        private async Task<IncomeBuilderData> GetDataAsync(int familyId)
        {
            return new IncomeBuilderData
            {
                AllScheduledOperations = await scheduledOperationDal.GetAllAsync(familyId),
                FamilyId = familyId,
                MemberNames = await familyMembersDal.GetMemberNamesAsync(familyId)
            };
        }

        private async Task<MonthReport[]> CalculateReportsAsync(int familyId, YearAndMonth start, YearAndMonth end)
        {
            var familyMonths = await GetMonthsAsync(familyId, start, end);
            var data = await GetDataAsync(familyId);

            decimal sum = 0;
            var result = new List<MonthReport>();
            foreach (var month in EnumerateMonths(start, end))
            {
                var monthData = new CurrentMonthData
                {
                    MonthReport = new MonthReport
                    {
                        Date = month
                    },
                    Month = month
                };

                if (familyMonths.TryGetValue(month, out MonthData monthDataTemp))
                {
                    monthData.MonthData = monthDataTemp;
                };

                decimal income = await CalculateIncomeAsync(monthData, data);
                sum = CheckSum(monthData, sum, income);
                result.Add(monthData.MonthReport);
            }

            return result.ToArray();
        }

        private async Task<MonthReport> CalculateReportAsync(int familyId, YearAndMonth month)
        {
            var data = await GetDataAsync(familyId);
            var monthData = new CurrentMonthData
            {
                MonthReport = new MonthReport
                {
                    Date = month
                },
                Month = month,
                MonthData = await monthDataDal.GetMonthDataAsync(familyId, month)
            };

            decimal income = await CalculateIncomeAsync(monthData, data);
            CheckSum(monthData, income, income);
            return monthData.MonthReport;
        }

        private decimal CheckSum(CurrentMonthData monthData, decimal currentSum, decimal income)
        {
            decimal sum = currentSum;

            if (monthData.MonthData != null)
            {
                sum = CheckTotalOverride(sum, income, monthData.MonthData);
            }
            else
            {
                sum += income;
            }

            monthData.MonthReport.Sum = sum;
            return sum;
        }

        private async Task<decimal> CalculateIncomeAsync(CurrentMonthData monthData, IncomeBuilderData data)
        {
            decimal income = 0;

            if (monthData.MonthData != null && monthData.MonthData.IncomeOverride.HasValue)
            {
                income = monthData.MonthData.IncomeOverride.Value;
            }
            else
            {
                income = CheckScheduledOperations(data, monthData);
                income = await CheckOperationOverridesAsync(data, income, monthData);
                income += await CheckOperationsAsync(monthData, data);
                monthData.MonthReport.Income = income;
            }

            return income;
        }

        private async Task<decimal> CheckOperationsAsync(CurrentMonthData monthData, IncomeBuilderData data)
        {
            var operations = await operationDal.GetOperationsAsync(data.FamilyId, monthData.Month);
            return CheckOperations(operations, monthData.MonthReport, data);
        }

        private decimal CheckOperations(IEnumerable<IOperation> operations, MonthReport report, IncomeBuilderData data)
        {
            decimal income = 0;

            foreach (var operation in operations)
            {
                var operationReport = operation.ToOperationReport(report.Date);

                if (operation.MemberId.HasValue)
                {
                    var member = new KeyValuePair<int, string>(operation.MemberId.Value, data.MemberNames[operation.MemberId.Value]);                
                    operationReport.Member = member;
                }

                report.Operations.Add(operationReport);
                income += operation.Amount;
            }

            return income;
        }

        private async Task<decimal> CheckOperationOverridesAsync(IncomeBuilderData data, decimal income, CurrentMonthData monthData)
        {
            var currentIncome = income;
            var overrides = await scheduledOperationDal.GetOverridesForMonthAsync(data.FamilyId, monthData.Month);
            foreach (var overrideEntry in overrides)
            {
                OverrideOperation(monthData.MonthReport.Operations, overrideEntry);
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

        private decimal CheckScheduledOperations(IncomeBuilderData data, CurrentMonthData monthData)
        {
            var monthlyScheduledOperations = data.AllScheduledOperations.Where(o => IsActive(o, monthData.Month.ToDateTime()));
            return CheckOperations(monthlyScheduledOperations, monthData.MonthReport, data);
        }

        private bool IsActive(ScheduledOperation operation, DateTime month)
        {
            bool isActive = true;

            if (operation.Date.HasValue)
            {
                isActive = comparer.Compare(month, operation.Date.Value) >= 0;
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

        private IEnumerable<DateTime> EnumerateDays(YearAndMonth month)
        {
            var current = new DateTime(month.Year, month.Month, 1);
            while (current.Month == month.Month)
            {
                yield return current;
                current = current.AddDays(1);
            }
        }
    }
}
