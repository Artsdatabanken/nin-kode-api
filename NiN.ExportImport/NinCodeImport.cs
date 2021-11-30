namespace NiN.ExportImport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.Configuration.Attributes;
    using NiN.Database;

    public class NinCodeImport
    {
        private readonly NiNDbContext _context;
        private readonly string _version;

        public NinCodeImport(NiNDbContext ninContext, string version)
        {
            _context = ninContext;
            _version = version;
        }

        public IEnumerable<KartleggingRecord> FixKartleggingConnections(string path)
        {
            var csvConfiguration = new CsvConfiguration(new CultureInfo("nb-NO"));

            if (!File.Exists(path)) yield break;

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, csvConfiguration);

            var records = csv.GetRecords<KartleggingRecord>();
            foreach (var record in records)
            {
                yield return record;
            }
        }
    }

    public class KartleggingRecord
    {
        [Index(0)]
        public int Malestokk { get; set; }
        
        [Index(1)]
        public string SammensattKode { get; set; }
        
        [Index(2)]
        public string Name { get; set; }

        [Index(3)]
        public string Grunntypenummer { get; set; }

        [Index(4)]
        public string Grunntypekoder { get; set; }
    }
}
