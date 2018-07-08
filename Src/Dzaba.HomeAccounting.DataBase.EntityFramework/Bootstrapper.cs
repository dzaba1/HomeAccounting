using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public static class Bootstrapper
    {
        public static void RegisterEntityFramework(this IServiceCollection container)
        {
            container.AddDbContext<DatabaseContext>(OptionsHandler, ServiceLifetime.Transient, ServiceLifetime.Transient);
        }

        private static void OptionsHandler(IServiceProvider container, DbContextOptionsBuilder optionsBuilder)
        {
            var provider = container.GetService<IEntityFrameworkProvider>();
            optionsBuilder.UseLazyLoadingProxies();
            provider.Register(optionsBuilder);
        }
    }
}
