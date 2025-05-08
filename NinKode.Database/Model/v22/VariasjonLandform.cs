namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public class VariasjonLandform
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
        [JsonProperty("Nivå 1 kode")]
        [JsonPropertyName("Nivå 1 kode")]
        public string Nivaa1kode { get; set; }
        [JsonProperty("col_3")]
        [JsonPropertyName("col_3")]
        public string col_3 { get; set; }
        [JsonProperty("Nivå 2 kode")]
        [JsonPropertyName("Nivå 2 kode")]
        public string Nivaa2Kode { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Navn")]
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }
        [JsonProperty("Variabeltype")]
        [JsonPropertyName("Variabeltype")]
        public string Variabeltype { get; set; }
        [JsonProperty("Romlig skala")]
        [JsonPropertyName("Romlig skala")]
        public string RomligSkala { get; set; }
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
