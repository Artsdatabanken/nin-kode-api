using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models
{
    public class AlleKortkoder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string Kortkode { get; set; }
        public string? KortkodeV2 { get; set; }
        public KlasseEnum TypeKlasseEnum { get; set; }
        public Versjon Versjon { get; set; }
    }
}
