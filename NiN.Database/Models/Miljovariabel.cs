namespace NiN.Database.Models
{
    using System;
    using System.Collections.Generic;
    using NiN.Database.Converters;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Enums;

    public class Miljovariabel : BaseEntity
    {
        public Miljovariabel()
        {
            Trinn = new List<Trinn>();
        }
        
        public LKMKode Kode { get; set; }

        public LkmKategoriEnum Lkm => Kode.LkmKategori;

        public string LkmKategori => NinEnumConverter.GetValue<LkmKategoriEnum>(Kode.LkmKategori);

        public string Type => "Miljøvariabel";
        
        public virtual ICollection<Trinn> Trinn { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }
    }
}