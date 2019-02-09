using Dzaba.HomeAccounting.DataBase.Contracts;
using Dzaba.HomeAccounting.DataBase.EntityFramework;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Sqlite;
using Dzaba.HomeAccounting.Engine;
using Dzaba.HomeAccounting.Utils;
using Dzaba.HomeAccounting.Windows.View;
using Dzaba.HomeAccounting.Windows.ViewModel;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Windows;
using Ninject;

namespace Dzaba.HomeAccounting.Windows
{
    internal static class Bootstrapper
    {
        public static IKernel CreateContainer()
        {
            var container = new StandardKernel();
            container.RegisterMvvm(new BootstrapOptions
            {
                LongOperationPopupSingleton = false
            });
            container.RegisterMvvmWindows();
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
            container.RegisterView<SelectFamilyView, SelectFamilyViewModel>();
            container.RegisterView<FamilyMainView, FamilyMainViewModel>();
            container.RegisterView<MembersView, MembersViewModel>();
            container.RegisterView<OperationsView, OperationsViewModel>();
            container.RegisterView<IncomeView, IncomeViewModel>();
            container.RegisterView<OperationOverridesView, OperationOverridesViewModel>();
        }
    }
}
