using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_m005
    {
        public string Kode { get; set; }

        public string Kortkode { get; set; }
        public string Navn { get; set; }

        internal static CsvdataImporter_m005 ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_m005()
            {
                Kode = columns[0],
                Navn = char.ToUpper(columns[1][0]) + columns[1].Substring(1),
                Kortkode = columns[2]
            };
        }

        public static List<CsvdataImporter_m005> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_m005.ParseRow).ToList();
        }
    }
}
