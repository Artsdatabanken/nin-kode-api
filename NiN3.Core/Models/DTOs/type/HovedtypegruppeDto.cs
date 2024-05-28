namespace NiN3.Core.Models.DTOs.type
{
    public class HovedtypegruppeDto
    {
        public string Navn { get; set; }
        public KodeDto Kode { get; set; }
        public string Kategori { get; set; }
        //public string Typekategori3 { get; set; }
        public NiN3.Core.Models.Enums.Typekategori3Enum? Typekategori3Enum { get; set; }
        public string Typekategori3Navn { get; set; }
        public ICollection<HovedoekosystemDto> Hovedoekosystemer { get; set; } = new List<HovedoekosystemDto>();
        public ICollection<HovedtypeDto> Hovedtyper { get; set; } = new List<HovedtypeDto>();
        public ICollection<KonverteringDto> konverteringer { get; set; } = new List<KonverteringDto>();        
    }
}
