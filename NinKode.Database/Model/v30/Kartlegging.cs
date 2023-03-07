namespace NinKode.Database.Model.v30
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class Kartlegging
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("Nivå")]
        [JsonPropertyName("Nivå")]
        public string Nivaa { get; set; }
        [JsonProperty("Hovedtype_kode")]
        [JsonPropertyName("Hovedtype_kode")]
        public string HovedtypeKode { get; set; }
        [JsonProperty("Hovedtype_navn")]
        [JsonPropertyName("Hovedtype_navn")]
        public string HovedtypeNavn { get; set; }
        [JsonProperty("Kode")]
        [JsonPropertyName("Kode")]
        public string Kode { get; set; }
        [JsonProperty("Sammensatt_kode")]
        [JsonPropertyName("Sammensatt_kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Navn_Kartleggingsenheter 1:2500 for terrestre systemer")]
        [JsonPropertyName("Navn_Kartleggingsenheter 1:2500 for terrestre systemer")]
        public string NavnKartleggingsenheter2500 { get; set; }
        [JsonProperty("Navn_Kartleggingsenheter 1:5000 for terrestre systemer")]
        [JsonPropertyName("Navn_Kartleggingsenheter 1:5000 for terrestre systemer")]
        public string NavnKartleggingsenheter5000 { get; set; }
        [JsonProperty("Navn_Kartleggingsenheter 1:10000 for terrestre systemer")]
        [JsonPropertyName("Navn_Kartleggingsenheter 1:10000 for terrestre systemer")]
        public string NavnKartleggingsenheter10000 { get; set; }
        [JsonProperty("Navn_Kartleggingsenheter 1:20 000 for terrestre systemer")]
        [JsonPropertyName("Navn_Kartleggingsenheter 1:20 000 for terrestre systemer")]
        public string NavnKartleggingsenheter20000 { get; set; }
        [JsonProperty("Grunntypenr")]
        [JsonPropertyName("Grunntypenr")]
        public string Grunntypenr { get; set; }
        [JsonProperty("Grunntypekoder")]
        [JsonPropertyName("Grunntypekoder")]
        public string Grunntypekoder { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp{ get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
    }
}
