using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_m005_hovedtype_mapping
    {
        public string m005kode { get; set; }
        public string Hovedtype_kode { get; set; }

        internal static CsvdataImporter_m005_hovedtype_mapping ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_m005_hovedtype_mapping()
            {
                m005kode = columns[0],
                Hovedtype_kode = columns[1]
            };
        }

        public static List<CsvdataImporter_m005_hovedtype_mapping> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_m005_hovedtype_mapping.ParseRow).ToList();
        }
    }
}
