using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;
using System;
using System.IO;

namespace Dzaba.HomeAccounting.Windows
{
    internal sealed class ConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return SqliteProvider.CreateConnectionString(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.db"));
        }
    }
}
