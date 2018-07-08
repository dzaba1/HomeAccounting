using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal class FamilyMemberConfiguration : EntityConfigurationBase<FamilyMember>
    {
        protected override void Configure(EntityTypeBuilder<FamilyMember> builder)
        {
            builder.HasOne(p => p.Family)
                 .WithMany(p => p.Members)
                 .HasForeignKey(p => p.FamilyId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
