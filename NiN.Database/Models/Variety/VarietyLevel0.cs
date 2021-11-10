namespace NiN.Database.Models.Variety
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Codes;

    public class VarietyLevel0 : BaseEntity
    {
        [Required]
        public virtual VarietyLevel0Code Kode { get; set; }

        public virtual ICollection<VarietyLevel1> UnderordnetKoder { get; set; } = new List<VarietyLevel1>();
    }
}
