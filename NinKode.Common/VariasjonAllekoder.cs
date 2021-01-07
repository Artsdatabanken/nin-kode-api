namespace NinKode.Common
{
    using System.Globalization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class VariasjonAllekoder
    {
        [JsonProperty("Navn")]
        public string Navn { get; set; }

        [JsonProperty("VariasjonKode")]
        public VariasjonKode Kode { get; set; }

        [JsonProperty("OverordnetKode")]
        public VariasjonKode OverordnetKode { get; set; }

        [JsonProperty("UnderordnetKoder", NullValueHandling = NullValueHandling.Ignore)]
        public VariasjonKode[] UnderordnetKoder { get; set; }
    }

    public class VariasjonKode
    {
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("Definisjon")]
        public string Definisjon { get; set; }
    }

    public partial class VariasjonAllekoder
    {
        public static VariasjonAllekoder[] FromJson(string json) => JsonConvert.DeserializeObject<VariasjonAllekoder[]>(json);
    }

    //public static class Serialize
    //{
    //    public static string ToJson(this VariasjonAllekoder[] self) => JsonConvert.SerializeObject(self);
    //}

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
