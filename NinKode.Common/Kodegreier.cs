namespace NinKode.Common
{
    using System;

    using Newtonsoft.Json;

    public partial class Kodegreier
    {
        [JsonProperty("Navn")]
        public string Navn { get; set; }

        [JsonProperty("Kode")]
        public KodeKode Kode { get; set; }

        [JsonProperty("OverordnetKode")]
        public KodeKode OverordnetKode { get; set; }

        [JsonProperty("UnderordnetKoder")]
        public KodeKode[] UnderordnetKoder { get; set; }
    }

    public partial class KodeKode
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Definisjon")]
        public Uri Definisjon { get; set; }
    }

    public partial class Kodegreier
    {
        public static Kodegreier FromJson(string json) => JsonConvert.DeserializeObject<Kodegreier>(json);
    }

    //public static class Serialize
    //{
    //    public static string ToJson(this Kodegreier self) => JsonConvert.SerializeObject(self);
    //}

    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters = {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}
