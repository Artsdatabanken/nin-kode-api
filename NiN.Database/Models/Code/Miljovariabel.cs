namespace NiN.Database.Models.Code
{
    using System.Collections.Generic;
    using NiN.Database.Converters;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Miljovariabel : BaseEntity
    {
        public LKMKode Kode { get; set; }

        public LkmKategoriEnum Lkm => Kode.LkmKategori;

        public string LkmKategori => NinEnumConverter.GetValue<LkmKategoriEnum>(Kode.LkmKategori);

        public string Type => "Miljøvariabel";
        
        public virtual ICollection<Trinn> Trinn { get; set; } = new List<Trinn>();

        public virtual Hovedtype Hovedtype { get; set; }

        public virtual ICollection<Grunntype> Grunntype { get; set; } = new List<Grunntype>();
    }
}
