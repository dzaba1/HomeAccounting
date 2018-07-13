using Dzaba.HomeAccounting.Contracts;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    public class OperationOverride
    {
        public YearAndMonth Month { get; set; }
        public int OperationId { get; set; }
        public decimal Amount { get; set; }
        public int? MemberId { get; set; }
        public decimal OriginalAmount { get; set; }
    }
}
