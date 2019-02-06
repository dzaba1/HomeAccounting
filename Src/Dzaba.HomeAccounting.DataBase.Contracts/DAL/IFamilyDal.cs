using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.DAL
{
    public interface IFamilyDal
    {
        Task<string> GetNameAsync(int familyId);
        Task<Family> GetFamilyAsync(int familyId);
        Task<int> AddFamilyAsync(string name, IEnumerable<string> members);
        Task<int> AddFamilyAsync(string name);
        Task<int?> FindFamilyId(string name);
        Task<Family[]> GetAllAsync();
        Task<IReadOnlyDictionary<int, string>> GetAllNamesAsync();
        Task DeleteFamilyAsync(int id);
    }
}
