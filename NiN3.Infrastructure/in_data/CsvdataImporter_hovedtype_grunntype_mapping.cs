using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_hovedtype_grunntype_mapping
    {
        public string Hovedtype_kode { get; set; }
        public string Grunntype_kode { get; set; }

        internal static CsvdataImporter_hovedtype_grunntype_mapping ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_hovedtype_grunntype_mapping()
            {
                Hovedtype_kode = columns[0],
                Grunntype_kode = columns[1]
            };
        }

        public static List<CsvdataImporter_hovedtype_grunntype_mapping> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_hovedtype_grunntype_mapping.ParseRow).ToList();
        }
    }
}
