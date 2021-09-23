namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using NiN.Database.Models.Codes;

    public class Trinn : BaseEntity
    {
        public Trinn()
        {
            Basistrinn = new List<Basistrinn>();
        }

        public virtual TrinnKode Kode { get; set; }

        public virtual Miljovariabel Miljovariabel { get; set; }

        public virtual ICollection<Basistrinn> Basistrinn { get; set; }
    }
}