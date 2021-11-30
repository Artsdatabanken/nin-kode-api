namespace NiN.Database.Models.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BaseIdEntity
    {
        [Key]
        public Guid Id { get; set; }

        public NinVersion Version { get; set; }
    }
}
