namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public class NaturtypeGrunntype
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("Nivå")]
        [JsonPropertyName("Nivå")]
        public string Nivaa { get; set; }
        [JsonProperty("Hovedtypegruppe_kode")]
        [JsonPropertyName("Hovedtypegruppe_kode")]
        public string HovedtypegruppeKode { get; set; }
        [JsonProperty("Hovedtype_kode")]
        [JsonPropertyName("Hovedtype_kode")]
        public string HovedtypeKode { get; set; }
        [JsonProperty("HOVEDTYPE_navn")]
        [JsonPropertyName("HOVEDTYPE_navn")]
        public string HovedtypeNavn { get; set; }
        [JsonProperty("Sammensatt_Kode")]
        [JsonPropertyName("Sammensatt_Kode")]
        public string SammensattKode { get; set; }
        [JsonProperty("Navn_GRUNNTYPE 1:500 (langnavn)")]
        [JsonPropertyName("Navn_GRUNNTYPE 1:500 (langnavn)")]
        public string NavnGrunntypeLong { get; set; }
        [JsonProperty("Navn_GRUNNTYPE 1:500 (populærnavn)")]
        [JsonPropertyName("Navn_GRUNNTYPE 1:500 (populærnavn)")]
        public string NavnGrunntypeShort { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp{ get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
    }
}
