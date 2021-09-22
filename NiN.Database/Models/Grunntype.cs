namespace NiN.Database.Models
{
    public class Grunntype : BaseEntity
    {
        public Grunntype() { }

        //[Key]
        //public int Id { get; set; }

        //[StringLength(255)]
        //public string Navn { get; set; }

        public virtual Hovedtype Hovedtype { get; set; }

        public virtual GrunntypeKode Kode { get; set; }
    }
}