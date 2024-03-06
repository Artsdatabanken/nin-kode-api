using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models
{
    public class Enumoppslag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        //public Guid Id { get; set; }
        [Required]
        public Versjon Versjon { get; set; }
        public int Ordinal{ get; set; }
        public string Verdi { get; set; }
        public string Beskrivelse { get; set; }
        public string Enumtype { get; set; }
    }
}
