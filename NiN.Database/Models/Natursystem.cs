namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Converters;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Enums;

    public class Natursystem : BaseEntity
    {
        public Natursystem()
        {
            UnderordnetKoder = new List<Hovedtypegruppe>();
        }

        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);

        [Required]
        public virtual NatursystemKode Kode { get; set; }

        public virtual ICollection<Hovedtypegruppe> UnderordnetKoder { get; set; }
    }
}