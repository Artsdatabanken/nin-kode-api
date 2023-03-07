namespace NinKode.Database.Model.v30
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class VariasjonRegional
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("col_0")]
        [JsonPropertyName("col_0")]
        public string col_0 { get; set; }
        [JsonProperty("col_1")]
        [JsonPropertyName("col_1")]
        public string col_1 { get; set; }
        [JsonProperty("Kode")]
        [JsonPropertyName("Kode")]
        public string Kode { get; set; }
        [JsonProperty("K/T")]
        [JsonPropertyName("K/T")]
        public string K_T { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Klasse/trinnbetegnelse")]
        [JsonPropertyName("Klasse/trinnbetegnelse")]
        public string KlasseTrinnbetegnelse { get; set; }
        [JsonProperty("NiN 1")]
        [JsonPropertyName("NiN 1")]
        public string NiN1 { get; set; }
        [JsonProperty("Alternative betegnelser etc.")]
        [JsonPropertyName("Alternative betegnelser etc.")]
        public string AlternativeBetegnelserEtc { get; set; }
        [JsonProperty("Tags")]
        [JsonPropertyName("Tags")]
        public string Tags { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp { get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
    }
}
