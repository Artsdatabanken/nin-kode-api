﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.DTOs
{
    public class KodeDto
    {
        public string Id { get; set; }
        public string? Definisjon { get; set; }//URL to the definition, "/hentKode" - endpoint
        public string? Langkode { get; set; }
    }
}
