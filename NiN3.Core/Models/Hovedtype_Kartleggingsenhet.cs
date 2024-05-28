using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class Hovedtype_Kartleggingsenhet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        //public Guid Id { get; set; }
        public Versjon Versjon { get; set; }

        public Kartleggingsenhet Kartleggingsenhet { get; set; }
        public Hovedtype Hovedtype { get; set; }
    }
}
