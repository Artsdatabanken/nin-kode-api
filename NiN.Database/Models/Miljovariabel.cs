namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using NiN.Database.Converters;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Enums;

    public class Miljovariabel : BaseEntity
    {
        public LKMKode Kode { get; set; }

        public LkmKategoriEnum Lkm => Kode.LkmKategori;

        public string LkmKategori => NinEnumConverter.GetValue<LkmKategoriEnum>(Kode.LkmKategori);

        public string Type => "Miljøvariabel";
        
        public virtual ICollection<Trinn> Trinn { get; set; } = new List<Trinn>();

        public virtual Hovedtype Hovedtype { get; set; }
    }
}