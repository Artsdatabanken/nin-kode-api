namespace NinKode.Database.Services
{
    using System.IO;
    using NiN.Database;
    using NiN.Export;
    using NinKode.Common.Interfaces;

    public class ExportService : IExportService
    {
        public Stream ExportToCsv(NiNContext context, string version)
        {
            var ninCodeExport = new NinCodeExport(context, version);
            var stream = ninCodeExport.GenerateStream();
            if (stream == null) return null;

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        #region private methods

        #endregion
    }
}
