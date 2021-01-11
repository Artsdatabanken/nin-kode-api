namespace NinKode.Common.Models.Code
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Codes
    {
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }

        [JsonPropertyName("Kategori")]
        public string Kategori { get; set; }

        [JsonPropertyName("Kode")]
        public AllCodesCode Kode { get; set; }

        [JsonPropertyName("ElementKode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ElementKode { get; set; }

        [JsonPropertyName("OverordnetKode")]
        public AllCodesCode OverordnetKode { get; set; }

        [JsonPropertyName("UnderordnetKoder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AllCodesCode[] UnderordnetKoder { get; set; }

        [JsonPropertyName("Beskrivelse")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Uri Beskrivelse { get; set; }

        [JsonPropertyName("Kartleggingsenheter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, AllCodesCode[]> Kartleggingsenheter { get; set; }

        [JsonPropertyName("Miljovariabler")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EnvironmentVariable[] Miljovariabler { get; set; }
    }

    //public partial class Codes
    //{
    //    public static Codes[] FromJson(string json) => JsonSerializer.Deserialize<Codes[]>(json);
    //}
}
