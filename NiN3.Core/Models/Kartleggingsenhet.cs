using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    [Index(nameof(Kode), IsUnique = false)]
    public class Kartleggingsenhet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        //public Guid Id { get; set; }
        public string? Navn { get; set; }
        public string Kode { get; set; }// added nullable suffix
        public string Langkode { get; set; } 
        public MaalestokkEnum Maalestokk { get; set; }
        public ICollection<Grunntype> Grunntyper { get; set; } = new List<Grunntype>();
        public Versjon Versjon { get; set; }
    }
}
