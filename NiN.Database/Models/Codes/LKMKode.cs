﻿namespace NiN.Database.Models.Codes
{
    using System.ComponentModel.DataAnnotations;
    using NiN.Database.Models.Enums;

    public class LKMKode
    {
        public LKMKode() { }

        [Key]
        public int Id { get; set; }

        [StringLength(25)]
        public string Kode { get; set; }

        public LkmKategoriEnum LkmKategori { get; set; }
    }
}