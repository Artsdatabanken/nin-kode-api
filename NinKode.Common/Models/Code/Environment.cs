namespace NinKode.Common.Models.Code
{
    using System.Text.Json.Serialization;

    public class Environment
    {
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }

        [JsonPropertyName("Kode")]
        public string Kode { get; set; }

        [JsonPropertyName("Type")]
        public TypeEnum Type { get; set; }

        [JsonPropertyName("Trinn")]
        public Step[] Trinn { get; set; }
    }
}