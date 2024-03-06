using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models
{
    /* Transient class to hold variabelnavn and måleskala for type -classes on mapping to DTOs */
    public class Variabeltrinn
    {
        public Variabelnavn? Variabelnavn { get; set; }
        public Maaleskala Maaleskala { get; set; }
    }
}
