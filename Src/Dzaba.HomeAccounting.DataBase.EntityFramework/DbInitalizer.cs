using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Migration;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    internal sealed class DbInitalizer : IDbInitializer
    {
        private readonly Func<DatabaseContext> dbContextFactory;
        private readonly IMigrationManager<DatabaseContext> migrationManager;

        public DbInitalizer(Func<DatabaseContext> dbContextFactory,
            IMigrationManager<DatabaseContext> migrationManager)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));
            Require.NotNull(migrationManager, nameof(migrationManager));

            this.dbContextFactory = dbContextFactory;
            this.migrationManager = migrationManager;
        }

        public async Task InitializeAsync()
        {
            using (var dbContext = dbContextFactory())
            {
                if (await dbContext.Database.EnsureCreatedAsync())
                {
                    SetVersion(dbContext);
                    await dbContext.SaveChangesAsync();
                }
                else if (dbContext.GetVersion() != DbConsts.DatabaseVersion)
                {
                    await migrationManager.MigrateAsync(dbContext);

                    SetVersion(dbContext);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private void SetVersion(DatabaseContext dbContext)
        {
            var data = dbContext.DatabaseData.FirstOrDefault();
            if (data == null)
            {
                data = new DatabaseData();
            }
            data.Version = DbConsts.DatabaseVersion;
            dbContext.DatabaseData.Update(data);
        }
    }
}
