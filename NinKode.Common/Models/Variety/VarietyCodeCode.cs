namespace NinKode.Common.Models.Variety
{
    using System;
    using System.Text.Json.Serialization;

    public class VarietyCodeCode
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("Definisjon")]
        public Uri Definition { get; set; }
    }
}