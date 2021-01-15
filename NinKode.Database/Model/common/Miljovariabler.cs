namespace NinKode.Database.Model.common
{
    using System;
    using System.Linq;

    public class Miljovariabler
    {
        public string altkode { get; set; }
        public LkmKoder[] barn { get; set; }

        public string GetTittelByKode(string kode)
        {
            return barn == null
                ? null
                : (from lkmKoder in barn
                    where lkmKoder.trinnkode.Equals(kode, StringComparison.OrdinalIgnoreCase)
                    select lkmKoder.tittel.nb).FirstOrDefault();
        }
    }
}
