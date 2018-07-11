using Dzaba.HomeAccounting.DataBase.Contracts.DAL;
using Dzaba.HomeAccounting.DataBase.Contracts.Model;
using Dzaba.HomeAccounting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzaba.HomeAccounting.IntegrationTests
{
    public interface IInvokerDal
    {
        Task<int> AddSmithFamilyAsync();
        Task<int> AddScheduledOperationAsync(int familyId, string name, decimal amount,
            int? memberId = null, DateTime? starts = null, DateTime? ends = null);
    }

    internal sealed class InvokerDal : IInvokerDal
    {
        private readonly IFamilyDal familyDal;
        private readonly IScheduledOperationDal scheduledOperationDal;
        private readonly IOperationDal operationDal;

        public InvokerDal(IFamilyDal familyDal,
             IScheduledOperationDal scheduledOperationDal,
             IOperationDal operationDal)
        {
            Require.NotNull(familyDal, nameof(familyDal));
            Require.NotNull(scheduledOperationDal, nameof(scheduledOperationDal));
            Require.NotNull(operationDal, nameof(operationDal));

            this.familyDal = familyDal;
            this.scheduledOperationDal = scheduledOperationDal;
            this.operationDal = operationDal;
        }

        public async Task<int> AddSmithFamilyAsync()
        {
            return await familyDal.AddFamilyAsync("Smith", new[] { "Mark", "Alice" });
        }

        public async Task<int> AddScheduledOperationAsync(int familyId, string name, decimal amount,
            int? memberId = null, DateTime? starts = null, DateTime? ends = null)
        {
            return await scheduledOperationDal.AddScheduledOperationAsync(new ScheduledOperation
            {
                Amount = amount,
                FamilyId = familyId,
                MemberId = memberId,
                Name = name,
                Starts = starts,
                Ends = ends
            });
        }

        public async Task<int> AddOperation(int familyId, string name, decimal amount, int monthId,
            int? memberId, DateTime? dateTime)
        {
            return await operationDal.AddOperationAsync(new Operation
            {
                Amount = amount,
                DateTime = dateTime,
                FamilyId = familyId,
                MemberId = memberId,
                MonthId = monthId,
                Name = name
            });
        }
    }
}
