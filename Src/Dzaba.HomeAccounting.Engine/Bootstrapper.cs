using Dzaba.HomeAccounting.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Dzaba.HomeAccounting.Engine
{
    public static class Bootstrapper
    {
        public static void RegisterEngine(this IServiceCollection container)
        {
            Require.NotNull(container, nameof(container));

            container.AddTransient<IIncomeEngine, IncomeEngine>();
        }
    }
}
