using NiN3.Core.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiN3.Core.Models
{
    public class Hovedtypegruppe_Hovedoekosystem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public Hovedtypegruppe Hovedtypegruppe { get; set; }
        public HovedoekosystemEnum? HovedoekosystemEnum { get; set; }
    }
}