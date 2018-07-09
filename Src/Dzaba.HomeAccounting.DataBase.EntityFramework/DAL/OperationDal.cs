﻿using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class OperationDal : IOperationDal
    {
        private readonly IDatabaseContextFactory dbContextFactory;

        public OperationDal(IDatabaseContextFactory dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddOperationAsync(Operation operation)
        {
            Require.NotNull(operation, nameof(operation));

            using (var dbContext = dbContextFactory.Create())
            {
                dbContext.Operations.Add(operation);
                await dbContext.SaveChangesAsync();
                return operation.Id;
            } 
        }

        public async Task<Operation[]> GetOperationsAsync(int monthId)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                return await dbContext.Operations
                    .Where(o => o.MonthId == monthId)
                    .ToArrayAsync();
            }
        }
    }
}