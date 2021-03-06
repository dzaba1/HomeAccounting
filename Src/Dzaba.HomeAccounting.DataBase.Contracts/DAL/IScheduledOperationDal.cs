﻿using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IScheduledOperationDal
    {
        Task<ScheduledOperation[]> GetAllAsync(int familyId);
        Task<OperationOverride[]> GetOverridesForMonthAsync(int familyId, YearAndMonth month);
        Task<ScheduledOperationOverride[]> GetOverridesForOperationAsync(int operationId);
        Task<int> AddScheduledOperationAsync(ScheduledOperation operation);
        Task OverrideAsync(YearAndMonth month, int operationId, decimal amount);
        Task DeleteAsync(int id);
        Task UpdateAsync(ScheduledOperation operation);
        Task<ScheduledOperation> GetAsync(int id);
    }
}
