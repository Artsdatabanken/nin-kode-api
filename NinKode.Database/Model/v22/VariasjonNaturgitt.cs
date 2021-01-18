namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class VariasjonNaturgitt
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("Besys")]
        [JsonPropertyName("Besys")]
        public string Besys { get; set; }
        [JsonProperty("Nivå 1 kode")]
        [JsonPropertyName("Nivå 1 kode")]
        public string Nivaa1kode { get; set; }
        [JsonProperty("col_2")]
        [JsonPropertyName("col_2")]
        public string col_2 { get; set; }
        [JsonProperty("col_3")]
        [JsonPropertyName("col_3")]
        public string col_3 { get; set; }
        [JsonProperty("Nivå 2 beskrivelse")]
        [JsonPropertyName("Nivå 2 beskrivelse")]
        public string Nivaa2Beskrivelse { get; set; }
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
        [JsonProperty("Nivå 2 tags")]
        [JsonPropertyName("Nivå 2 tags")]
        public string Nivaa2Tags { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp { get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
    }
}
