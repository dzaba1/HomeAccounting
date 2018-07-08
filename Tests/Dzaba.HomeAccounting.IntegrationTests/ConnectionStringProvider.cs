using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    internal sealed class ConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return SqliteProvider.CreateConnectionString(DbUtils.Location);
        }
    }
}
