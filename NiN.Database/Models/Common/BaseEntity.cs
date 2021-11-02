namespace NiN.Database.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public class BaseEntity : BaseIdEntity
    {
        [StringLength(255)]
        public string Navn { get; set; }
    }
}
