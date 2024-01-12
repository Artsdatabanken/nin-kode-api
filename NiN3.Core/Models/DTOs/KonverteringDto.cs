using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.DTOs
{
    public class KonverteringDto
    {

        public string Kode { get; set; } //Code of current version
        public string ForrigeKode { get; set; } //Code of previous version
        public KlasseEnum Klasse { get; set; } //Class of current version
        public string KlasseNavn { get; set; }
        public string ForrigeVersjon { get; set; } //Previous version
        public int? FoelsomhetsPresisjon { get; set; } //Sensitivity precision
        public int? Spesifiseringsevne { get; set; } //Specificity
        public string? Url { get; set; } //Url to the previous version 
    }
}
