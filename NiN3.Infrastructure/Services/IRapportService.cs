using NiN3.Core.Models.DTOs.rapport;

namespace NiN3.Infrastructure.Services
{
    public interface IRapportService
    {
        public List<KodeoversiktDto> GetKodeSummary(string versjon);
        public string MakeKodeoversiktCSV(string versjon, string separator=";");
        public byte[] MakeKodeoversiktXlsx(string versjon);
    }
}
