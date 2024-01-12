using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NiN3.Core.Models.Enums
{
    public enum Typekategori2Enum
    {
        //[Description("")]
        //Default,
        [Description("Bremassiv")]
        BM,
        [Description("Elveløp")]
        EL,
        [Description("Landformer i fast fjell og løsmasser")]
        FL,
        [Description("Innsjøbasseng")]
        IB,
        [Description("Landskapstype")]
        LA,
        [Description("Natursystem")]
        NA,
        [Description("Naturkompleks")]
        NK,
        [Description("Torvmarksmassiv")]
        TM
    }
}
