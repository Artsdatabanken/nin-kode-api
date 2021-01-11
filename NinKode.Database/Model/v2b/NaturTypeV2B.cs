namespace NinKode.Database.Model.v2b
{
    using System.Collections.Generic;
    using Raven.Abstractions.Counters.Notifications;

    public class NaturTypeV2B
    {
        public int DatabankId { get; set; }
        public string Navn { get; set; }
        public string Kategori { get; set; }
        public string Kode { get; set; }
        public string ElementKode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string[] GrunntypeKoder { get; set; }
        public TrinnV2B[] Trinn { get; set; }
        public IDictionary<string, string[]> Kartleggingsenheter { get; set; }
        public string Malestokk { get; set; }
    }

    public class TrinnV2B
    {
        public string Id { get; set; }
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public SubTrinnV2B[] Trinn { get; set; }
    }

    public class SubTrinnV2B
    {
        public string Navn { get; set; }
        public string Kode { get; set; }
    }
}