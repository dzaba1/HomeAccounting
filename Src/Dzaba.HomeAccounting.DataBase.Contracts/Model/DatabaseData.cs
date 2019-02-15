using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    public class DatabaseData : IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(64)]
        [Required]
        [Column("Version")]
        public string VersionRaw { get; set; }

        [NotMapped]
        public Version Version
        {
            get => Version.Parse(VersionRaw);
            set => VersionRaw = value.ToString();
        }
    }
}
