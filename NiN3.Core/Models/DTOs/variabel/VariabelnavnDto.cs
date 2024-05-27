using NiN3.Core.Models.Enums;

namespace NiN3.Core.Models.DTOs.variabel
{
    public class VariabelnavnDto
    {
        public string Navn { get; set; }
        public KodeDto Kode { get; set; } = new KodeDto();

        public Variabelkategori2Enum? Variabelkategori2Enum { get; set; }
        public string Variabelkategori2Navn { get; set; }
        public VariabeltypeEnum? VariabeltypeEnum { get; set; }
        public string VariabeltypeNavn { get; set; }
        //public string Variabelgruppe { get; set; }
        public VariabelgruppeEnum? VariabelgruppeEnum { get; set; }
        public string VariabelgruppeNavn { get; set; }

        public ICollection<MaaleskalaDto> Variabeltrinn { get; set; } = new List<MaaleskalaDto>();

        public ICollection<KonverteringDto> Konverteringer { get; set; } = new List<KonverteringDto>();
    }
}
