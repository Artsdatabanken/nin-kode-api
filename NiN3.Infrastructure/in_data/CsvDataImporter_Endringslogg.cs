using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvDataImporter_Endringslogg
    {
        public string tidspunkt { get; set; }
        public string beskrivelse { get; set; }
        public string versjon { get; set; }

        internal static CsvDataImporter_Endringslogg ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvDataImporter_Endringslogg()
            {
                tidspunkt = columns[1],
                beskrivelse = columns[2],
                versjon= columns[0]
            };
        }
        public static List<CsvDataImporter_Endringslogg> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvDataImporter_Endringslogg.ParseRow)
                .OrderBy(x => x.tidspunkt)
                .ThenBy(x => x.beskrivelse)
                .ThenBy(x => x.versjon)
                .ToList();
        }
    }
}
