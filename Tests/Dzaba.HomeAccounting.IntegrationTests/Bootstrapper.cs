using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;
using Dzaba.HomeAccounting.Engine;
using Dzaba.HomeAccounting.Utils;
using Ninject;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    public static class Bootstrapper
    {
        public static IKernel CreateContainer()
        {
            var container = new StandardKernel();
            container.RegisterEntityFramework();
            container.RegisterSqlite();
            container.RegisterEngine();
            container.RegisterIntegrationTests();

            return container;
        }

        public static void RegisterIntegrationTests(this IKernel container)
        {
            container.RegisterTransient<IConnectionStringProvider, ConnectionStringProvider>();
            container.RegisterTransient<IInvokerDal, InvokerDal>();
        }
    }
}
