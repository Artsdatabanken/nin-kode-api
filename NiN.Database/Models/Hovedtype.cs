﻿namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Converters;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Enums;

    public class Hovedtype : BaseEntity
    {
        [StringLength(255)]
        public string Kategori => NinEnumConverter.GetValue<KategoriEnum>(Kode.Kategori);

        public virtual HovedtypeKode Kode { get; set; }

        public virtual Hovedtypegruppe Hovedtypegruppe { get; set; }

        public virtual ICollection<Grunntype> UnderordnetKoder { get; set; } = new List<Grunntype>();

        public virtual ICollection<Kartleggingsenhet> Kartleggingsenheter { get; set; } = new List<Kartleggingsenhet>();

        public virtual ICollection<Miljovariabel> Miljovariabler { get; set; } = new List<Miljovariabel>();
    }
}