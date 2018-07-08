using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal class MonthConfiguration : EntityConfigurationBase<Month>
    {
        protected override void Configure(EntityTypeBuilder<Month> builder)
        {
            builder.HasOne(p => p.Family)
                 .WithMany(p => p.Months)
                 .HasForeignKey(p => p.FamilyId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
