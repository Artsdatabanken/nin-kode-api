namespace NinKode.Database.Model.v30
{
    public class TrinnV30
    {
        public string Id { get; set; }
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string LKMKategori { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public SubTrinnV30[] Trinn { get; set; }
    }
}