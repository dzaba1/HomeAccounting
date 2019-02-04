using Dzaba.HomeAccounting.Windows.View;
using Dzaba.Mvvm.Windows;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using System;
using System.Windows;

namespace Dzaba.HomeAccounting.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppHandler.OnStartup<MainWindow, SelectFamilyView>(this, Bootstrapper.CreateContainer);
        }
    }
}
