using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IOperationDal
    {
        Task<Operation[]> GetOperationsAsync(int monthId);
    }
}
