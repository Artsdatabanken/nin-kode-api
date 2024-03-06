using NiN3.Core.Models.DTOs.type;
using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
