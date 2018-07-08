using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("ScheduledOperationOverrides")]
    public class ScheduledOperationOverride
    {
        [Key]
        public int MonthId { get; set; }

        public virtual Month Month { get; set; }

        [Key]
        public int OperationId { get; set; }

        public virtual ScheduledOperation Operation { get; set; }

        public decimal Amount { get; set; }
    }
}
