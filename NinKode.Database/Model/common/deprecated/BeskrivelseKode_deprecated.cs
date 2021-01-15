namespace NinKode.Database.Model.common.deprecated
{
    using System;

    public class BeskrivelseKode_deprecated
    {
        public int index { get; set; }
        
        public string kode { get; set; }

        public string trinnkode => string.IsNullOrEmpty(kode)
            ? ""
            : $"BeSys{kode.Substring(kode.LastIndexOf("-", StringComparison.Ordinal) + 1)}";

        public string parentkode => string.IsNullOrEmpty(kode)
            ? ""
            : kode.Substring(kode.LastIndexOf("-", StringComparison.Ordinal) + 1) != "0"
                ? "BeSys0"
                : "";

        public Title tittel { get; set; }
    }
}