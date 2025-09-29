using NiN3.Core.Models.Enums;

namespace NiN3.Core.Models.DTOs.variabel
{
    public class MaaleskalaDto
    {
        public string MaaleskalaNavn { get; set; }
        public MaaleskalatypeEnum? MaaleskalatypeEnum { get; set; }
        public string MaaleskalatypeNavn { get; set; }
        public EnhetEnum? EnhetEnum { get; set; }
        public string EnhetNavn { get; set; }

        public IList<TrinnDto> Trinn { get; set; } = new List<TrinnDto>();
    }
}
