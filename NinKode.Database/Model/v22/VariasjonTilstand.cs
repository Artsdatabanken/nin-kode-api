namespace NinKode.Database.Model.v22
{
    using Newtonsoft.Json;
    using System;
    using System.Text.Json.Serialization;
    

    public class VariasjonTilstand
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
        [JsonProperty("kode")]
        [JsonPropertyName("kode")]
        public string Kode { get; set; }
        [JsonProperty("Vaiabeltema")]
        [JsonPropertyName("Vaiabeltema")]
        public string Vaiabeltema { get; set; }
        [JsonProperty("Nivå 2 kode")]
        [JsonPropertyName("Nivå 2 kode")]
        public string Nivaa2Kode { get; set; }
        [JsonProperty("Variabel")]
        [JsonPropertyName("Variabel")]
        public string Variabel { get; set; }
        [JsonProperty("Nivå 3 kode")]
        [JsonPropertyName("Nivå 3 kode")]
        public string Nivaa3Kode { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Trinnbeskrivelse")]
        [JsonPropertyName("Trinnbeskrivelse")]
        public string Trinnbeskrivelse { get; set; }
        [JsonProperty("PGM")]
        [JsonPropertyName("PGM")]
        public string PGM { get; set; }
        [JsonProperty("VM")]
        [JsonPropertyName("VM")]
        public string VM { get; set; }
        [JsonProperty("Variabeltype")]
        [JsonPropertyName("Variabeltype")]
        public string Variabeltype { get; set; }
        [JsonProperty("K/TMåleskala")]
        [JsonPropertyName("K/TMåleskala")]
        public string K_TMaaleskala { get; set; }
        [JsonProperty("RS")]
        [JsonPropertyName("RS")]
        public string RS { get; set; }
        [JsonProperty("KG")]
        [JsonPropertyName("KG")]
        public string KG { get; set; }
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
