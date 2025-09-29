using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class VariabelnavnMaaleskala
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public Variabelnavn Variabelnavn { get; set; }
        //public Variabeltrinn maaleskala { get; set; }
        public Maaleskala Maaleskala { get; set; }
    }
}
