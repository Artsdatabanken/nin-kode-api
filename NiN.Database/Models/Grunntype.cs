namespace NiN.Database.Models
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Converters;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Enums;

    public class Grunntype : BaseEntity
    {
        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);
        
        public virtual Hovedtype Hovedtype { get; set; }

        public virtual GrunntypeKode Kode { get; set; }
    }
}