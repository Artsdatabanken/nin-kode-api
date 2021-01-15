namespace NinKode.Database.Model.common
{
    using System;

    public class LkmKoder
    {
        public int index { get; set; }
        public string kode { get; set; }

        public string trinnkode => string.IsNullOrEmpty(kode)
            ? ""
            : kode.Substring(kode.LastIndexOf("-", StringComparison.Ordinal) + 1);
        public Title tittel { get; set; }
    }
}
