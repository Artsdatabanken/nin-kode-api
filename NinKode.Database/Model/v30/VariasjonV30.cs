namespace NinKode.Database.Model.v30
{
    using System.Collections.Generic;

    public class VariasjonV30
    {
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public IList<VariasjonV30> Trinn { get; set; }
    }
}