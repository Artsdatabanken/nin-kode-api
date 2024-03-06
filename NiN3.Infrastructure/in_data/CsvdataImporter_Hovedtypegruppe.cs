using NiN3.Core.Models.Enums;

namespace NiN3KodeAPI.in_data
{
    public class CsvdataImporter_Hovedtypegruppe
    {
        public Typekategori2Enum? Typekategori2 { get; set; }   
        public string Hovedtypegruppe { get; set;}
        public string Hovedtypegruppenavn { get; set;}
        public Typekategori3Enum? Typekategori3 { get; set; }
        public string Kode { get; set; }
        internal static CsvdataImporter_Hovedtypegruppe ParseRow(string row)
        {
            var columns = row.Split(';');
            var csv_htg= new CsvdataImporter_Hovedtypegruppe()
            {
                Typekategori2 = EnumUtil.ParseEnum<Typekategori2Enum>(columns[0]),
                Hovedtypegruppe = columns[1],
                Hovedtypegruppenavn = char.ToUpper(columns[2][0]) + columns[2].Substring(1),
                Typekategori3 = EnumUtil.ParseEnum<Typekategori3Enum>(columns[3]),
                Kode = columns[4]
            };
            return csv_htg;
        }

        public static List<CsvdataImporter_Hovedtypegruppe> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Hovedtypegruppe.ParseRow).ToList();
        }
    }
}
