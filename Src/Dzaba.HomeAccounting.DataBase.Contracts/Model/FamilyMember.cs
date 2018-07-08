using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    [Table("FamilyMembers")]
    public class FamilyMember : IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(64)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public int FamilyId { get; set; }

        public virtual Family Family { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }

        public virtual ICollection<ScheduledOperation> ScheduledOperation { get; set; }
    }
}
