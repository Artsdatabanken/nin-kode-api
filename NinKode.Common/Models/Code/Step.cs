namespace NinKode.Common.Models.Code
{
    using System.Text.Json.Serialization;

    public class Step
    {
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }

        [JsonPropertyName("Kode")]
        public string Kode { get; set; }

        [JsonPropertyName("Basistrinn")]
        public string Basistrinn { get; set; }
    }
}