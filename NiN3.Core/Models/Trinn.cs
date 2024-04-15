using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models
{
    public class Trinn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string? Langkode { get; set; }
        public string Beskrivelse { get; set; }
        public string Verdi { get; set; }

        public Maaleskala Maaleskala { get; set; }
    }
}
