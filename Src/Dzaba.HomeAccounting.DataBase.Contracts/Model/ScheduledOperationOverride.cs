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
        public YearAndMonth YearAndMonth
        {
            get
            {
                return new YearAndMonth(Year, Month);
            }
            set
            {
                Year = value.Year;
                Month = value.Month;
            }
        }

        public int OperationId { get; set; }

        public virtual ScheduledOperation Operation { get; set; }

        public decimal Amount { get; set; }
    }
}
