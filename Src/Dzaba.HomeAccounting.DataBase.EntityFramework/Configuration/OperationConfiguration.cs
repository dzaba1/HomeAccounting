using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal class OperationConfiguration : EntityConfigurationBase<Operation>
    {
        protected override void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.HasOne(p => p.Family)
                .WithMany(p => p.Operations)
                .HasForeignKey(p => p.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Month)
                .WithMany(p => p.Operations)
                .HasForeignKey(p => p.MonthId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Member)
                .WithMany(p => p.Operations)
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
