using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IFamilyDal
    {
        Task<string> GetNameAsync(int familyId);
        Task<Dictionary<int, string>> GetMemberNamesAsync(int familyId);
        Task<int> AddFamilyAsync(string name, IEnumerable<string> members);
    }
}
