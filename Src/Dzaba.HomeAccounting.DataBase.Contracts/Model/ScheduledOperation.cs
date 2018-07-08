using Dzaba.HomeAccounting.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("ScheduledOperations")]
    public class ScheduledOperation : IOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Family))]
        public int FamilyId { get; set; }

        public virtual Family Family { get; set; }

        public decimal Amount { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [ForeignKey(nameof(Member))]
        public int? MemberId { get; set; }

        public virtual FamilyMember Member { get; set; }

        public DateTime? Starts { get; set; }

        public DateTime? Ends { get; set; }

        public virtual ICollection<ScheduledOperationOverride> Overrides { get; set; }

        public OperationReport ToOperationReport()
        {
            return new OperationReport
            {
                Amount = Amount,
                Id = Id,
                Name = Name,
                IsScheduled = true
            };
        }
    }
}
