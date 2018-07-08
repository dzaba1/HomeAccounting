using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IMonthDal
    {
        Task<Month[]> GetMonthsAsync(int familyId, DateTime start, DateTime end);
    }
}
