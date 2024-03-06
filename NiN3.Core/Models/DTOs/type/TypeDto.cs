using Newtonsoft.Json;
using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.DTOs.type
{
    [JsonObject("Type")]
    public class TypeDto
    {
        public string Navn { get; set; }
        public KodeDto Kode { get; set; } = new KodeDto();
        public string Kategori { get; set; }
        

        //public string Ecosystnivaa { get; set; }

        public EcosystnivaaEnum? EcosystnivaaEnum { get; set; }

        public string EcosystnivaaNavn { get; set; }

        //public string Typekategori { get; set; }
        public TypekategoriEnum? TypekategoriEnum { get; set; }
        public string TypekategoriNavn { get; set; }
        public Typekategori2Enum? Typekategori2Enum { get; set; }
        public string? Typekategori2Navn { get; set; }
        //public string? Typekategori2 { get; set; }
        //public string? Langkode { get; set; }
        public ICollection<HovedtypegruppeDto> Hovedtypegrupper { get; set; } = new List<HovedtypegruppeDto>();
    }
}
