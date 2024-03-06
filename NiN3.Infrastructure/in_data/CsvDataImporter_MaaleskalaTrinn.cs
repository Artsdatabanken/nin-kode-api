using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvDataImporter_MaaleskalaTrinn
    {

        //public string VNKortkode { get; set; }
        //public MaaleskalatypeEnum MaaleskalatypeEnum { get; set; }
        public string Maaleskalanavn { get; set; }
        public string Trinnverdi { get; set; }
        public string Trinn { get; set; }
        internal static CsvDataImporter_MaaleskalaTrinn ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvDataImporter_MaaleskalaTrinn()
            {
                //VNKortkode = columns[0],
                //MaaleskalatypeEnum = EnumUtil.ParseEnum<MaaleskalatypeEnum>(columns[1]),
                Maaleskalanavn = columns[2],
                Trinnverdi = columns[1],
                Trinn = columns[0]
            };
        }

        public static List<CsvDataImporter_MaaleskalaTrinn> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvDataImporter_MaaleskalaTrinn.ParseRow).ToList();
        }
    }
}
