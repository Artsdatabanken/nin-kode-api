namespace NinKode.Common.Models.Variety
{
    using System;
    using System.Text.Json.Serialization;

    public class VarietyCodeCode
    {
        [JsonPropertyName("Id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        [JsonPropertyName("Definisjon")]
        public string Definition { get; set; }
    }
}