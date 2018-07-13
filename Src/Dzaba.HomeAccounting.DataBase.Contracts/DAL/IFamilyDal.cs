using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IFamilyDal
    {
        Task<string> GetNameAsync(int familyId);
        Task<Family> GetFamilyAsync(int familyId);
        Task<IReadOnlyDictionary<int, string>> GetMemberNamesAsync(int familyId);
        Task<int> AddFamilyAsync(string name, IEnumerable<string> members);
        Task<int?> FindFamilyId(string name);
        Task<int?> FindMemberId(int familyId, string name);
    }
}
