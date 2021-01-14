namespace NinKode.Database.Model.v22
{
    using System.Collections.Generic;

    public class VariasjonV22
    {
        public string Navn { get; set; }
        public string Kode { get; set; }
        public string OverordnetKode { get; set; }
        public string[] UnderordnetKoder { get; set; }
        public string Type { get; set; }
        public IList<VariasjonV22> Trinn { get; set; }
    }
}