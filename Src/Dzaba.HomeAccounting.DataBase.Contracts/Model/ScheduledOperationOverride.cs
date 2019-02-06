using System.ComponentModel.DataAnnotations.Schema;
using Dzaba.HomeAccounting.Contracts;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("ScheduledOperationOverrides")]
    public class ScheduledOperationOverride
    {
        public ushort Year { get; set; }

        public byte Month { get; set; }

        [NotMapped]
        public YearAndMonth YearAndMonth => new YearAndMonth(Year, Month);

        public virtual MonthData MonthData { get; set; }

        public int OperationId { get; set; }

        public virtual ScheduledOperation Operation { get; set; }

        public decimal Amount { get; set; }
    }
}
