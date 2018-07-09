using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class MonthDal : IMonthDal
    {
        private readonly IDatabaseContextFactory dbContextFactory;

        public MonthDal(IDatabaseContextFactory dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddMonthAsync(Month month)
        {
            Require.NotNull(month, nameof(month));

            using (var dbContext = dbContextFactory.Create())
            {
                dbContext.Months.Add(month);
                await dbContext.SaveChangesAsync();
                return month.Id;
            }
        }

        public async Task<Month[]> GetMonthsAsync(int familyId, DateTime start, DateTime end)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                return await dbContext.Months
                    .Where(m => m.FamilyId == familyId && m.Date >= start && m.Date <= end)
                    .ToArrayAsync();
            }
        }
    }
}
