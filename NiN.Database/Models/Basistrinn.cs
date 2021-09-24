namespace NiN.Database.Models
{
    using NiN.Database.Models.Codes;

    public class Basistrinn : BaseEntity
    {
        public virtual Trinn Trinn { get; set; }

        public virtual BasistrinnKode Kode { get; set; }
    }
}