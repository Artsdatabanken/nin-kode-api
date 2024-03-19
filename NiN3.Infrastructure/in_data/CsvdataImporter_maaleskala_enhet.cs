using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_maaleskala_enhet
    {
        public MaaleskalatypeEnum? MaaleskalatypeEnum { get; set; }
        public EnhetEnum? EnhetEnum { get; set; }    
        public string Maaleskalanavn { get; set; }

        public static CsvdataImporter_maaleskala_enhet ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_maaleskala_enhet()
            {
                MaaleskalatypeEnum = EnumUtil.ParseEnum<MaaleskalatypeEnum>(columns[0]),
                EnhetEnum = EnumUtil.ParseEnum<EnhetEnum>(columns[1]),
                Maaleskalanavn = columns[2],
            };
        }
        public static List<CsvdataImporter_maaleskala_enhet> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_maaleskala_enhet.ParseRow).ToList();
        }
    }
}
