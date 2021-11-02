namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Common;

    public class Trinn : BaseEntity
    {
        public virtual TrinnKode Kode { get; set; }

        public virtual Miljovariabel Miljovariabel { get; set; }

        public virtual ICollection<Basistrinn> Basistrinn { get; set; } = new List<Basistrinn>();
    }
}