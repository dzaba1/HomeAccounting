using Microsoft.EntityFrameworkCore;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public interface IEntityFrameworkProvider
    {
        void Register(DbContextOptionsBuilder optionsBuilder);
    }
}
