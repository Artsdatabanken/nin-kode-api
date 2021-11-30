namespace NiN.Database.Models.Code
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NiN.Database.Converters;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Basistrinn : BaseEntity
    {
        [StringLength(255)]
        //public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(KategoriEnum.Basistrinn);

        //public virtual BasistrinnKode Kode { get; set; }

        public virtual ICollection<Trinn> Trinn { get; set; } = new List<Trinn>();

        public virtual ICollection<Grunntype> Grunntype { get; set; } = new List<Grunntype>();
    }
}