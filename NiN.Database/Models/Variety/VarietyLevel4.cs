namespace NiN.Database.Models.Variety
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Codes;

    public class VarietyLevel4 : BaseEntity
    {
        [Required]
        public virtual VarietyLevel4Code Kode { get; set; }

        public virtual VarietyLevel3 OverordnetKode { get; set; }

        public virtual ICollection<VarietyLevel5> UnderordnetKoder { get; set; } = new List<VarietyLevel5>();
    }
}
