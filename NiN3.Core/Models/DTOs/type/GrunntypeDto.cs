using NiN3.Core.Models.DTOs.variabel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.DTOs.type
{
    public class GrunntypeDto
    {
        public string Navn { get; set; }
        public string Kategori { get; set; } = "Grunntype";
        public KodeDto Kode { get; set; }
        public ICollection<VariabeltrinnDto> Variabeltrinn { get; set; } = new List<VariabeltrinnDto>();
        public ICollection<KonverteringDto> Konverteringer { get; set; } = new List<KonverteringDto>();
    }
}
