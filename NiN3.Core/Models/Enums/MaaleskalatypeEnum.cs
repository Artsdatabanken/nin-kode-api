using System.ComponentModel;

namespace NiN3.Core.Models.Enums
{
    public enum MaaleskalatypeEnum
    {
        //[Description("")]
        //Default,
        [Description("Binær")]
        B,
        [Description("Telleskala, observert antall")]
        D,
        [Description("Telleskala, observert antall")]
        D0,
        [Description("Telleskala, observert antall")]
        D1a,
        [Description("Telleskala, observert antall")]
        D1b,
        [Description("Kontinuerlig, observert verdi")]
        K,
        [Description("Generisk måleskala for andelsvariabel")]
        P,
        [Description("Generisk måleskala for andelsvariabel")]
        P6a,
        [Description("Generisk måleskala for andelsvariabel")]
        P6c,
        [Description("Prosentskala")]
        P9a,
        [Description("Generisk måleskala for andelsvariabel, der n angir trinn om m angir variant")]
        Pnm,
        [Description("Variabelspesifikk, ikke-ordnet faktorverdi")]
        SI,
        [Description("Variabelspesifikk, ordnet faktorverdi")]
        SO,
        [Description("Tetthetsskala, 2-logaritmisk")]
        T,
        [Description("Tetthetsskala, 2-logaritmisk")]
        T0,
        [Description("Tetthetsskala, 2-logaritmisk")]
        T1a,
        [Description("Tetthetsskala, 2-logaritmisk")]
        T1b,
        [Description("Tetthetsskala, 2-logaritmisk")]
        T1c
    }
}
