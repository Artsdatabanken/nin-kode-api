namespace NiN.Database.Models.Code
{
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Common;

    public class Basistrinn : BaseEntity
    {
        public virtual Trinn Trinn { get; set; }

        public virtual BasistrinnKode Kode { get; set; }
    }
}