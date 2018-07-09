﻿using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    public static class Bootstrapper
    {
        public static IServiceProvider CreateContainer()
        {
            var container = new ServiceCollection();
            container.RegisterEntityFramework();
            container.RegisterSqlite();
            container.RegisterIntegrationTests();

            return container.BuildServiceProvider();
        }

        public static void RegisterIntegrationTests(this IServiceCollection container)
        {
            container.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
        }
    }
}