using Dzaba.Utils;
using System;
using System.Data.Common;
using System.Linq;

namespace Dzaba.HomeAccounting.DataBase.EntityFramework
{
    public static class Extensions
    {
        internal static Version GetVersion(this DatabaseContext dbContext)
        {
            Require.NotNull(dbContext, nameof(dbContext));

            try
            {
                var data = dbContext.DatabaseData.FirstOrDefault();
                if (data == null)
                {
                    return new Version(1, 0, 0, 0);
                }
                return data.Version;
            }
            catch (DbException)
            {
                return new Version(1, 0, 0, 0);
            }
        }
    }
}
