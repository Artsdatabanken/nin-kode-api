namespace NiN.Database.Models.Code
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Converters;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class Hovedtypegruppe : BaseEntity
    {
        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);

        [Required]
        public virtual HovedtypegruppeKode Kode { get; set; }

        public virtual Natursystem Natursystem { get; set; }

        public virtual ICollection<Hovedtype> UnderordnetKoder { get; set; } = new List<Hovedtype>();
    }
}