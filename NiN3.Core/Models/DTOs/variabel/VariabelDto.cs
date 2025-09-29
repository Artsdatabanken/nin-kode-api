using NiN3.Core.Models.Enums;

namespace NiN3.Core.Models.DTOs.variabel
{
    public class VariabelDto
    {
        public string Navn { get; set; }
        public KodeDto Kode { get; set; } = new KodeDto();       
        public string Kategori { get; set; }
        public EcosystnivaaEnum? EcosystnivaaEnum { get; set; }
        public string EcosystnivaaNavn { get; set; }
        public VariabelkategoriEnum? VariabelkategoriEnum { get; set; }
        public string VariabelkategoriNavn { get; set; }

        public ICollection<VariabelnavnDto> Variabelnavn { get; set; } = new List<VariabelnavnDto>();

    }
}