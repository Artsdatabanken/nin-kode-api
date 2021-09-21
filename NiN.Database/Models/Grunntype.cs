namespace NiN.Database.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Grunntype
    {
        public Grunntype() { }

        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Navn { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }

        public virtual GrunntypeKode GrunntypeKode { get; set; }
    }
}