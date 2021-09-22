namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Hovedtypegruppe : BaseEntity
    {
        public Hovedtypegruppe()
        {
            UnderordnetKoder = new List<Hovedtype>();
        }

        //[Key]
        //public int Id { get; set; }

        //[StringLength(255)]
        //public string Navn { get; set; }

        [Required]
        public virtual HovedtypegruppeKode Kode { get; set; }

        public virtual Natursystem Natursystem { get; set; }

        public virtual ICollection<Hovedtype> UnderordnetKoder { get; set; }
    }
}