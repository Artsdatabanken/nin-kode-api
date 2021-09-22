namespace NiN.Database.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Navn { get; set; }
    }
}
