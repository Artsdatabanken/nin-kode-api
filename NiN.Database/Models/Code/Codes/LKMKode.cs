namespace NiN.Database.Models.Code.Codes
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Code.Enums;

    public class LKMKode : BaseIdEntity
    {
        public LKMKode() { }

        [StringLength(25)]
        public string Kode { get; set; }

        public LkmKategoriEnum LkmKategori { get; set; }
    }
}