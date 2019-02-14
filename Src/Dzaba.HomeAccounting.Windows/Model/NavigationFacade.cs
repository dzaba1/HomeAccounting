using Dzaba.HomeAccounting.Windows.ViewModel;
using Dzaba.Mvvm.Windows.Navigation;
using Dzaba.Utils;
using System.Windows;

namespace Dzaba.HomeAccounting.Windows.Model
{
    public interface INavigationFacade
    {
        void ShowView<T>(string name, object arguments) where T : FrameworkElement;
    }

    internal sealed class NavigationFacade : INavigationFacade
    {
        private readonly INavigationService navigation;
        private readonly IBreadcrumbService breadcrumb;

        public NavigationFacade(INavigationService navigation,
            IBreadcrumbService breadcrumb)
        {
            Require.NotNull(navigation, nameof(navigation));
            Require.NotNull(breadcrumb, nameof(breadcrumb));

            this.navigation = navigation;
            this.breadcrumb = breadcrumb;
        }

        public void ShowView<T>(string name, object arguments)
            where T : FrameworkElement
        {
            breadcrumb.Add<T>(name, arguments);
            navigation.ShowView<T>(arguments);
        }
    }
}
