using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("Months")]
    public class Month : IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }

        [ForeignKey(nameof(Family))]
        public int FamilyId { get; set; }

        public virtual Family Family { get; set; }

        public DateTime Date { get; set; }

        public decimal? IncomeOverride { get; set; }

        public decimal? TotalOverride { get; set; }

        public virtual ICollection<ScheduledOperationOverride> ScheduledOperationOverrides { get; set; }

        public string Notes { get; set; }
    }
}
