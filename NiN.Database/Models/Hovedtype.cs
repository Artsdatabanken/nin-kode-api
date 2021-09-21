namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Hovedtype
    {
        public Hovedtype()
        {
            UnderordnetKoder = new List<Grunntype>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Navn { get; set; }

        public virtual HovedtypeKode HovedtypeKode { get; set; }

        public virtual Hovedtypegruppe Hovedtypegruppe { get; set; }

        public virtual ICollection<Grunntype> UnderordnetKoder { get; set; }

        public virtual ICollection<Kartleggingsenhet> Kartleggingsenheter { get; set; }

        public virtual ICollection<Miljovariabel> Miljovariabler { get; set; }
    }
}