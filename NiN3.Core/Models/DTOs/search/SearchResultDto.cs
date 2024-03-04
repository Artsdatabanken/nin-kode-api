using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
