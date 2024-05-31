using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class Endringslogg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        [ForeignKey("VersjonId")]
        public Versjon Versjon { get; set; }
        public String Tidspunkt { get; set; }
        public String Beskrivelse { get; set; }
    }
}
