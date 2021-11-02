namespace NiN.Database.Models.Codes
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using NiN.Database.Models.Enums;
    using NiN.Database.Models;
    using NiN.Database.Models.Common;

    public class Kode : BaseIdEntity
    {
        [StringLength(25)]
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

    public class KartleggingsenhetKode : Kode
    {
        public int KartleggingsenhetId { get; set; }

        [ForeignKey(nameof(KartleggingsenhetId))]
        public virtual Kartleggingsenhet Kartleggingsenhet { get; set; }

        public KartleggingsenhetKode()
        {
            Kategori = KategoriEnum.Kartleggingsenhet;
        }
    }

    public class TrinnKode : Kode
    {
        public int TrinnId { get; set; }

        [ForeignKey(nameof(TrinnId))]
        public virtual Trinn Trinn { get; set; }

        public TrinnKode()
        {
            Kategori = KategoriEnum.Trinn;
        }
    }

    public class BasistrinnKode : Kode
    {
        public int BasistrinnId { get; set; }

        [ForeignKey(nameof(BasistrinnId))]
        public virtual Basistrinn Basistrinn { get; set; }

        public BasistrinnKode()
        {
            Kategori = KategoriEnum.Basistrinn;
        }
    }
}