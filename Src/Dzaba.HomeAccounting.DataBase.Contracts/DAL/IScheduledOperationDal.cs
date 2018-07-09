using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IScheduledOperationDal
    {
        Task<ScheduledOperation[]> GetAllAsync(int familyId);
        Task<OperationOverride[]> GetOverridesForMonthAsync(int monthId);
        Task<int> AddScheduledOperationAsync(ScheduledOperation operation);
        Task OverrideAsync(int monthId, int operationId, decimal amount);
    }
}
