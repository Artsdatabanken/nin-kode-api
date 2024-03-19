using System.ComponentModel;

namespace NiN3.Core.Models.Enums
{
    public enum EnhetEnum
    {
        //[Description("")]
        //Default,
        [Description("Binær")]
        B,
        [Description("Grader")]
        G,
        [Description("Observert antall")]
        OA,
        [Description("Prosent")]
        P,
        [Description("Tetthet")]
        T,
        [Description("Ukjent, ikke angitt")]
        U,
        [Description("Variabelspesifikk trinndeling, ikke-ordnet faktorvariabel")]
        VSI,
        [Description("Variabelspesifikk trinndeling, ordnet faktorvariabel")]
        VSO
    }
}