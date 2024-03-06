using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class Versjon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        //public Guid Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Navn { get; set; } = string.Empty;
        public ICollection<Type> Typer { get; set; }
        public ICollection<Variabel> Variabler { get; set; }
    }
}