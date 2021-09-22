namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Converters;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Enums;

    public class Miljovariabel
    {
        public Miljovariabel()
        {
            Trinn = new List<Trinn>();
        }

        [Key]
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Navn { get; set; }
        
        [StringLength(255)]
        public LKMKode Kode { get; set; }

        public LkmKategoriEnum Lkm => Kode.LkmKategori;

        public string LkmKategori => NinEnumConverter.GetValue<LkmKategoriEnum>(Kode.LkmKategori);

        public string Type => "Miljøvariabel";
        
        public virtual ICollection<Trinn> Trinn { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }
    }
}