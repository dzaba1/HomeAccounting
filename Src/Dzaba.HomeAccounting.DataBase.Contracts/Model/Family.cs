using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("Families")]
    public class Family : IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public virtual ICollection<FamilyMember> Members { get; set; }

        public virtual ICollection<Month> Months { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }

        public virtual ICollection<ScheduledOperation> ScheduledOperations { get; set; }
    }
}
