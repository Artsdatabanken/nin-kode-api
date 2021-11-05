namespace NiN.Database.Models.Code
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Kartleggingsenhet : BaseIdEntity
    {
        public MalestokkEnum Malestokk { get; set; }

        public KartleggingsenhetKode Kode { get; set; }

        [StringLength(1000)]
        public string Definisjon { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }
    }
}