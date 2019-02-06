using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IOperationDal
    {
        Task<Operation[]> GetOperationsAsync(int familyId, YearAndMonth month);
        Task<Operation[]> GetOperationsAsync(int familyId);
        Task<int> AddOperationAsync(Operation operation);
        Task DeleteAsync(int id);
        Task UpdateAsync(Operation operation);
    }
}
