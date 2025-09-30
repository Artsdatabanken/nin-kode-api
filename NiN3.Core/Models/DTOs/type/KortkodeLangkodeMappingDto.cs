using NiN3.Core.Models.Enums;

namespace NiN3.Core.Models.DTOs.type
{
    public class KortkodeLangkodeMappingDto
    {
        public required string Kortkode { get; set; }
        public required string Langkode { get; set; }
        public string? Navn { get; set; }
    }

    public class KortkodeLangkodeResponseDto
    {
        public List<KortkodeLangkodeMappingDto> Mappings { get; set; } = new List<KortkodeLangkodeMappingDto>();
        public List<string> IkkeFunnet { get; set; } = new List<string>();
    }
}