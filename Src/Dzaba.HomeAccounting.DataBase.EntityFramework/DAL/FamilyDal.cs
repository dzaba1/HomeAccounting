using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class FamilyDal : IFamilyDal
    {
        private readonly Func<DatabaseContext> dbContextFactory;

        public FamilyDal(Func<DatabaseContext> dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddFamilyAsync(string name, IEnumerable<string> members)
        {
            Require.NotWhiteSpace(name, nameof(name));
            Require.NotNull(members, nameof(members));

            using (var dbContext = dbContextFactory())
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

        public async Task<int?> FindFamilyId(string name)
        {
            Require.NotWhiteSpace(name, nameof(name));

            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Families
                    .Where(f => f.Name == name)
                    .Select(f => f.Id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<int?> FindMemberId(int familyId, string name)
        {
            Require.NotWhiteSpace(name, nameof(name));

            using (var dbContext = dbContextFactory())
            {
                return await dbContext.FamilyMembers
                    .Where(m => m.FamilyId == familyId && m.Name == name)
                    .Select(m => m.Id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<Family> GetFamilyAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Families.FirstOrDefaultAsync(f => f.Id == familyId);
            }
        }

        public async Task<IReadOnlyDictionary<int, string>> GetMemberNamesAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.FamilyMembers
                    .Where(m => m.FamilyId == familyId)
                    .ToDictionaryAsync(m => m.Id, m => m.Name);
            }
        }

        public async Task<string> GetNameAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Families
                    .Where(f => f.Id == familyId)
                    .Select(f => f.Name)
                    .FirstAsync();
            }
        }
    }
}
