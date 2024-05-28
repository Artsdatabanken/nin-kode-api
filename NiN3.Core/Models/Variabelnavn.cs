using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{

    [Index(nameof(Kode), IsUnique = false)]
    public class Variabelnavn
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }

        public string Kode { get; set; }
        public string? Langkode { get; set; }
        public Variabelkategori2Enum? Variabelkategori2 { get; set; }
        public VariabeltypeEnum? Variabeltype { get; set; }
        public VariabelgruppeEnum? Variabelgruppe { get; set; }
        public String? Navn { get; set; } = "";
        public Versjon Versjon { get; set; }
        [ForeignKey("VariabelId")]
        public Variabel Variabel { get; set; }

        public ICollection<VariabelnavnMaaleskala> VariabelnavnMaaleskala { get; set; } = new List<VariabelnavnMaaleskala>();

        public ICollection<Konvertering> Konverteringer { get; set; } = new List<Konvertering>();
    }
}
