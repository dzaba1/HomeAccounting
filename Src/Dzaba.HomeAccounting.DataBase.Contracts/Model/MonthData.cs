using Dzaba.HomeAccounting.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("Months")]
    public class MonthData
    {
        public ushort Year { get; set; }

        public byte Month { get; set; }

        [NotMapped]
        public YearAndMonth YearAndMonth
        {
            get => new YearAndMonth(Year, Month);
            set
            {
                Year = value.Year;
                Month = value.Month;
            }
        }

        public int FamilyId { get; set; }

        public virtual Family Family { get; set; }

        public decimal? IncomeOverride { get; set; }

        public decimal? TotalOverride { get; set; }

        public string Notes { get; set; }
    }
}
