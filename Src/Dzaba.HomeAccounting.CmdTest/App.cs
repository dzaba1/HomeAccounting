using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.CmdTest
{
    public interface IApp : IDisposable
    {
        Task RunAsync();
    }

    internal sealed class App : IApp
    {
        public void Dispose()
        {
            
        }

        public async Task RunAsync()
        {
            
        }
    }
}
