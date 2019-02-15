using Dzaba.HomeAccounting.DataBase.Contracts.Migration;
using System;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Migration
{
    public abstract class EfMigration : IMigration<DatabaseContext>
    {
        public abstract Version From { get; }

        public abstract Task MigrateAsync(DatabaseContext context);
    }
}
