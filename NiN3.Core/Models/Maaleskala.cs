using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NiN3.Core.Models.Enums;

namespace NiN3.Core.Models
{
    public class Maaleskala
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string MaaleskalaNavn { get; set; }
        public MaaleskalatypeEnum? MaaleskalatypeEnum { get; set; }
        public EnhetEnum? EnhetEnum { get; set; }
        public ICollection<Trinn> Trinn { get; set; } = new List<Trinn>();
    }
}
