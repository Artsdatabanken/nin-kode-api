using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class GrunntypeVariabeltrinn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public Variabelnavn? Variabelnavn { get; set; }
        public Grunntype Grunntype { get; set; }
        public Maaleskala Maaleskala { get; set; }
        public Trinn? Trinn { get; set; }
    }
}