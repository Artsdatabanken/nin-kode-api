using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    [Index(nameof(Kode), IsUnique = false)]
    public class Grunntype
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        [Required]
        public Versjon Versjon { get; set; }
        [StringLength(255)]
        public string Langkode { get; set; }
        public string Kode { get; set; }
        public string? Navn { get; set; }
        public string Delkode { get; set; }
        public ProsedyrekategoriEnum? Prosedyrekategori { get; set; }
        [ForeignKey("HovedtypeId")]
        public Hovedtype Hovedtype { get; set; }

        //public Kartleggingsenhet? kartleggingsenhet { get; set; }

        public ICollection<GrunntypeVariabeltrinn> GrunntypeVariabeltrinn { get; set; } = new List<GrunntypeVariabeltrinn>();

        public ICollection<Konvertering> Konverteringer { get; set; } = new List<Konvertering>();
    }
}