using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class ScheduledOperationDal : IScheduledOperationDal
    {
        private readonly Func<DatabaseContext> dbContextFactory;

        public ScheduledOperationDal(Func<DatabaseContext> dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddScheduledOperationAsync(ScheduledOperation operation)
        {
            Require.NotNull(operation, nameof(operation));

            using (var dbContext = dbContextFactory())
            {
                dbContext.ScheduledOperations.Add(operation);
                await dbContext.SaveChangesAsync();
                return operation.Id;
            }
        }

        public async Task<ScheduledOperation[]> GetAllAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.ScheduledOperations
                    .Where(o => o.FamilyId == familyId)
                    .ToArrayAsync();
            }
        }

        public async Task<OperationOverride[]> GetOverridesForMonthAsync(int familyId, YearAndMonth month)
        {
            using (var dbContext = dbContextFactory())
            {
                var query = from ov in dbContext.ScheduledOperationOverrides
                            join o in dbContext.ScheduledOperations on ov.OperationId equals o.Id
                            where o.FamilyId == familyId && ov.Year == month.Year && ov.Month == month.Month
                            select new { ov.Year, ov.Month, ov.OperationId, ov.Amount, o.MemberId, OriginalAmount = o.Amount };

                var data = await query.ToArrayAsync();
                return data.Select(o => new OperationOverride
                {
                    Amount = o.Amount,
                    MemberId = o.MemberId,
                    Month = new YearAndMonth(o.Year, o.Month),
                    OperationId = o.OperationId,
                    OriginalAmount = o.OriginalAmount
                }).ToArray();
            }
        }

        public async Task OverrideAsync(YearAndMonth month, int operationId, decimal amount)
        {
            using (var dbContext = dbContextFactory())
            {
                var existing = await dbContext.ScheduledOperationOverrides.FirstOrDefaultAsync(o => o.Year == month.Year && o.Month == month.Month && o.OperationId == operationId);
                if (existing == null)
                {
                    existing = new ScheduledOperationOverride
                    {
                        Amount = amount,
                        Year = month.Year,
                        Month = month.Month,
                        OperationId = operationId
                    };
                    dbContext.ScheduledOperationOverrides.Add(existing);
                }
                else
                {
                    existing.Amount = amount;
                    dbContext.ScheduledOperationOverrides.Update(existing);
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var dbContext = dbContextFactory())
            {
                var query = dbContext.ScheduledOperations.Where(o => o.Id == id);
                dbContext.ScheduledOperations.RemoveRange(query);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(ScheduledOperation operation)
        {
            using (var dbContext = dbContextFactory())
            {
                dbContext.ScheduledOperations.Update(operation);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<ScheduledOperationOverride[]> GetOverridesForOperationAsync(int operationId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.ScheduledOperationOverrides
                    .Where(o => o.OperationId == operationId)
                    .OrderByDescending(o => o.Year)
                    .ThenByDescending(o => o.Month)
                    .ToArrayAsync();
            }
        }

        public async Task<ScheduledOperation> GetAsync(int id)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.ScheduledOperations
                    .FirstOrDefaultAsync(o => o.Id == id);
            }
        }
    }
}
