namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public class VariasjonGeo
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("col_0")]
        [JsonPropertyName("col_0")]
        public string col_0 { get; set; }
        [JsonProperty("Variabelgruppe")]
        [JsonPropertyName("Variabelgruppe")]
        public string Variabelgruppe { get; set; }
        [JsonProperty("Nivå 1 Kode")]
        [JsonPropertyName("Nivå 1 Kode")]
        public string Nivaa1kode { get; set; }
        [JsonProperty("col_3")]
        [JsonPropertyName("col_3")]
        public string col_3 { get; set; }
        [JsonProperty("Nivå 2 Kode")]
        [JsonPropertyName("Nivå 2 Kode")]
        public string Nivaa2Kode { get; set; }
        [JsonProperty("col_5")]
        [JsonPropertyName("col_5")]
        public string col_5 { get; set; }
        [JsonProperty("Nivå 3 Kode")]
        [JsonPropertyName("Nivå 3 Kode")]
        public string Nivaa3Kode { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Navn")]
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }
        [JsonProperty("Variabeltype")]
        [JsonPropertyName("Variabeltype")]
        public string Variabeltype { get; set; }
        [JsonProperty("Nivå 2 Kode ny")]
        [JsonPropertyName("Nivå 2 Kode ny")]
        public string Nivaa2KodeNy { get; set; }
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
