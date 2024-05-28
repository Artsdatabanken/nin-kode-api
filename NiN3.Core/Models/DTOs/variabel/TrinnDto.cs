namespace NiN3.Core.Models.DTOs.variabel
{
    public class TrinnDto
    {
        public string Verdi { get; set; }
        public string? Langkode { get; set; }
        public string Kode { get; set; }
        public string Beskrivelse { get; set; }

        public bool Registert { get; set; } = false;
        public ICollection<KonverteringDto> Konverteringer { get; set; } = new List<KonverteringDto>();
    }
}
