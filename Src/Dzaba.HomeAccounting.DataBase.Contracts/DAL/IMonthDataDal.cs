using Dzaba.HomeAccounting.Contracts;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IMonthDataDal
    {
        Task<MonthData> GetMonthDataAsync(int familyId, YearAndMonth month);
        Task<MonthData[]> GetMonthsAsync(int familyId, YearAndMonth start, YearAndMonth end);
    }
}
