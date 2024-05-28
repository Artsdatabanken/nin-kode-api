using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class Kartleggingsenhet_Grunntype
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public Versjon Versjon { get; set; }
        public Kartleggingsenhet Kartleggingsenhet { get; set; }
        public Grunntype Grunntype { get; set; }
    }
}
