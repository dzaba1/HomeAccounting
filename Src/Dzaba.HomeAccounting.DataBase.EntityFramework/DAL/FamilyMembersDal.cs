using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.Utils;
using Microsoft.EntityFrameworkCore;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.DAL
{
    internal sealed class FamilyMembersDal : IFamilyMembersDal
    {
        private readonly Func<DatabaseContext> dbContextFactory;

        public FamilyMembersDal(Func<DatabaseContext> dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IReadOnlyDictionary<int, string>> GetMemberNamesAsync(int familyId)
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.FamilyMembers
                    .Where(m => m.FamilyId == familyId)
                    .Select(m => new { m.Id, m.Name })
                    .ToDictionaryAsync(m => m.Id, m => m.Name);
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

        public async Task<int> AddMemeberAsync(int familyId, string name)
        {
            Require.NotWhiteSpace(name, nameof(name));

            using (var dbContext = dbContextFactory())
            {
                var entity = new FamilyMember
                {
                    FamilyId = familyId,
                    Name = name
                };

                await dbContext.FamilyMembers
                    .AddAsync(entity);
                await dbContext.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task DeleteMemberAsync(int id)
        {
            using (var dbContext = dbContextFactory())
            {
                var query = dbContext.FamilyMembers.Where(m => m.Id == id);

                dbContext.FamilyMembers.RemoveRange(query);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
