using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Dzaba.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class MonthDataDal : IMonthDataDal
    {
        private readonly Func<DatabaseContext> dbContextFactory;

        public MonthDataDal(Func<DatabaseContext> dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<MonthData> GetMonthDataAsync(int familyId, YearAndMonth month)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Months
                    .FirstOrDefaultAsync(m => m.FamilyId == familyId && m.Year == month.Year && m.Month == month.Month);
            }
        }

        public async Task<MonthData[]> GetMonthsAsync(int familyId, YearAndMonth start, YearAndMonth end)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Months
                    .Where(m => m.FamilyId == familyId)
                    .Where(IsGreaterOrEqual(start))
                    .Where(IsSmallerOrEqual(end))
                    .ToArrayAsync();
            }
        }

        private Expression<Func<MonthData, bool>> IsSmallerOrEqual(YearAndMonth date)
        {
            return m => m.Year < date.Year || m.Year == date.Year && m.Month <= date.Month;
        }

        private Expression<Func<MonthData, bool>> IsGreaterOrEqual(YearAndMonth date)
        {
            return m => m.Year > date.Year || m.Year == date.Year && m.Month >= date.Month;
        }
    }
}
