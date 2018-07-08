using Dzaba.HomeAccounting.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("Operations")]
    public class Operation : IOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Family))]
        public int FamilyId { get; set; }

        public virtual Family Family { get; set; }

        [ForeignKey(nameof(Month))]
        public int MonthId { get; set; }

        public virtual Month Month { get; set; }

        public decimal Amount { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [ForeignKey(nameof(Member))]
        public int? MemberId { get; set; }

        public virtual FamilyMember Member { get; set; }

        public DateTime? DateTime { get; set; }

        public OperationReport ToOperationReport()
        {
            return new OperationReport
            {
                Amount = Amount,
                Id = Id,
                Name = Name,
                IsScheduled = false,
                DateTime = DateTime,
                IsOverriden = false
            };
        }
    }
}
