using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;
using Dzaba.HomeAccounting.Engine;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dzaba.HomeAccounting.CmdTest
{
    internal static class Bootstrapper
    {
        public static IServiceProvider CreateContainer()
        {
            var container = new ServiceCollection();
            container.RegisterEntityFramework();
            container.RegisterSqlite();
            container.RegisterEngine();
            container.RegisterCmd();

            return container.BuildServiceProvider();
        }

        public static void RegisterCmd(this IServiceCollection container)
        {
            container.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
            container.AddTransient<IApp, App>();
        }
    }
}
