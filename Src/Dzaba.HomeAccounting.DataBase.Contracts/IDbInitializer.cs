using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
