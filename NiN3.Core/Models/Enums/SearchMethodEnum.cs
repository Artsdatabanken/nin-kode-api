using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NiN3.Core.Models.Enums
{
    /// <summary>
    /// Definerer søkemetoden som skal brukes ved søk i NiN-systemet.
    /// VIKTIG: Kun SW (StartsWith) oppfører seg annerledes - alle andre alternativer utfører substrengsøk.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SearchMethodEnum
    {
        /// <summary>
        /// Søk etter termer som starter med søkestrengen (prefikssøk).
        /// Dette er det ENESTE alternativet som gjør noe annerledes enn substrengsøk.
        /// </summary>
        [Description("StartsWith - Søk etter termer som begynner med søkestrengen")]
        
        /// <summary>
        /// Søk etter termer som inneholder søkestrengen hvor som helst (substrengsøk).
        /// Dette er STANDARDMETODEN og brukes for alle andre verdier.
        /// </summary>
        [Description("Contains - Søk etter termer som inneholder søkestrengen (standard)")]
        C = 1,
        
        /// <summary>
        /// Standardsøkemetode - bruker Contains (substrengsøk).
        /// MERK: Dette er det samme som C-alternativet.
        /// </summary>
        [Description("Default - Bruker Contains-metoden (substrengsøk)")]
        Default = 1
    }
}
