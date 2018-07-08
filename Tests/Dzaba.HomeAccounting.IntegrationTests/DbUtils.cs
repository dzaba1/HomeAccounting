using System;
using System.IO;
using System.Threading;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    public static class DbUtils
    {
        public static readonly string Location = Path.Combine(Path.GetTempPath(), "homeAccounting.db");

        public static void Delete()
        {
            if (!File.Exists(Location))
            {
                return;
            }

            int counter = 0;
            while (true)
            {
                try
                {
                    File.Delete(Location);
                    return;
                }
                catch (Exception)
                {
                    counter++;

                    if (counter >= 20)
                    {
                        throw;
                    }

                    Thread.Sleep(10);
                }
            }
        }
    }
}
