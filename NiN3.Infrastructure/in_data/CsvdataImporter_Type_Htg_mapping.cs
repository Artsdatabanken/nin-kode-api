using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_Type_Htg_mapping
    {
        public string Type_kode { get; set; }
        public string Hovedtypegruppe_kode { get; set; }
        //public string Typekategori2 { get; set; }

        internal static CsvdataImporter_Type_Htg_mapping ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_Type_Htg_mapping()
            {
                Type_kode = columns[0],
                Hovedtypegruppe_kode = columns[1]
            };
        }

        public static List<CsvdataImporter_Type_Htg_mapping> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Type_Htg_mapping.ParseRow).ToList();
        }
    }
}
