using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class Trinn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string? Langkode { get; set; }
        public string Beskrivelse { get; set; }
        public string Verdi { get; set; }
        public Maaleskala Maaleskala { get; set; }
        public ICollection<Konvertering> Konverteringer { get; set; } = new List<Konvertering>();
    }
}
