﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models
{
    public class Hovedtype_Kartleggingsenhet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        //public Guid Id { get; set; }
        public Versjon Versjon { get; set; }

        public Kartleggingsenhet Kartleggingsenhet { get; set; }
        public Hovedtype Hovedtype { get; set; }
    }
}
