using Dzaba.HomeAccounting.DataBase.EntityFramework.Migration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite.Migration
{
    internal sealed class HasConstantDateMigration : EfMigration
    {
        public override Version From { get; } = new Version(1, 0, 0, 0);

        public override async Task MigrateAsync(DatabaseContext context)
        {
            var sql = $@"ALTER TABLE ""Operations"" ADD COLUMN ""HasConstantDate"" INTEGER NOT NULL DEFAULT 1";
            await context.Database.ExecuteSqlCommandAsync(sql);

            sql = $@"ALTER TABLE ""ScheduledOperations"" ADD COLUMN ""HasConstantDate"" INTEGER NOT NULL DEFAULT 1";
            await context.Database.ExecuteSqlCommandAsync(sql);
        }
    }
}
