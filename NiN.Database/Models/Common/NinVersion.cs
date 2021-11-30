﻿namespace NiN.Database.Models.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NinVersion
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(255)]
        public string Navn { get; set; }
    }
}
