namespace NinKode.Database.Model.v22
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public class HovedtypeTrinn
    {
        [JsonProperty("docId")]
        [JsonPropertyName("docId")]
        public string docId { get; set; }
        [JsonProperty("HT")]
        [JsonPropertyName("HT")]
        public string HT { get; set; }
        [JsonProperty("Hovedtypenavn")]
        [JsonPropertyName("Hovedtypenavn")]
        public string HovedtypeNavn { get; set; }
        [JsonProperty("LKM kategori")]
        [JsonPropertyName("LKM kategori")]
        public string LkmKategori { get; set; }
        [JsonProperty("Gradientkodedefinisjon")]
        [JsonPropertyName("Gradientkodedefinisjon")]
        public string Gradientkodedefinisjon { get; set; }
        [JsonProperty("Gradientkode")]
        [JsonPropertyName("Gradientkode")]
        public string Gradientkode { get; set; }
        [JsonProperty("Trinn1")]
        [JsonPropertyName("Trinn1")]
        public string Trinn1 { get; set; }
        [JsonProperty("Trinn1 csv")]
        [JsonPropertyName("Trinn1 csv")]
        public string Trinn1Csv { get; set; }
        [JsonProperty("Trinn1 navn")]
        [JsonPropertyName("Trinn1 navn")]
        public string Trinn1Navn { get; set; }
        [JsonProperty("Trinn2")]
        [JsonPropertyName("Trinn2")]
        public string Trinn2 { get; set; }
        [JsonProperty("Trinn2 csv")]
        [JsonPropertyName("Trinn2 csv")]
        public string Trinn2Csv { get; set; }
        [JsonProperty("Trinn2 navn")]
        [JsonPropertyName("Trinn2 navn")]
        public string Trinn2Navn { get; set; }
        [JsonProperty("Trinn3")]
        [JsonPropertyName("Trinn3")]
        public string Trinn3 { get; set; }
        [JsonProperty("Trinn3 csv")]
        [JsonPropertyName("Trinn3 csv")]
        public string Trinn3Csv { get; set; }
        [JsonProperty("Trinn3 navn")]
        [JsonPropertyName("Trinn3 navn")]
        public string Trinn3Navn { get; set; }
        [JsonProperty("Trinn4")]
        [JsonPropertyName("Trinn4")]
        public string Trinn4 { get; set; }
        [JsonProperty("Trinn4 csv")]
        [JsonPropertyName("Trinn4 csv")]
        public string Trinn4Csv { get; set; }
        [JsonProperty("Trinn4 navn")]
        [JsonPropertyName("Trinn4 navn")]
        public string Trinn4Navn { get; set; }
        [JsonProperty("Trinn5")]
        [JsonPropertyName("Trinn5")]
        public string Trinn5 { get; set; }
        [JsonProperty("Trinn5 csv")]
        [JsonPropertyName("Trinn5 csv")]
        public string Trinn5Csv { get; set; }
        [JsonProperty("Trinn5 navn")]
        [JsonPropertyName("Trinn5 navn")]
        public string Trinn5Navn { get; set; }
        [JsonProperty("Trinn6")]
        [JsonPropertyName("Trinn6")]
        public string Trinn6 { get; set; }
        [JsonProperty("Trinn6 csv")]
        [JsonPropertyName("Trinn6 csv")]
        public string Trinn6Csv { get; set; }
        [JsonProperty("Trinn6 navn")]
        [JsonPropertyName("Trinn6 navn")]
        public string Trinn6Navn { get; set; }
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
