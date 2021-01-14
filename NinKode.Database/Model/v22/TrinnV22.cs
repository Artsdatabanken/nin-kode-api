namespace NinKode.Database.Model.v22
{
    public class TrinnV22
    {
        public string Id { get; set; }
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public SubTrinnV22[] Trinn { get; set; }
    }
}