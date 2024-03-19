using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.Enums
{
    public enum VariabelgruppeEnum
    {
        //[Description("")]
        //Default,
        [Description("Elveløpsegenskaper")]
        EE,
        [Description("Skogbruksrelaterte egenskaper")]
        SB,
        [Description("Liggende død ved (læger)")]
        DL,
        [Description("Artsforekomst/-mengde")]
        W,
        [Description("Artsgruppesammensetning")]
        AG,
        [Description("Relativ del-artsgruppesammensetning")]
        AR,
        [Description("Naturgitte breobjekter")]
        BO,
        [Description("Elvebanker")]
        EB,
        [Description("Andre naturgitte elveløpsobjekter")]
        EO,
        [Description("Naturgitte innsjøobjekter")]
        IO,
        [Description("Torvmarksmassiv: Myrsegment")]
        TA,
        [Description("Torvmarksmassiv: Myrstruktur")]
        TB,
        [Description("Torvmarksmassiv: Mikrostruktur")]
        TC,
        [Description("Skogegenskaper")]
        SE,
        [Description("Innsjøbassengegenskaper")]
        IE,
        [Description("Generelle egenskaper")]
        GE,
        [Description("Generelle terrengegenskaper")]
        GT,
        [Description("Havegenskaper")]
        HE,
        [Description("Bremassivegenskaper")]
        BE,
        [Description("Menneskeskapt objekt i elv")]
        OE,
        [Description("Menneskeskapt objekt i innsjø eller til havs")]
        OI,
        [Description("Menneskeskapt terrestrisk objekt")]
        OT,
        [Description("Suksesjonsrelaterte egenskaper")]
        SU,
        [Description("Jordbruksrelaterte egenskaper")]
        JB,
        [Description("Stående død ved (gadder)")]        
        DG,
        [Description("Relativ sammensetning av død ved")]
        DR,
        [Description("Generelle treegenskaper")]
        TE,
        [Description("Gammelt tre")]
        TG,
        [Description("Tre med spesielt livsmedium")]
        TL,
        [Description("Trær med gitt minstestørrelse")]
        TM,
        [Description("Trær med gitt størrelse")]
        TS,
        [Description("Fremmedartsegenskaper")]
        FA,
    }
}
