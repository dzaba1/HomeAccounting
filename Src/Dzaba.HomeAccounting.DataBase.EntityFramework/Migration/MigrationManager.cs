using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Migration;
using Dzaba.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Migration
{
    internal sealed class MigrationManager : IMigrationManager<DatabaseContext>
    {
        private readonly IMigration<DatabaseContext>[] migrations;

        public MigrationManager(IEnumerable<IMigration<DatabaseContext>> migrations)
        {
            Require.NotNull(migrations, nameof(migrations));

            this.migrations = migrations.ToArray();
        }

        public async Task MigrateAsync(DatabaseContext context)
        {
            var currentVersion = context.GetVersion();
            var versionsToMigrate = migrations
                .Select(x => x.From)
                .Where(v => v >= currentVersion)
                .Where(v => v < DbConsts.DatabaseVersion)
                .Distinct()
                .OrderBy(v => v);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var version in versionsToMigrate)
                    {
                        var currentMigrations = migrations.Where(x => x.From == version);
                        foreach (var currentMigration in currentMigrations)
                        {
                            await currentMigration.MigrateAsync(context);
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
