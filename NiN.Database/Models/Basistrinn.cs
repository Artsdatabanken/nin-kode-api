namespace NiN.Database.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Basistrinn
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Navn { get; set; }

        [StringLength(255)]
        public string Kode { get; set; }

        public virtual LKMKode LKMKode { get; set; }
    }
}