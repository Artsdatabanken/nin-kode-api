namespace NiN.Database.Models.Variety
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Codes;

    public class VarietyLevel5 : BaseEntity
    {
        [Required]
        public virtual VarietyLevel5Code Kode { get; set; }

        public virtual VarietyLevel4 OverordnetKode { get; set; }
    }
}
