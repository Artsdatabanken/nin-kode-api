namespace NinKode.Database.Model.v30
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class VariasjonTerrengform
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("col_0")]
        [JsonPropertyName("col_0")]
        public string col_0 { get; set; }
        [JsonProperty("Vaiabelgruppe")]
        [JsonPropertyName("Vaiabelgruppe")]
        public string Vaiabelgruppe { get; set; }
        [JsonProperty("Kode")]
        [JsonPropertyName("Kode")]
        public string Kode { get; set; }
        [JsonProperty("Vaiabelnavn")]
        [JsonPropertyName("Vaiabelnavn")]
        public string Vaiabelnavn { get; set; }
        [JsonProperty("Trinn")]
        [JsonPropertyName("Trinn")]
        public string Trinn { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Forklaring")]
        [JsonPropertyName("Forklaring")]
        public string Forklaring { get; set; }
        [JsonProperty("Måleintervall")]
        [JsonPropertyName("Måleintervall")]
        public string Maaleintervall { get; set; }
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
