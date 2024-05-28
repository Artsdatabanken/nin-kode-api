using NiN3.Core.Models.DTOs.type;
using NiN3.Core.Models.DTOs.variabel;

namespace NiN3.Core.Models.DTOs
{
    public class VersjonDto
    {
        public string Navn { get; set; }
        public ICollection<TypeDto> Typer { get; set; } = new List<TypeDto>();
        public ICollection<VariabelDto> Variabler { get; set; } = new List<VariabelDto>();
    }
}
