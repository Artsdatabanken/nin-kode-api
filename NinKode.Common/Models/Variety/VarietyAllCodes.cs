namespace NinKode.Common.Models.Variety
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class VarietyAllCodes
    {
        [JsonPropertyName("Navn")]
        public string Name { get; set; }

        [JsonPropertyName("VariasjonKode")]
        public VarietyAllCodesCode Code { get; set; }

        [JsonPropertyName("OverordnetKode")]
        public VarietyAllCodesCode OverordnetKode { get; set; }

        [JsonPropertyName("UnderordnetKoder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public VarietyAllCodesCode[] UnderordnetKoder { get; set; }
    }

    public partial class VarietyAllCodes
    {
        public static VarietyAllCodes[] FromJson(string json) => JsonSerializer.Deserialize<VarietyAllCodes[]>(json);
    }

    //public static class Serialize
    //{
    //    public static string ToJson(this VarietyAllCodes[] self) => JsonConvert.SerializeObject(self);
    //}

    ////internal static class Converter
    ////{
    ////    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    ////    {
    ////        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    ////        DateParseHandling = DateParseHandling.None,
    ////        Converters = {
    ////            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    ////        },
    ////    };
    ////}
}
