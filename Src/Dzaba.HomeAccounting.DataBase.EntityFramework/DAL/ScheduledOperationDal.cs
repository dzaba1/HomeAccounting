using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class ScheduledOperationDal : IScheduledOperationDal
    {
        private readonly IDatabaseContextFactory dbContextFactory;

        public ScheduledOperationDal(IDatabaseContextFactory dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddScheduledOperationAsync(ScheduledOperation operation)
        {
            Require.NotNull(operation, nameof(operation));

            using (var dbContext = dbContextFactory.Create())
            {
                dbContext.ScheduledOperations.Add(operation);
                await dbContext.SaveChangesAsync();
                return operation.Id;
            }
        }

        public async Task<ScheduledOperation[]> GetAllAsync(int familyId)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                return await dbContext.ScheduledOperations
                    .Where(o => o.FamilyId == familyId)
                    .ToArrayAsync();
            }
        }

        public async Task<OperationOverride[]> GetOverridesForMonthAsync(int monthId)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                var query = from ov in dbContext.ScheduledOperationOverrides
                            join o in dbContext.ScheduledOperations on ov.OperationId equals o.Id
                            where ov.MonthId == monthId
                            select new { ov.MonthId, ov.OperationId, ov.Amount, o.MemberId, OriginalAmount = o.Amount };

                var data = await query.ToArrayAsync();
                return data.Select(o => new OperationOverride
                {
                    Amount = o.Amount,
                    MemberId = o.MemberId,
                    MonthId = o.MonthId,
                    OperationId = o.OperationId,
                    OriginalAmount = o.OriginalAmount
                }).ToArray();
            }
        }

        public async Task OverrideAsync(int monthId, int operationId, decimal amount)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                var existing = await dbContext.ScheduledOperationOverrides.FirstOrDefaultAsync(o => o.MonthId == monthId && o.OperationId == operationId);
                if (existing == null)
                {
                    existing = new ScheduledOperationOverride
                    {
                        Amount = amount,
                        MonthId = monthId,
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
    }
}
