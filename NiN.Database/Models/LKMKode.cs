namespace NiN.Database.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LKMKode
    {
        public LKMKode()
        {
            Basistrinn = new List<Basistrinn>();
        }

        [Key]
        public int Id { get; set; }
        public string KodeId { get; set; }
        public virtual ICollection<Basistrinn> Basistrinn { get; set; }
    }
}