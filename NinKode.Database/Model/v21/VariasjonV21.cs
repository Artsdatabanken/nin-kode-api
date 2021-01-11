namespace NinKode.Database.Model.v21
{
    using System.Collections.Generic;

    public class VariasjonV21
    {
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public IList<VariasjonV21> Trinn { get; set; }
    }
}