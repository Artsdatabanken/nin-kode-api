namespace NiN.Database.Models.Codes
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using NiN.Database.Models.Enums;

    public class Kode
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(10)]
        public string KodeName { get; set; }
        
        [StringLength(1000)]
        public string Definisjon { get; set; }

        public KategoriEnum Kategori { get; set; }
    }

    public class NatursystemKode : Kode
    {
        public int NatursystemId { get; set; }

        [ForeignKey(nameof(NatursystemId))]
        public virtual Natursystem Natursystem { get; set; }

        public NatursystemKode()
        {
            Kategori = KategoriEnum.Naturmangfoldnivå;
        }
    }

    public class HovedtypegruppeKode : Kode
    {
        public int HovedtypegruppeId { get; set; }

        [ForeignKey(nameof(HovedtypegruppeId))]
        public virtual Hovedtypegruppe Hovedtypegruppe { get; set; }

        public HovedtypegruppeKode()
        {
            Kategori = KategoriEnum.Hovedtypegruppe;
        }
    }

    public class HovedtypeKode : Kode
    {
        public int HovedtypeId { get; set; }

        [ForeignKey(nameof(HovedtypeId))]
        public virtual Hovedtype Hovedtype { get; set; }

        public HovedtypeKode()
        {
            Kategori = KategoriEnum.Hovedtype;
        }
    }

    public class GrunntypeKode : Kode
    {
        public int GrunntypeId { get; set; }

        [ForeignKey(nameof(GrunntypeId))]
        public virtual Grunntype Grunntype { get; set; }

        public GrunntypeKode()
        {
            Kategori = KategoriEnum.Grunntype;
        }
    }
}