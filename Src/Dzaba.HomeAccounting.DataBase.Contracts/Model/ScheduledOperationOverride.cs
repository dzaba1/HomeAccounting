using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("ScheduledOperationOverrides")]
    public class ScheduledOperationOverride
    {
        public ushort Year { get; set; }

        public byte Month { get; set; }

        public virtual MonthData MonthData { get; set; }

        public int OperationId { get; set; }

        public virtual ScheduledOperation Operation { get; set; }

        public decimal Amount { get; set; }
    }
}
