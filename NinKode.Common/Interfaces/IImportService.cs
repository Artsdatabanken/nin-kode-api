namespace NinKode.Common.Interfaces
{
    using System.IO;
    using NiN.Database;

    public interface IImportService
    {
        bool ImportFromCsv(Stream stream, NiNDbContext dbContext, string version);
    }
}