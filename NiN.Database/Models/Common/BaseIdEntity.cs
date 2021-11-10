namespace NiN.Database.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public class BaseIdEntity
    {
        [Key]
        public int Id { get; set; }

        public NinVersion Version { get; set; }
    }
}
