using NiN3.Core.Models.Enums;

namespace NiN3.Core.Models.DTOs.search
{
    public class SearchResultDto
    {
        public string Kode { get; set; }
        public string Langkode { get; set; }
        public string Navn { get; set; }
        public KlasseEnum KlasseEnum { get; set; }
        public string KlasseNavn { get; set; }
    }
}
