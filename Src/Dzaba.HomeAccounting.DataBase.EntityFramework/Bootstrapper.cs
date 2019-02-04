using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration;
using Dzaba.HomeAccounting.DataBase.EntityFramework.DAL;
using Dzaba.HomeAccounting.Utils;
using Dzaba.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using System;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public static class Bootstrapper
    {
        public static void RegisterEntityFramework(this IKernel container)
        {
            Require.NotNull(container, nameof(container));

            container.RegisterTransient<IEntityConfiguration, FamilyConfiguration>();
            container.RegisterTransient<IEntityConfiguration, FamilyMemberConfiguration>();
            container.RegisterTransient<IEntityConfiguration, MonthDataConfiguration>();
            container.RegisterTransient<IEntityConfiguration, OperationConfiguration>();
            container.RegisterTransient<IEntityConfiguration, ScheduledOperationConfiguration>();
            container.RegisterTransient<IEntityConfiguration, ScheduledOperationOverrideConfiguration>();

            container.RegisterTransient<IDbInitializer, DbInitalizer>();
            container.RegisterTransient<IFamilyDal, FamilyDal>();
            container.RegisterTransient<IMonthDataDal, MonthDataDal>();
            container.RegisterTransient<IOperationDal, OperationDal>();
            container.RegisterTransient<IScheduledOperationDal, ScheduledOperationDal>();

            RegisterDbContext(container);
        }

        private static void RegisterDbContext(IKernel container)
        {
            container.Bind<DbContextOptions>()
                .ToMethod(c =>
                {
                    DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
                    OptionsHandler(c.Kernel, optionsBuilder);
                    return optionsBuilder.Options;
                })
                .InSingletonScope();

            container.Bind<DatabaseContext>()
                .ToSelf()
                .InTransientScope();

            container.Bind<Func<DatabaseContext>>()
                .ToMethod(c => () =>
                {
                    var dbContext = c.Kernel.Get<DatabaseContext>();
                    dbContext.Database.EnsureCreated();
                    return dbContext;
                })
                .InTransientScope();
        }

        private static void OptionsHandler(IServiceProvider container, DbContextOptionsBuilder optionsBuilder)
        {
            var provider = container.GetRequiredService<IEntityFrameworkProvider>();
            var connectionStringProvider = container.GetRequiredService<IConnectionStringProvider>();

            optionsBuilder.UseLazyLoadingProxies();
            provider.Register(connectionStringProvider.GetConnectionString(), optionsBuilder);
        }
    }
}
