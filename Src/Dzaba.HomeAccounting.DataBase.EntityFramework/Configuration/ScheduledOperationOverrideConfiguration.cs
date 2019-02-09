using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal class ScheduledOperationOverrideConfiguration : EntityConfigurationBase<ScheduledOperationOverride>
    {
        protected override void Configure(EntityTypeBuilder<ScheduledOperationOverride> builder)
        {
            builder.HasKey(p => new { p.Year, p.Month, p.OperationId });
            builder.HasOne(p => p.Operation)
                .WithMany(p => p.Overrides)
                .HasForeignKey(p => p.OperationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
