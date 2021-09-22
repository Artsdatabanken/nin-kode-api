namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Natursystem : BaseEntity
    {
        public Natursystem()
        {
            UnderordnetKoder = new List<Hovedtypegruppe>();
        }

        //[Key]
        //public int Id { get; set; }

        //[StringLength(255)]
        //public string Navn { get; set; }

        [StringLength(255)]
        public string Kategori { get; set; }

        [Required]
        public virtual NatursystemKode Kode { get; set; }

        public virtual ICollection<Hovedtypegruppe> UnderordnetKoder { get; set; }
    }
}