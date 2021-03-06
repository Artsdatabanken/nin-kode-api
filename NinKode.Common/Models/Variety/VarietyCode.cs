﻿namespace NinKode.Common.Models.Variety
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class VarietyCode
    {
        [JsonPropertyName("Navn")]
        public string Name { get; set; }

        [JsonPropertyName("Kode")]
        public VarietyCodeCode Code { get; set; }

        [JsonPropertyName("OverordnetKode")]
        public VarietyCodeCode OverordnetKode { get; set; }

        [JsonPropertyName("UnderordnetKoder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public VarietyCodeCode[] UnderordnetKoder { get; set; }
    }

    public partial class VarietyCode
    {
        public static VarietyCode FromJson(string json) => JsonSerializer.Deserialize<VarietyCode>(json);
    }
}
