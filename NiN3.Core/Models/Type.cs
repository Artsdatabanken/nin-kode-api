using NiN3.Core.Models.Enums;
using NiN3.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace NiN3.Core.Models
{
    [Index(nameof(Kode), IsUnique = false)]
    
    public class Type 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        //public Guid Id { get; set; }
        [ForeignKey("VersjonId")]
        public Versjon Versjon { get; set; }
        public string Navn { get; set; }
        public EcosystnivaaEnum? Ecosystnivaa { get; set; }
        public TypekategoriEnum? Typekategori { get; set; }
        public Typekategori2Enum? Typekategori2 { get; set;}
        public string Kode { get; set; }
        public string Langkode { get; set; }
        public ICollection<Hovedtypegruppe> Hovedtypegrupper { get; set; }
    }
}