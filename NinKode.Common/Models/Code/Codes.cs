namespace NinKode.Common.Models.Code
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class Codes
    {
        [JsonPropertyName("Navn")]
        public string Navn { get; set; }

        [JsonPropertyName("Kategori")]
        public string Kategori { get; set; }

        [JsonIgnore]
        [JsonPropertyName("Kategori_")]
        public Category Kategori_ { get; set; }

        [JsonPropertyName("Kode")]
        public AllCodesCode Kode { get; set; }

        [JsonPropertyName("ElementKode")]
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
        public Environment[] Miljovariabler { get; set; }
    }

    public enum Category {
        Grunntype,
        Hovedtype,
        Hovedtypegruppe,
        Kartleggingsenhet,
        Naturtypenivaa
    }

    public enum TypeEnum { Miljøvariabel }

    public partial class Codes
    {
        public static Codes[] FromJson(string json) => JsonSerializer.Deserialize<Codes[]>(json);
    }

    public static class Serialize
    {
        public static string ToJson(this Codes self) => JsonSerializer.Serialize(self);
        public static string ToJson(this Codes[] self) => JsonSerializer.Serialize(self);
    }

    internal class Converter : JsonConverterFactory
    {

        //public static readonly JsonSerializerOptions Settings = new JsonSerializerOptions
        //{
        //    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        //    DateParseHandling = DateParseHandling.None,
        //    Converters = {
        //        CategoryConverter.Singleton,
        //        TypeEnumConverter.Singleton,
        //        new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        //    },
        //};
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    //internal class CategoryConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Category?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case "Grunntype":
    //                return Category.Grunntype;
    //            case "Hovedtype":
    //                return Category.Hovedtype;
    //            case "Hovedtypegruppe":
    //                return Category.Hovedtypegruppe;
    //            case "Kartleggingsenhet":
    //                return Category.Kartleggingsenhet;
    //            case "Naturtypenivå":
    //                return Category.Naturtypenivaa;
    //        }
    //        throw new Exception("Cannot unmarshal type Kategori");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Category)untypedValue;
    //        switch (value)
    //        {
    //            case Category.Grunntype:
    //                serializer.Serialize(writer, "Grunntype");
    //                return;
    //            case Category.Hovedtype:
    //                serializer.Serialize(writer, "Hovedtype");
    //                return;
    //            case Category.Hovedtypegruppe:
    //                serializer.Serialize(writer, "Hovedtypegruppe");
    //                return;
    //            case Category.Kartleggingsenhet:
    //                serializer.Serialize(writer, "Kartleggingsenhet");
    //                return;
    //            case Category.Naturtypenivå:
    //                serializer.Serialize(writer, "Naturtypenivå");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type Kategori");
    //    }

    //    public static readonly CategoryConverter Singleton = new CategoryConverter();
    //}

    //internal class TypeEnumConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        if (value == "Miljøvariabel")
    //        {
    //            return TypeEnum.Miljøvariabel;
    //        }
    //        throw new Exception("Cannot unmarshal type TypeEnum");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (TypeEnum)untypedValue;
    //        if (value == TypeEnum.Miljøvariabel)
    //        {
    //            serializer.Serialize(writer, "Miljøvariabel");
    //            return;
    //        }
    //        throw new Exception("Cannot marshal type TypeEnum");
    //    }

    //    public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    //}
}
