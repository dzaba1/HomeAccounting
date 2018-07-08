﻿using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public static class Bootstrapper
    {
        public static void RegisterEntityFramework(this IServiceCollection container)
        {
            container.AddTransient<IEntityConfiguration, FamilyConfiguration>();
            container.AddTransient<IEntityConfiguration, FamilyMemberConfiguration>();
            container.AddTransient<IEntityConfiguration, MonthConfiguration>();
            container.AddTransient<IEntityConfiguration, OperationConfiguration>();
            container.AddTransient<IEntityConfiguration, ScheduledOperationConfiguration>();
            container.AddTransient<IEntityConfiguration, ScheduledOperationOverrideConfiguration>();

            container.AddDbContext<DatabaseContext>(OptionsHandler, ServiceLifetime.Transient, ServiceLifetime.Transient);
        }

        private static void OptionsHandler(IServiceProvider container, DbContextOptionsBuilder optionsBuilder)
        {
            var provider = container.GetService<IEntityFrameworkProvider>();
            var connectionStringProvider = container.GetService<IConnectionStringProvider>();

            optionsBuilder.UseLazyLoadingProxies();
            provider.Register(connectionStringProvider.GetConnectionString(), optionsBuilder);
        }
    }
}