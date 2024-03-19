using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.DTOs.type
{
    public class KartleggingsenhetDto
    {
        public string Kategori { get; set; } = "Kartleggingsenhet";
        //public MaalestokkEnum Maalestokk { get; set; }
        //public string Maalestokk { get; set; }
        public MaalestokkEnum MaalestokkEnum { get; set; }
        public string MaalestokkNavn { get; set; }
        public string? Navn { get; set; }
        public KodeDto Kode { get; set; }
        //public string Kode { get; set; }
        //public string Kortkode { get; set; }
        public ICollection<GrunntypeDto> Grunntyper { get; set; } = new List<GrunntypeDto>();
    }
}
