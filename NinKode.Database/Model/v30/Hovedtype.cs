namespace NinKode.Database.Model.v30
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class Hovedtype
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("HTK")]
        [JsonPropertyName("HTK")]
        public string HovedtypeKode { get; set; }
        [JsonProperty("Navn")]
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }
        [JsonProperty("PrK")]
        [JsonPropertyName("PrK")]
        public string PrK { get; set; }
        [JsonProperty("PrK-tekst")]
        [JsonPropertyName("PrK-tekst")]
        public string PrKTekst { get; set; }
        [JsonProperty("GrL")]
        [JsonPropertyName("GrL")]
        public string GrL { get; set; }
        [JsonProperty("Definisjonsgrunnlag-tekst")]
        [JsonPropertyName("Definisjonsgrunnlag-tekst")]
        public string DefinisjonsgrunnlagTekst { get; set; }
        [JsonProperty("NiN[1]")]
        [JsonPropertyName("NiN[1]")]
        public string NiN { get; set; }
        [JsonProperty("dLKM")]
        [JsonPropertyName("dLKM")]
        public string dLKM { get; set; }
        [JsonProperty("hLKM")]
        [JsonPropertyName("hLKM")]
        public string hLKM { get; set; }
        [JsonProperty("tLKM")]
        [JsonPropertyName("tLKM")]
        public string tLKM { get; set; }
        [JsonProperty("uLKM")]
        [JsonPropertyName("uLKM")]
        public string uLKM { get; set; }
        [JsonProperty("Kunnskapsgrunnlag - Hovedtypen generelt")]
        [JsonPropertyName("Kunnskapsgrunnlag - Hovedtypen generelt")]
        public string KunnskapsgrunnlagGenerelt { get; set; }
        [JsonProperty("Kunnskapsgrunnlag - Grunntypeinndelingen")]
        [JsonPropertyName("Kunnskapsgrunnlag - Grunntypeinndelingen")]
        public string KunnskapsgrunnlagGrunntypeinndelingen { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp{ get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
    }
}
