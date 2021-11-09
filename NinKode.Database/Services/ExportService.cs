namespace NinKode.Database.Services
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NiN.Export;
    using NinKode.Common.Interfaces;

    public class ExportService : IExportService
    {
        private const string DbConnString = "NiNConnectionString";

        private readonly NiNContext _context;

        public ExportService(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue(DbConnString, "");
            _context = string.IsNullOrEmpty(connectionString) ? new NiNContext() : new NiNContext(connectionString);
        }

        public Stream ExportToCsv(string version)
        {
            var ninCodeExport = new NinCodeExport(_context, version);
            var stream = ninCodeExport.GenerateStream();
            if (stream == null) return null;

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        #region private methods

        #endregion
    }
}
