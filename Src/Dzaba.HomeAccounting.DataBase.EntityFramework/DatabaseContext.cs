﻿using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration;
using Dzaba.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        private readonly IEntityConfiguration[] entityConfigurations;

        public DatabaseContext(DbContextOptions options,
            IEnumerable<IEntityConfiguration> entityConfigurations)
            : base(options)
        {
            Require.NotNull(entityConfigurations, nameof(entityConfigurations));

            this.entityConfigurations = entityConfigurations.ToArray();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var configuration in entityConfigurations)
            {
                configuration.Configure(modelBuilder);
            }
        }

        public DbSet<Family> Families { get; set; }

        public DbSet<FamilyMember> FamilyMembers { get; set; }

        public DbSet<MonthData> Months { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<ScheduledOperation> ScheduledOperations { get; set; }

        public DbSet<ScheduledOperationOverride> ScheduledOperationOverrides { get; set; }

        public DbSet<DatabaseData> DatabaseData { get; set; }
    }
}
