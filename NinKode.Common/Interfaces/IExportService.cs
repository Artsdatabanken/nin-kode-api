namespace NinKode.Common.Interfaces
{
    using System.IO;

    public interface IExportService
    {
        Stream ExportToCsv(string version);
    }
}