using Dzaba.HomeAccounting.Utils;
using Microsoft.EntityFrameworkCore;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite
{
    public sealed class SqliteProvider : IEntityFrameworkProvider
    {
         public void Register(string connectionString, DbContextOptionsBuilder optionsBuilder)
        {
            Require.NotWhiteSpace(connectionString, nameof(connectionString));
            Require.NotNull(optionsBuilder, nameof(optionsBuilder));

            optionsBuilder.UseSqlite(connectionString);
        }

        public static string CreateConnectionString(string location)
        {
            Require.NotEmpty(location, nameof(location));

            return $"Data Source={location}";
        }
    }
}
