using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Migration
{
    public interface IMigrationManager<in T>
    {
        Task MigrateAsync(T context);
    }
}
