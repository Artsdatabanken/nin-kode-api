namespace NinKode.Database.Model.v22
{
    using System.Collections.Generic;

    public class NaturTypeV22
    {
        public int DatabankId { get; set; }
        public string Navn { get; set; }
        public string Kategori { get; set; }
        public string Kode { get; set; }
        public string ElementKode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string[] GrunntypeKoder { get; set; }
        public TrinnV22[] Trinn { get; set; }
        public IDictionary<string, string[]> Kartleggingsenheter { get; set; }
        public string Malestokk { get; set; }
    }
}