namespace NinKode.Common.Models.Code
{
    using System.Text.Json.Serialization;

    public class EnvironmentVariable
    {
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }

        [JsonPropertyName("Kode")]
        public string Kode { get; set; }

        [JsonPropertyName("LKM Kategori")]
        public string LKMKategori { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Trinn")]
        public Step[] Trinn { get; set; }
    }
}