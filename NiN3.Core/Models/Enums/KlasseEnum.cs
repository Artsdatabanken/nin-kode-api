using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NiN3.Core.Models.Enums
{
    /// <summary>
    /// Definerer klassifikasjonstypene som er tilgjengelige for søk i NiN-systemet.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KlasseEnum
    {
        /// <summary>
        /// Søk i Type-klassifiseringen.
        /// </summary>
        [Description("Type - Søk i type-klassifisering")]
        T = 0,
        
        /// <summary>
        /// Søk i Hovedtypegruppe (overordnet typegruppe).
        /// </summary>
        [Description("Hovedtypegruppe - Søk i hovedtypegrupper")]
        HTG = 1,
        
        /// <summary>
        /// Søk i Hovedtype (hovedkategorier av naturtyper).
        /// </summary>
        [Description("Hovedtype - Søk i hovedtyper")]
        HT = 2,
        
        /// <summary>
        /// Søk i Grunntype (grunnleggende naturtyper).
        /// </summary>
        [Description("Grunntype - Søk i grunntyper")]
        GT = 3,
        
        /// <summary>
        /// Søk i Kartleggingsenhet (enheter for kartlegging).
        /// </summary>
        [Description("Kartleggingsenhet - Søk i kartleggingsenheter")]
        KE = 4,
        
        /// <summary>
        /// Søk i Variabel (miljøvariabler).
        /// </summary>
        [Description("Variabel - Søk i variabler")]
        V = 5,
        
        /// <summary>
        /// Søk i Variabelnavn (navn på miljøvariabler).
        /// </summary>
        [Description("Variabelnavn - Søk i variabelnavn")]
        VN = 6,
        
        /// <summary>
        /// Søk i Variabeltrinn (trinn/verdier for variabler).
        /// </summary>
        [Description("Variabeltrinn - Søk i variabeltrinn")]
        VT = 7,
        
        /// <summary>
        /// Søk i alle klassifikasjoner - Dette er STANDARDVALGET.
        /// </summary>
        [Description("Alle - Søk i alle klassifikasjoner (standard)")]
        ALL = 8,
        
        /// <summary>
        /// Standardverdi - bruker ALL (søk i alle klassifikasjoner).
        /// </summary>
        [Description("Default - Bruker ALL (søk i alle klassifikasjoner)")]
        Default = 8
    }
}
