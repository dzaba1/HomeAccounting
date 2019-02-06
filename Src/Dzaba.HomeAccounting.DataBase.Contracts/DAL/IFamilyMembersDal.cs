using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IFamilyMembersDal
    {
        Task<IReadOnlyDictionary<int, string>> GetMemberNamesAsync(int familyId);
        Task<int?> FindMemberId(int familyId, string name);
        Task<int> AddMemeberAsync(int familyId, string name);
        Task DeleteMemberAsync(int id);
    }
}
