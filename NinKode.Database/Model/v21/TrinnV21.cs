namespace NinKode.Database.Model.v21
{
    public class TrinnV21
    {
        public string Id { get; set; }
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public SubTrinnV21[] Trinn { get; set; }
    }
}