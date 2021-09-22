namespace NiN.Database.Models
{
    using System.Collections.Generic;

    public class Hovedtype : BaseEntity
    {
        public Hovedtype()
        {
            UnderordnetKoder = new List<Grunntype>();
            Kartleggingsenheter = new List<Kartleggingsenhet>();
            Miljovariabler = new List<Miljovariabel>();
        }

        //[Key]
        //public int Id { get; set; }

        //[StringLength(255)]
        //public string Navn { get; set; }

        public virtual HovedtypeKode Kode { get; set; }

        public virtual Hovedtypegruppe Hovedtypegruppe { get; set; }

        public virtual ICollection<Grunntype> UnderordnetKoder { get; set; }

        public virtual ICollection<Kartleggingsenhet> Kartleggingsenheter { get; set; }

        public virtual ICollection<Miljovariabel> Miljovariabler { get; set; }
    }
}