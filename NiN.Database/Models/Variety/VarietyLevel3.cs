namespace NiN.Database.Models.Variety
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Codes;

    public class VarietyLevel3 : BaseEntity
    {
        [Required]
        public virtual VarietyLevel3Code Kode { get; set; }

        public virtual VarietyLevel2 OverordnetKode { get; set; }

        public virtual ICollection<VarietyLevel4> UnderordnetKoder { get; set; } = new List<VarietyLevel4>();
    }
}
