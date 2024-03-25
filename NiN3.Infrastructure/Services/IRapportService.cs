using NiN3.Core.Models.DTOs.rapport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.Services
{
    public interface IRapportService
    {
        public List<KodeoversiktDto> GetKodeSummary(string versjon);
        public string MakeKodeoversiktCSV(string versjon, string separator=";");
        public byte[] MakeKodeoversiktXlsx(string versjon);
    }
}
