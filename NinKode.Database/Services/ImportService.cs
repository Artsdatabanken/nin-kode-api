namespace NinKode.Database.Services
{
    using System.IO;
    using NiN.Database;
    using NinKode.Common.Interfaces;

    public class ImportService : IImportService
    {
        public bool ImportFromCsv(Stream stream, NiNDbContext dbContext, string version)
        {
            throw new System.NotImplementedException();
        }
    }
}