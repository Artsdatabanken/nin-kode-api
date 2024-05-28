using NiN3.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class AlleKortkoder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string Kortkode { get; set; }
        public string? KortkodeV2 { get; set; }
        public KlasseEnum TypeKlasseEnum { get; set; }
        public Versjon Versjon { get; set; }
    }
}
