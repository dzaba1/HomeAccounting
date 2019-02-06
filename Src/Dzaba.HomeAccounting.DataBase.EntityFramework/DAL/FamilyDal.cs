using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.Utils;
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

        public Task<int> AddFamilyAsync(string name)
        {
            return AddFamilyAsync(name, Enumerable.Empty<string>());
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

        public async Task<Family[]> GetAllAsync()
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Families.ToArrayAsync();
            }
        }

        public async Task<IReadOnlyDictionary<int, string>> GetAllNamesAsync()
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Families
                    .Select(x => new {x.Id, x.Name})
                    .ToDictionaryAsync(x => x.Id, x => x.Name);
            }
        }

        public async Task DeleteFamilyAsync(int id)
        {
            using (var dbContext = dbContextFactory())
            {
                var query = dbContext.Families.Where(f => f.Id == id);
                dbContext.Families.RemoveRange(query);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Family> GetFamilyAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Families.FirstOrDefaultAsync(f => f.Id == familyId);
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
