namespace NinKode.Database.Model.v20b
{
    using System.Collections.Generic;

    public class VariasjonV2B
    {
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public IList<VariasjonV2B> Trinn { get; set; }
    }
}