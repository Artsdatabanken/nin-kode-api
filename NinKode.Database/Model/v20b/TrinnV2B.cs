namespace NinKode.Database.Model.v20b
{
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
}