namespace NiN.Database.Models.Code.Codes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;

    public class NinKode : BaseIdEntity
    {
        [StringLength(255)]
        public string KodeName { get; set; }
        
        [StringLength(1000)]
        public string Definisjon { get; set; }

        public KategoriEnum Kategori { get; set; }
    }

    public class NatursystemKode : NinKode
    {
        public Guid NatursystemId { get; set; }

        [ForeignKey(nameof(NatursystemId))]
        public virtual Natursystem Natursystem { get; set; }

        public NatursystemKode()
        {
            Kategori = KategoriEnum.Naturmangfoldnivå;
        }
    }

    public class HovedtypegruppeKode : NinKode
    {
        public Guid HovedtypegruppeId { get; set; }

        [ForeignKey(nameof(HovedtypegruppeId))]
        public virtual Hovedtypegruppe Hovedtypegruppe { get; set; }

        public HovedtypegruppeKode()
        {
            Kategori = KategoriEnum.Hovedtypegruppe;
        }
    }

    public class HovedtypeKode : NinKode
    {
        public Guid HovedtypeId { get; set; }

        [ForeignKey(nameof(HovedtypeId))]
        public virtual Hovedtype Hovedtype { get; set; }

        public HovedtypeKode()
        {
            Kategori = KategoriEnum.Hovedtype;
        }
    }

    public class GrunntypeKode : NinKode
    {
        public Guid GrunntypeId { get; set; }

        [ForeignKey(nameof(GrunntypeId))]
        public virtual Grunntype Grunntype { get; set; }

        public GrunntypeKode()
        {
            Kategori = KategoriEnum.Grunntype;
        }
    }

    public class KartleggingsenhetKode : NinKode
    {
        public Guid KartleggingsenhetId { get; set; }

        [ForeignKey(nameof(KartleggingsenhetId))]
        public virtual Kartleggingsenhet Kartleggingsenhet { get; set; }

        public KartleggingsenhetKode()
        {
            Kategori = KategoriEnum.Kartleggingsenhet;
        }
    }

    public class TrinnKode : NinKode
    {
        public Guid TrinnId { get; set; }

        [ForeignKey(nameof(TrinnId))]
        public virtual Trinn Trinn { get; set; }

        public TrinnKode()
        {
            Kategori = KategoriEnum.Trinn;
        }
    }

    //public class BasistrinnKode : NinKode
    //{
    //    public int BasistrinnId { get; set; }

    //    [ForeignKey(nameof(BasistrinnId))]
    //    public virtual Basistrinn Basistrinn { get; set; }

    //    public BasistrinnKode()
    //    {
    //        Kategori = KategoriEnum.Basistrinn;
    //    }
    //}
}