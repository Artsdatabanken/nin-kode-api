namespace NiN.ExportImport
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Ionic.Zip;
    using Microsoft.EntityFrameworkCore;
    using NiN.Database;
    using NiN.Database.Models.Code;
    using NiN.ExportImport.Map;

    public class NinCodeExport
    {
        private readonly NiNDbContext _context;
        private readonly string _version;

        public NinCodeExport(NiNDbContext ninContext, string version)
        {
            _context = ninContext;
            _version = version;
        }

        public MemoryStream GenerateStream()
        {
            var streams = new Dictionary<string, MemoryStream>();
            var writers = new Dictionary<string, CsvWriter>();

            var csvConfiguration = new CsvConfiguration(new CultureInfo("nb-NO"));
            //var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            var streamNames = new[]
            {
                "grunntyper",
                "kartleggingsenheter",
                "miljovariabler"
            };

            foreach (var name in streamNames)
            {
                var memoryStream = new MemoryStream();
                var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8) { AutoFlush = true };
                var csvWriter = CreateCsvWriter(streamWriter, csvConfiguration);

                csvWriter.WriteHeader<Natursystem>();
                csvWriter.WriteHeader<Hovedtypegruppe>();
                csvWriter.WriteHeader<Hovedtype>();

                switch (name)
                {
                    case "grunntyper":
                        csvWriter.WriteHeader<Grunntype>();
                        break;
                    case "kartleggingsenheter":
                        csvWriter.WriteHeader<Kartleggingsenhet>();
                        break;
                    case "miljovariabler":
                        csvWriter.WriteHeader<Miljovariabel>();
                        csvWriter.WriteHeader<Trinn>();
                        csvWriter.WriteHeader<Basistrinn>();
                        break;
                }
                
                csvWriter.NextRecord();

                streams.Add(name, memoryStream);
                writers.Add(name, csvWriter);
            }

            var na = _context.Natursystem
                .Include(x => x.Version)
                .Include(x => x.Kode)
                .Include(x => x.UnderordnetKoder)
                .FirstOrDefault(x => x.Version.Navn == _version);

            if (na == null) return null;

            foreach (var hovedtypegruppe in na.UnderordnetKoder)
            {
                var htg = _context.Hovedtypegruppe
                    .Include(x => x.Kode)
                    .Include(x => x.UnderordnetKoder)
                    .FirstOrDefault(x => x.Id == hovedtypegruppe.Id);
                if (htg == null) continue;

                foreach (var hovedtype in htg.UnderordnetKoder)
                {
                    var ht = _context.Hovedtype
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .Include(x => x.Kartleggingsenheter)
                        .Include(x => x.Miljovariabler)
                        .FirstOrDefault(x => x.Id == hovedtype.Id);
                    if (ht == null) continue;

                    foreach (var grunntype in ht.UnderordnetKoder)
                    {
                        var gt = _context.Grunntype
                            .Include(x => x.Kode)
                            .FirstOrDefault(x => x.Id == grunntype.Id);
                        if (gt == null) continue;

                        writers["grunntyper"].WriteRecord(na);
                        writers["grunntyper"].WriteRecord(htg);
                        writers["grunntyper"].WriteRecord(ht);
                        writers["grunntyper"].WriteRecord(gt);
                        writers["grunntyper"].NextRecord();
                    }

                    foreach (var kartleggingsenhet in ht.Kartleggingsenheter)
                    {
                        var k = _context.Kartleggingsenhet
                            .Include(x => x.Kode)
                            .FirstOrDefault(x => x.Id == kartleggingsenhet.Id);
                        if (k == null) continue;

                        writers["kartleggingsenheter"].WriteRecord(na);
                        writers["kartleggingsenheter"].WriteRecord(htg);
                        writers["kartleggingsenheter"].WriteRecord(ht);
                        writers["kartleggingsenheter"].WriteRecord(k);
                        writers["kartleggingsenheter"].NextRecord();
                    }

                    foreach (var miljovariabel in ht.Miljovariabler)
                    {
                        var m = _context.Miljovariabel
                            .Include(x => x.Kode)
                            .Include(x => x.Trinn)
                            .FirstOrDefault(x => x.Id == miljovariabel.Id);
                        if (m == null) continue;

                        foreach (var trinn in miljovariabel.Trinn)
                        {
                            var t = _context.Trinn
                                .Include(x => x.Kode)
                                .Include(x => x.Basistrinn)
                                .FirstOrDefault(x => x.Id == trinn.Id);
                            if (t == null) continue;

                            foreach (var basistrinn in t.Basistrinn)
                            {
                                var bt = _context.Basistrinn
                                    //.Include(x => x.Kode)
                                    .FirstOrDefault(x => x.Id == basistrinn.Id);
                                if (bt == null) continue;

                                writers["miljovariabler"].WriteRecord(na);
                                writers["miljovariabler"].WriteRecord(htg);
                                writers["miljovariabler"].WriteRecord(ht);
                                writers["miljovariabler"].WriteRecord(m);
                                writers["miljovariabler"].WriteRecord(t);
                                writers["miljovariabler"].WriteRecord(bt);
                                writers["miljovariabler"].NextRecord();
                            }
                        }
                    }
                }
            }

            var zipFile = new ZipFile();
            var outputStream = new MemoryStream();

            foreach (var (key, stream) in streams)
            {
                stream.Seek(0, SeekOrigin.Begin);
                zipFile.AddEntry($"{key}_v{_version}.csv", stream);
            }

            zipFile.Save(outputStream);

            return outputStream;
        }

        #region private methods

        private static CsvWriter CreateCsvWriter(StreamWriter streamWriter, CsvConfiguration csvConfiguration)
        {
            var csvWriter = new CsvWriter(streamWriter, csvConfiguration);

            csvWriter.Context.RegisterClassMap<NatursystemMap>();
            csvWriter.Context.RegisterClassMap<HovedtypegruppeMap>();
            csvWriter.Context.RegisterClassMap<HovedtypeMap>();
            csvWriter.Context.RegisterClassMap<GrunntypeMap>();
            csvWriter.Context.RegisterClassMap<KartleggingsenhetMap>();
            csvWriter.Context.RegisterClassMap<MiljovariabelMap>();
            csvWriter.Context.RegisterClassMap<TrinnMap>();
            csvWriter.Context.RegisterClassMap<BasistrinnMap>();
            
            return csvWriter;
        }
        
        #endregion
    }
}
