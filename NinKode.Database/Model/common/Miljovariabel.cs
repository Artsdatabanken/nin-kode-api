namespace NinKode.Database.Model.common
{
    using System;
    using System.Linq;

    public class Miljovariabel
    {
        public string altkode { get; set; }
        public LkmKoder[] barn { get; set; }

        public string GetTitleByKode(string kode)
        {
            return barn == null
                ? null
                : (from child in barn
                    where child.trinnkode.Equals(kode, StringComparison.OrdinalIgnoreCase)
                    select child.tittel.nb).FirstOrDefault();
        }
    }
}
