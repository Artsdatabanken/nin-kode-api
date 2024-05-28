using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    [Index(nameof(Kode))] // added attribute to create index for Kode attribute
    public class Konvertering
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string Kode { get; set; } //Code of current version
        public string ForrigeKode { get; set; } //Code of previous version
        public KlasseEnum Klasse { get; set; } //Class of current version
        public Versjon ForrigeVersjon { get; set; } //Previous version
        public Versjon Versjon { get; set; } //Current version
        public string Url { get; set; } //Url to the previous version
        public int? FoelsomhetsPresisjon { get; set; } //Sensitivity precision
        public int? Spesifiseringsevne { get; set; } //Specificity
    }
}
