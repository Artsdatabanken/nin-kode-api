namespace NinKode.Database.Model.v30
{
    using System;
    using System.Text.Json.Serialization;
    using Raven.Imports.Newtonsoft.Json;

    public class BeskrivelsesSystem
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("Nivå 1 Kode")]
        [JsonPropertyName("Nivå 1 Kode")]
        public string Nivaa1Kode { get; set; }
        [JsonProperty("Variabelgruppe")]
        [JsonPropertyName("Variabelgruppe")]
        public string Variabelgruppe { get; set; }
        [JsonProperty("Nivå 2 kode")]
        [JsonPropertyName("Nivå 2 kode")]
        public string Nivaa2Kode { get; set; }
        [JsonProperty("col_3")]
        [JsonPropertyName("col_3")]
        public string col3 { get; set; }
        [JsonProperty("DTG_Timestamp")]
        [JsonPropertyName("DTG_Timestamp")]
        public DateTime DtgTimestamp { get; set; }
        [JsonProperty("DTG_Ticks")]
        [JsonPropertyName("DTG_Ticks")]
        public long DtgTicks { get; set; }
        public string Kode => $"BeSys{Nivaa1Kode}";
        public string OverordnetKode => Nivaa1Kode.Equals("0")
            ? ""
            : "BeSys0";
        public string[] UnderordnetKoder => string.IsNullOrEmpty(Nivaa2Kode)
            ? null
            : Nivaa2Kode.Split(',', StringSplitOptions.RemoveEmptyEntries);
    }
}
