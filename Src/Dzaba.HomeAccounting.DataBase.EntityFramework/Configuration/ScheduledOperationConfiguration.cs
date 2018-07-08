using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal class ScheduledOperationConfiguration : EntityConfigurationBase<ScheduledOperation>
    {
        protected override void Configure(EntityTypeBuilder<ScheduledOperation> builder)
        {
            builder.HasOne(p => p.Family)
                .WithMany(p => p.ScheduledOperations)
                .HasForeignKey(p => p.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Member)
                .WithMany(p => p.ScheduledOperation)
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
