using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    internal abstract class EntityConfigurationBase<T> : IEntityConfiguration
        where T : class
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<T>();
            Configure(builder);
        }

        protected abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
