namespace NiN.Database.Models
{
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Common;

    public class Basistrinn : BaseEntity
    {
        public virtual Trinn Trinn { get; set; }

        public virtual BasistrinnKode Kode { get; set; }
    }
}