namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        public string Kode { get; set; }
        
        public LKMKategoriEnum LkmKategori { get; set; }
        
        [StringLength(255)]
        public string Type { get; set; }
        
        public virtual ICollection<Trinn> Trinn { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }
    }
}