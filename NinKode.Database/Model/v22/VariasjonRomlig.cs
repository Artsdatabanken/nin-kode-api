namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public class VariasjonRomlig
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("Kode")]
        [JsonPropertyName("Kode")]
        public string Kode { get; set; }
        [JsonProperty("Vaiabelgruppe")]
        [JsonPropertyName("Vaiabelgruppe")]
        public string Vaiabelgruppe { get; set; }
        [JsonProperty("Kode2")]
        [JsonPropertyName("Kode2")]
        public string Kode2 { get; set; }
        [JsonProperty("Navn")]
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }
        [JsonProperty("trinn")]
        [JsonPropertyName("trinn")]
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
        [JsonProperty("Variabeltype")]
        [JsonPropertyName("Variabeltype")]
        public string VariabelType { get; set; }
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
