using Ninject;

namespace Dzaba.HomeAccounting.Utils
{
    public static class IocExtensions
    {
        public static void RegisterTransient<TInt, TImpl>(this IKernel container)
            where TImpl : TInt
        {
            Require.NotNull(container, nameof(container));

            container.Bind<TInt>().To<TImpl>().InTransientScope();
        }

        public static void RegisterSingleton<TInt, TImpl>(this IKernel container)
            where TImpl : TInt
        {
            Require.NotNull(container, nameof(container));

            container.Bind<TInt>().To<TImpl>().InSingletonScope();
        }
    }
}
