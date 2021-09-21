namespace NiN.Database.Models
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Enums;

    public class Kartleggingsenhet
    {
        [Key]
        public int Id { get; set; }

        public MalestokkEnum Malestokk { get; set; }

        [StringLength(10)]
        public string KodeId { get; set; }

        [StringLength(1000)]
        public string Definisjon { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }
    }
}