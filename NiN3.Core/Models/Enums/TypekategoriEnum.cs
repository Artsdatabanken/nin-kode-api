using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NiN3.Core.Models.Enums
{
    public enum TypekategoriEnum
    {
        //[Description("")]
        //Default,
        [Description("Livsmedium")]
        LI,
        [Description("Landformvariasjon")]
        LV,
        [Description("Marine vannmasser")]
        MV,
        [Description("Primært økodiversitetsnivå")]
        PE,
        [Description("Sekundært økodiversitetsnivå")]
        SE
    }
}
