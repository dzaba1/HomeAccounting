using Dzaba.HomeAccounting.DataBase.Contracts.Migration;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite.Migration;
using Dzaba.HomeAccounting.Utils;
using Dzaba.Utils;
using Ninject;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite
{
    public static class Bootstrapper
    {
        public static void RegisterSqlite(this IKernel container)
        {
            Require.NotNull(container, nameof(container));

            container.RegisterTransient<IEntityFrameworkProvider, SqliteProvider>();
            container.RegisterTransient<IMigration<DatabaseContext>, DatabaseDataMigration>();
            container.RegisterTransient<IMigration<DatabaseContext>, HasConstantDateMigration>();
        }
    }
}
