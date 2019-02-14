using Dzaba.HomeAccounting.Windows.View;
using Dzaba.Mvvm;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    internal sealed class MainViewModel : BaseViewModel
    {
        private readonly IBreadcrumbService breadcrumbService;

        public MainViewModel(IBreadcrumbService breadcrumbService)
        {
            Require.NotNull(breadcrumbService, nameof(breadcrumbService));

            this.breadcrumbService = breadcrumbService;

            breadcrumbService.Add<SelectFamilyView>("Rodziny", null);
        }
    }
}
