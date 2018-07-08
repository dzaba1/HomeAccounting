using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.Utils;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    internal sealed class DbInitalizer : IDbInitializer
    {
        private readonly IDatabaseContextFactory dbContextFactory;

        public DbInitalizer(IDatabaseContextFactory dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task InitializeAsync()
        {
            using (var dbContext = dbContextFactory.Create())
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
        }
    }
}
