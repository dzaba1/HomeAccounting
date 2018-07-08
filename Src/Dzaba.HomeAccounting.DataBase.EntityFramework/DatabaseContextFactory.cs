using Dzaba.HomeAccounting.Utils;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    internal interface IDatabaseContextFactory
    {
        DatabaseContext Create();
    }

    internal sealed class DatabaseContextFactory : IDatabaseContextFactory
    {
        private readonly IServiceProvider container;

        public DatabaseContextFactory(IServiceProvider container)
        {
            Require.NotNull(container, nameof(container));

            this.container = container;
        }

        public DatabaseContext Create()
        {
            var dbContext = container.GetRequiredService<DatabaseContext>();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
