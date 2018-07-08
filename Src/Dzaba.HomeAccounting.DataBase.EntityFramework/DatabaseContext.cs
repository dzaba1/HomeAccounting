using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

        public DbSet<Family> Families { get; set; }

        public DbSet<FamilyMember> FamilyMembers { get; set; }

        public DbSet<Month> Months { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<ScheduledOperation> ScheduledOperations { get; set; }

        public DbSet<ScheduledOperationOverride> ScheduledOperationOverrides { get; set; }
    }
}
