using System;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Migration
{
    public interface IMigration<in T>
    {
        Version From { get; }

        Task MigrateAsync(T context);
    }
}
