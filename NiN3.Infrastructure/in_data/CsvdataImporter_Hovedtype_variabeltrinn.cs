using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_Hovedtype_variabeltrinn
    {
        public string hovedtype_kode { get; set; }
        public string varkode2 { get; set; }
        public string trinn { get; set; }
        public string? variabelnavnKode { get; set; }
    
        internal static CsvdataImporter_Hovedtype_variabeltrinn ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_Hovedtype_variabeltrinn()
            {
                hovedtype_kode = columns[0],
                varkode2 = columns[1],
                trinn = columns[2],
                variabelnavnKode = columns[3] != "" ? columns[3] : null
            };
        }

        public static List<CsvdataImporter_Hovedtype_variabeltrinn> ProcessCSV(string path) {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Hovedtype_variabeltrinn.ParseRow)
                .OrderBy(x => x.trinn)
                    .ThenBy(x => x.varkode2)
                    .ThenBy(x => x.hovedtype_kode)
                    .ToList();
        }
    
    }
}


