using Dzaba.HomeAccounting.DataBase.EntityFramework.Migration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite.Migration
{
    internal sealed class DatabaseDataMigration : EfMigration
    {
        public override Version From { get; } = new Version(1, 0, 0, 0);

        public override Task MigrateAsync(DatabaseContext context)
        {
            var sql = @"CREATE TABLE ""DatabaseData"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_DatabaseData"" PRIMARY KEY AUTOINCREMENT,
    ""Version"" TEXT NOT NULL);";

            return context.Database.ExecuteSqlCommandAsync(sql);
        }
    }
}
