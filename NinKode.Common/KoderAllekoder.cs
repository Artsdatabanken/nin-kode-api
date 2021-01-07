namespace NinKode.Common
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class KoderAllekoder
    {
        [JsonProperty("Navn")]
        public string Navn { get; set; }

        [JsonProperty("Kategori")]
        public Kategori Kategori { get; set; }

        [JsonProperty("Kode")]
        public KoderKode Kode { get; set; }

        [JsonProperty("ElementKode")]
        public string ElementKode { get; set; }

        [JsonProperty("OverordnetKode")]
        public KoderKode OverordnetKode { get; set; }

        [JsonProperty("UnderordnetKoder", NullValueHandling = NullValueHandling.Ignore)]
        public KoderKode[] UnderordnetKoder { get; set; }

        [JsonProperty("Beskrivelse", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Beskrivelse { get; set; }

        [JsonProperty("Miljovariabler", NullValueHandling = NullValueHandling.Ignore)]
        public Miljovariabler[] Miljovariabler { get; set; }

        [JsonProperty("Kartleggingsenheter", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, KoderKode[]> Kartleggingsenheter { get; set; }
    }

    public class KoderKode
    {
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("Definisjon")]
        public string Definisjon { get; set; }
    }

    public class Miljovariabler
    {
        [JsonProperty("Navn")]
        public string Navn { get; set; }

        [JsonProperty("Kode")]
        public string Kode { get; set; }

        [JsonProperty("Type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("Trinn")]
        public Trinn[] Trinn { get; set; }
    }

    public class Trinn
    {
        [JsonProperty("Navn")]
        public string Navn { get; set; }

        [JsonProperty("Kode")]
        public string Kode { get; set; }
    }

    public enum Kategori { Grunntype, Hovedtype, Hovedtypegruppe, Kartleggingsenhet, Naturtypenivå };

    public enum TypeEnum { Miljøvariabel };

    public partial class KoderAllekoder
    {
        public static KoderAllekoder[] FromJson(string json) => JsonConvert.DeserializeObject<KoderAllekoder[]>(json);
    }

    //public static class Serialize
    //{
    //    public static string ToJson(this TopLevel[] self) => JsonConvert.SerializeObject(self);
    //}

    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters = {
    //            KategoriConverter.Singleton,
    //            TypeEnumConverter.Singleton,
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}

    internal class KategoriConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Kategori) || t == typeof(Kategori?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Grunntype":
                    return Kategori.Grunntype;
                case "Hovedtype":
                    return Kategori.Hovedtype;
                case "Hovedtypegruppe":
                    return Kategori.Hovedtypegruppe;
                case "Kartleggingsenhet":
                    return Kategori.Kartleggingsenhet;
                case "Naturtypenivå":
                    return Kategori.Naturtypenivå;
            }
            throw new Exception("Cannot unmarshal type Kategori");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Kategori)untypedValue;
            switch (value)
            {
                case Kategori.Grunntype:
                    serializer.Serialize(writer, "Grunntype");
                    return;
                case Kategori.Hovedtype:
                    serializer.Serialize(writer, "Hovedtype");
                    return;
                case Kategori.Hovedtypegruppe:
                    serializer.Serialize(writer, "Hovedtypegruppe");
                    return;
                case Kategori.Kartleggingsenhet:
                    serializer.Serialize(writer, "Kartleggingsenhet");
                    return;
                case Kategori.Naturtypenivå:
                    serializer.Serialize(writer, "Naturtypenivå");
                    return;
            }
            throw new Exception("Cannot marshal type Kategori");
        }

        public static readonly KategoriConverter Singleton = new KategoriConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Miljøvariabel")
            {
                return TypeEnum.Miljøvariabel;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.Miljøvariabel)
            {
                serializer.Serialize(writer, "Miljøvariabel");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
