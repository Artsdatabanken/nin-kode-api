﻿namespace NinKode.Database.Services
{
    using System.IO;
    using NiN.Database;
    using NiN.ExportImport;
    using NinKode.Common.Interfaces;

    public class ExportService : IExportService
    {
        public Stream ExportToCsv(NiNDbContext dbContext, string version)
        {
            var ninCodeExport = new NinCodeExport(dbContext, version);
            var stream = ninCodeExport.GenerateStream();
            if (stream == null) return null;

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
