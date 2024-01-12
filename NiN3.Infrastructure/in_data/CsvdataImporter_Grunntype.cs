using NiN3.Core.Models.Enums;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_Grunntype
    {
       //public string Hovedtypegruppe { get; set; }
        public ProsedyrekategoriEnum? Prosedyrekategori { get; set; }

        public string Hovedtype { get; set; }
        public string Grunntype { get; set; }
        public string Grunntypenavn { get; set; }

        public string Kode { get; set; }
        public string Langkode { get; set; }

        internal static CsvdataImporter_Grunntype ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_Grunntype()
            {
                Langkode = columns[0],
                Hovedtype = columns[3],
                Prosedyrekategori = EnumUtil.ParseEnum<ProsedyrekategoriEnum>(columns[2]),
                //Hovedtypegruppe = columns[0],
                Grunntype = columns[4],
                Grunntypenavn = char.ToUpper(columns[5][0]) + columns[5].Substring(1),
                Kode = columns[6]
            };
        }

        public static List<CsvdataImporter_Grunntype> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Grunntype.ParseRow).ToList();
        }
    }
}
