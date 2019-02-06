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
    internal sealed class OperationDal : IOperationDal
    {
        private readonly Func<DatabaseContext> dbContextFactory;

        public OperationDal(Func<DatabaseContext> dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Operation[]> GetOperationsAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Operations
                    .Where(o => o.FamilyId == familyId)
                    .ToArrayAsync();
            }
        }

        public async Task<int> AddOperationAsync(Operation operation)
        {
            Require.NotNull(operation, nameof(operation));

            using (var dbContext = dbContextFactory())
            {
                dbContext.Operations.Add(operation);
                await dbContext.SaveChangesAsync();
                return operation.Id;
            } 
        }

        public async Task DeleteAsync(int id)
        {
            using (var dbContext = dbContextFactory())
            {
                var query = dbContext.Operations.Where(o => o.Id == id);
                dbContext.Operations.RemoveRange(query);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Operation operation)
        {
            using (var dbContext = dbContextFactory())
            {
                dbContext.Operations.Update(operation);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Operation[]> GetOperationsAsync(int familyId, YearAndMonth month)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Operations
                    .Where(o => o.FamilyId == familyId)
                    .Where(o => o.Date.Year == month.Year && o.Date.Month == month.Month)
                    .ToArrayAsync();
            }
        }
    }
}
