using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class HovedtypeVariabeltrinn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public Variabelnavn? Variabelnavn { get; set; }
        public Hovedtype Hovedtype { get; set; }
        public Maaleskala Maaleskala { get; set; }
        public Trinn? Trinn { get; set; } 
    }
}
