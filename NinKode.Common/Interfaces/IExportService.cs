namespace NinKode.Common.Interfaces
{
    using System.IO;
    using NiN.Database;

    public interface IExportService
    {
        Stream ExportToCsv(NiNDbContext context, string version);
    }
}