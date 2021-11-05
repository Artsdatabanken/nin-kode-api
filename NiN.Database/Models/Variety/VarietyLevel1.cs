namespace NiN.Database.Models.Variety
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Codes;

    public class VarietyLevel1 : BaseEntity
    {
        [Required]
        public virtual VarietyLevel1Code Kode { get; set; }

        public virtual VarietyLevel0 OverordnetKode { get; set; }

        public virtual ICollection<VarietyLevel2> UnderordnetKoder { get; set; } = new List<VarietyLevel2>();
    }
}
