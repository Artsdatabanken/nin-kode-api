namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Abstractions.Data;
    using Raven.Imports.Newtonsoft.Json;

    public class VariasjonMenneskeskapt
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
        [JsonProperty("col_5")]
        [JsonPropertyName("col_5")]
        public string col_5 { get; set; }
        [JsonProperty("Nivå 3 kode")]
        [JsonPropertyName("Nivå 3 kode")]
        public string Nivaa3Kode { get; set; }
        [JsonProperty("Nivå2_kode")]
        [JsonPropertyName("Nivå2_kode")]
        public string Nivaa2_Kode { get; set; }
        [JsonProperty("Navn")]
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }
        [JsonProperty("Trinn")]
        [JsonPropertyName("Trinn")]
        public string Trinn { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Verdi")]
        [JsonPropertyName("Verdi")]
        public string Verdi { get; set; }
        [JsonProperty("Type")]
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonProperty("RS")]
        [JsonPropertyName("RS")]
        public string RS { get; set; }
        [JsonProperty("Nivå 2 kode ny")]
        [JsonPropertyName("Nivå 2 kode ny")]
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
