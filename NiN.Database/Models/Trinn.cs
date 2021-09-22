namespace NiN.Database.Models
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Codes;

    public class Trinn
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Navn { get; set; }
        
        public virtual LKMKode Kode { get; set; }
        
        public virtual Miljovariabel Miljovariabel { get; set; }
    }
}