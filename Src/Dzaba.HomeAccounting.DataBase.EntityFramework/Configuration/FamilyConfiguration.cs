using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal class FamilyConfiguration : EntityConfigurationBase<Family>
    {
        protected override void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
