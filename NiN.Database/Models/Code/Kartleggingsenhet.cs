namespace NiN.Database.Models.Code
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NiN.Database.Converters;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Kartleggingsenhet : BaseIdEntity
    {
        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);

        public MalestokkEnum Malestokk { get; set; }

        public KartleggingsenhetKode Kode { get; set; }

        [StringLength(1000)]
        public string Definisjon { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }

        public virtual ICollection<Grunntype> Grunntype { get; set; } = new List<Grunntype>();
    }
}
