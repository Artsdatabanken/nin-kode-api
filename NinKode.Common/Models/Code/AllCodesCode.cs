﻿namespace NinKode.Common.Models.Code
{
    using System.Text.Json.Serialization;

    public class AllCodesCode
    {
        [JsonPropertyName("Id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        [JsonPropertyName("Definisjon")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Definition { get; set; }
    }
}