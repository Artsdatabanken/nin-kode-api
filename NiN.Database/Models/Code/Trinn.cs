namespace NiN.Database.Models.Code
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NiN.Database.Converters;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Trinn : BaseEntity
    {
        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);

        public virtual TrinnKode Kode { get; set; }

        public virtual Miljovariabel Miljovariabel { get; set; }

        public virtual ICollection<Basistrinn> Basistrinn { get; set; } = new List<Basistrinn>();
    }
}