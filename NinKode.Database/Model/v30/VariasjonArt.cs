namespace NinKode.Database.Model.v30
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class VariasjonArt
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("Besys1")]
        [JsonPropertyName("Besys1")]
        public string Besys1 { get; set; }
        [JsonProperty("Nivå 1 kode")]
        [JsonPropertyName("Nivå 1 kode")]
        public string Nivaa1kode { get; set; }
        [JsonProperty("col_2")]
        [JsonPropertyName("col_2")]
        public string col2 { get; set; }
        [JsonProperty("Nivå 2 kode")]
        [JsonPropertyName("Nivå 2 kode")]
        public string Nivaa2kode { get; set; }
        [JsonProperty("Nivå 2 beskrivelse")]
        [JsonPropertyName("Nivå 2 beskrivelse")]
        public string Nivaa2beskrivelse { get; set; }
        [JsonProperty("Trinn")]
        [JsonPropertyName("Trinn")]
        public string Trinn { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Navn [og forklaring]")]
        [JsonPropertyName("Navn [og forklaring]")]
        public string NavnForklaring { get; set; }
        [JsonProperty("Type")]
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonProperty("Måle-skala")]
        [JsonPropertyName("Måle-skala")]
        public string MaaleSkala { get; set; }
        [JsonProperty("Nivå 2 kode ny")]
        [JsonPropertyName("Nivå 2 kode ny")]
        public string Nivaa2KodeNy { get; set; }
        [JsonProperty("Nivå 2 Tags")]
        [JsonPropertyName("Nivå 2 Tags")]
        public string Nivaa2Tags { get; set; }
        [JsonProperty("Tags")]
        [JsonPropertyName("Tags")]
        public string Tags { get; set; }
        [JsonProperty("Scientificnameid")]
        [JsonPropertyName("Scientificnameid")]
        public string ScientificnameId { get; set; }
        [JsonProperty("Scientificname")]
        [JsonPropertyName("Scientificname")]
        public string Scientificname { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp { get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
    }
}
