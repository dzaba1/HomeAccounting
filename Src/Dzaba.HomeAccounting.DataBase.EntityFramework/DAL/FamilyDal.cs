using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class FamilyDal : IFamilyDal
    {
        private readonly IDatabaseContextFactory dbContextFactory;

        public FamilyDal(IDatabaseContextFactory dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddFamilyAsync(string name, IEnumerable<string> members)
        {
            Require.NotWhiteSpace(name, nameof(name));
            Require.NotNull(members, nameof(members));

            using (var dbContext = dbContextFactory.Create())
            {
                var family = new Family
                {
                    Name = name,
                    Members = new List<FamilyMember>()
                };

                foreach (var memberName in members)
                {
                    family.Members.Add(new FamilyMember
                    {
                        Name = memberName
                    });
                }

                dbContext.Families.Add(family);
                await dbContext.SaveChangesAsync();
                return family.Id;
            }
        }

        public async Task<Family> GetFamilyAsync(int familyId)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                return await dbContext.Families.FirstOrDefaultAsync(f => f.Id == familyId);
            }
        }

        public async Task<IReadOnlyDictionary<int, string>> GetMemberNamesAsync(int familyId)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                return await dbContext.FamilyMembers
                    .Where(m => m.FamilyId == familyId)
                    .ToDictionaryAsync(m => m.Id, m => m.Name);
            }
        }

        public async Task<string> GetNameAsync(int familyId)
        {
            using (var dbContext = dbContextFactory.Create())
            {
                return await dbContext.Families
                    .Where(f => f.Id == familyId)
                    .Select(f => f.Name)
                    .FirstAsync();
            }
        }
    }
}
