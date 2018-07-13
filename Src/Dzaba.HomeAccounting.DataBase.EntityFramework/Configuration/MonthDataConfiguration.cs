using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal sealed class MonthDataConfiguration : EntityConfigurationBase<MonthData>
    {
        protected override void Configure(EntityTypeBuilder<MonthData> builder)
        {
            builder.HasKey(p => new { p.FamilyId, p.Year, p.Month });
            builder.HasIndex(p => new { p.Year, p.Month });
            builder.HasOne(p => p.Family)
                 .WithMany(p => p.Months)
                 .HasForeignKey(p => p.FamilyId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
