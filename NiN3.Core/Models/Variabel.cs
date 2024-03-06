using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models
{
    [Index(nameof(Kode), IsUnique = false)]
    public class Variabel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // added attribute to auto-generate Id
        public int Id { get; set; }
        public string Kode { get; set; }
        public string? Langkode { get; set; }
        public EcosystnivaaEnum? Ecosystnivaa { get; set; }
        public VariabelkategoriEnum? Variabelkategori { get; set; }
        public String Navn { get; set; }
        public Versjon Versjon { get; set; }
        public ICollection<Variabelnavn> Variabelnavn { get; set; } = new List<Variabelnavn>();
    }
}