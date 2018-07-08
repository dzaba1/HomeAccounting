using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IScheduledOperationDal
    {
        Task<ScheduledOperation[]> GetAllAsync(int familyId);
        Task<OperationOverride[]> GetOverridesForMonth(int monthId);
    }
}
