using System.ComponentModel;

namespace NiN3.Core.Models.Enums
{ 
   public enum VariabeltypeEnum
    {
        //[Description("")]
        //Default,
        [Description("Enkel, ikke-ordnet faktorvariabel")]
        FE,
        [Description("Kompleks, ikke-ordnet faktorvariabel")]
        FK,
        [Description("Enkel gradient")]
        GE,
        [Description("Kompleks gradient")]
        GK
    }
}
