using Dzaba.HomeAccounting.Windows.Model;
using Dzaba.Mvvm;
using Dzaba.Mvvm.Windows;
using Dzaba.Mvvm.Windows.Navigation;
using Dzaba.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dzaba.HomeAccounting.Windows.ViewModel
{
    public interface IBreadcrumbService
    {
        void Add<T>(string crumb, object parameters) where T : FrameworkElement;
    }

    internal sealed class BreadcrumbService : BaseViewModel, IBreadcrumbService
    {
        private readonly INavigationService navigation;
        private readonly IInteractionService interaction;

        public BreadcrumbService(INavigationService navigation,
            IInteractionService interaction)
        {
            Require.NotNull(navigation, nameof(navigation));
            Require.NotNull(interaction, nameof(interaction));

            this.navigation = navigation;
            this.interaction = interaction;
        }

        public ConcurrentObservableCollection<Crumb> Crumbs { get; } = new ConcurrentObservableCollection<Crumb>();

        public void Add<T>(string crumb, object parameters) where T : FrameworkElement
        {
            Crumbs.Add(new Crumb(crumb, typeof(T), parameters));
        }

        private DelegateCommand<Crumb> _navigateCommand;
        public DelegateCommand<Crumb> NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new DelegateCommand<Crumb>(OnNavigate);
                }

                return _navigateCommand;
            }
        }

        private void OnNavigate(Crumb crumb)
        {
            try
            {
                var index = Crumbs.IndexOf(crumb);
                var toRemove = Crumbs.Skip(index + 1).ToArray();
                foreach (var item in toRemove)
                {
                    Crumbs.Remove(item);
                }
                navigation.ShowView(crumb.ViewType, crumb.Parameters);
            }
            catch (Exception ex)
            {
                interaction.ShowError(ex, "Error");
            }
        }
    }
}
