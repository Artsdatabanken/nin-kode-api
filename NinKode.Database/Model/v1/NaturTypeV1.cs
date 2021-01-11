namespace NinKode.Database.Model.v1
{
    public class NaturTypeV1
    {
        public int DatabankId { get; set; }
        public string Navn { get; set; }
        public string Kategori { get; set; }
        public string Kode { get; set; }
        public string ElementKode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
    }
}