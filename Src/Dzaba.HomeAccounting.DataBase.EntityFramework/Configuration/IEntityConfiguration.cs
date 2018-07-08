using Microsoft.EntityFrameworkCore;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework.Configuration
{
    public interface IEntityConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
