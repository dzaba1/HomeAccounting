using Dzaba.HomeAccounting.Utils;
using Ninject;

namespace Dzaba.HomeAccounting.Engine
{
    public static class Bootstrapper
    {
        public static void RegisterEngine(this IKernel container)
        {
            Require.NotNull(container, nameof(container));

            container.RegisterTransient<IIncomeEngine, IncomeEngine>();
        }
    }
}
