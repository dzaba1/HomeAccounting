using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;
using Dzaba.HomeAccounting.Engine;
using Dzaba.HomeAccounting.Utils;
using Dzaba.HomeAccounting.Windows.ViewModel;
using Ninject;
using System.ComponentModel;
using System.Windows;

namespace Dzaba.HomeAccounting.Windows
{
    internal static class Bootstrapper
    {
        public static IKernel CreateContainer()
        {
            var container = new StandardKernel();
            container.RegisterEntityFramework();
            container.RegisterSqlite();
            container.RegisterEngine();
            container.RegisterApp();

            return container;
        }

        private static void RegisterApp(this IKernel container)
        {
            container.RegisterTransient<IConnectionStringProvider, ConnectionStringProvider>();

            container.RegisterView<MainWindow, MainViewModel>(true);
        }

        private static void RegisterView<TView, TViewModel>(this IKernel container, bool isSingleton)
            where TView : FrameworkElement
            where TViewModel : INotifyPropertyChanged
        {
            if (isSingleton)
            {
                container.Bind<TView>()
                    .ToSelf()
                    .InSingletonScope()
                    .WithPropertyValue("DataContext", c => c.Kernel.Get<TViewModel>());
            }
            else
            {
                container.Bind<TView>()
                    .ToSelf()
                    .InTransientScope()
                    .WithPropertyValue("DataContext", c => c.Kernel.Get<TViewModel>());
            }
        }
    }
}
