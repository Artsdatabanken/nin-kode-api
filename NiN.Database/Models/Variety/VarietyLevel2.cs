namespace NiN.Database.Models.Variety
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Codes;

    public class VarietyLevel2 : BaseEntity
    {
        [Required]
        public virtual VarietyLevel2Code Kode { get; set; }

        public virtual VarietyLevel1 OverordnetKode { get; set; }

        public virtual ICollection<VarietyLevel3> UnderordnetKoder { get; set; } = new List<VarietyLevel3>();
    }
}
