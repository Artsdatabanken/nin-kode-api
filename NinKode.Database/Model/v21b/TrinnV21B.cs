namespace NinKode.Database.Model.v21b
{
    public class TrinnV21B
    {
        public string Id { get; set; }
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public SubTrinnV21B[] Trinn { get; set; }
    }
}