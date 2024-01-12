using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvDataImporter_typeklasser_langkode
    {
        public string kode_type { get; set; }
        public string kode_hovedtypegruppe { get; set; }
        public string kode_hovedtype { get; set; }
        public string langkode { get; set; }
        internal static CsvDataImporter_typeklasser_langkode ParseRow(string row)
        {
            var columns = row.Split(';');
            
            return new CsvDataImporter_typeklasser_langkode()
            {
                langkode = columns[3],
                kode_type = columns[0],
                kode_hovedtypegruppe = columns[1],
                kode_hovedtype = columns[2]
            };
        }

        public static List<CsvDataImporter_typeklasser_langkode> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvDataImporter_typeklasser_langkode.ParseRow).ToList();
        }
    }
}
