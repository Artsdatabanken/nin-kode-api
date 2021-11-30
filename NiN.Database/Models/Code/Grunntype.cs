namespace NiN.Database.Models.Code
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Converters;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Grunntype : BaseEntity
    {
        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);
        
        public virtual Hovedtype Hovedtype { get; set; }

        public virtual GrunntypeKode Kode { get; set; }

        public virtual ICollection<Kartleggingsenhet> Kartleggingsenhet { get; }

        public virtual ICollection<Miljovariabel> Miljovariabel { get; }

        public virtual ICollection<Basistrinn> Basistrinn { get; }
    }
}