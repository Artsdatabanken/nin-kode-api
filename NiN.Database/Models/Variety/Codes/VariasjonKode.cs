namespace NiN.Database.Models.Variety.Codes
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety.Enums;

    public class VariasjonKode : BaseIdEntity
    {
        [StringLength(255)]
        public string KodeName { get; set; }

        [StringLength(1000)]
        public string Definisjon { get; set; }

        public VarietyEnum VarietyCategory { get; set; }
    }

    public class VarietyLevel0Code : VariasjonKode
    {
        public int VarietyLevelId { get; set; }

        [ForeignKey(nameof(VarietyLevelId))]
        public virtual VarietyLevel0 VarietyLevel { get; set; }

        public VarietyLevel0Code()
        {
            VarietyCategory = VarietyEnum.VarietyLevel0;
        }
    }

    public class VarietyLevel1Code : VariasjonKode
    {
        public int VarietyLevelId { get; set; }

        [ForeignKey(nameof(VarietyLevelId))]
        public virtual VarietyLevel1 VarietyLevel { get; set; }

        public VarietyLevel1Code()
        {
            VarietyCategory = VarietyEnum.VarietyLevel1;
        }
    }

    public class VarietyLevel2Code : VariasjonKode
    {
        public int VarietyLevelId { get; set; }

        [ForeignKey(nameof(VarietyLevelId))]
        public virtual VarietyLevel2 VarietyLevel { get; set; }

        public VarietyLevel2Code()
        {
            VarietyCategory = VarietyEnum.VarietyLevel2;
        }
    }

    public class VarietyLevel3Code : VariasjonKode
    {
        public int VarietyLevelId { get; set; }

        [ForeignKey(nameof(VarietyLevelId))]
        public virtual VarietyLevel3 VarietyLevel { get; set; }

        public VarietyLevel3Code()
        {
            VarietyCategory = VarietyEnum.VarietyLevel3;
        }
    }

    public class VarietyLevel4Code : VariasjonKode
    {
        public int VarietyLevelId { get; set; }

        [ForeignKey(nameof(VarietyLevelId))]
        public virtual VarietyLevel4 VarietyLevel { get; set; }

        public VarietyLevel4Code()
        {
            VarietyCategory = VarietyEnum.VarietyLevel4;
        }
    }

    public class VarietyLevel5Code : VariasjonKode
    {
        public int VarietyLevelId { get; set; }

        [ForeignKey(nameof(VarietyLevelId))]
        public virtual VarietyLevel5 VarietyLevel { get; set; }

        public VarietyLevel5Code()
        {
            VarietyCategory = VarietyEnum.VarietyLevel5;
        }
    }
}
