namespace NinKode.Database.Model.common.deprecated
{
    using System;
    using System.Linq;

    public class BeskrivelsesSystem_deprecated
    {
        public BeskrivelseKode_deprecated[] barn { get; set; }

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
