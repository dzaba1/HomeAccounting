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
            try
            {
                var container = Bootstrapper.CreateContainer();

                var mainWindow = container.Get<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
