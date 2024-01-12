using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.DTOs.type
{
    public class HovedtypeDto
    {
        public string Navn { get; set; }
        public string Kategori { get; set; } = "Hovedtype";
        public KodeDto Kode { get; set; }

        //public string Prosedyrekategori { get; set; }
        public ProsedyrekategoriEnum? ProsedyrekategoriEnum { get; set; }
        public string ProsedyrekategoriNavn { get; set; }
        public ICollection<VariabeltrinnDto> Variabeltrinn { get; set; } = new List<VariabeltrinnDto>();
        public ICollection<GrunntypeDto> Grunntyper { get; set; } = new List<GrunntypeDto>();
        public ICollection<KartleggingsenhetDto> Kartleggingsenheter { get; set; } = new List<KartleggingsenhetDto>();
        public ICollection<KonverteringDto> Konverteringer { get; set; } = new List<KonverteringDto>();
        
    }
}
