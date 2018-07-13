using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.CmdTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = Bootstrapper.CreateContainer();
                using (var app = container.GetRequiredService<IApp>())
                {
                    app.RunAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
