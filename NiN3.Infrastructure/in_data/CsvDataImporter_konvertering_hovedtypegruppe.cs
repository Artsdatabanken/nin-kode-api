using NiN3.Core.Models.Enums;

namespace NiN3.Infrastructure.in_data
{
    public class CsvDataImporter_konvertering_hovedtypegruppe
    {
        KlasseEnum Klasse { get; set; }
        public String Kode { get; set; }
        public String ForrigeKode { get; set; }

        public int? FoelsomhetsPresisjon { get; set; }
        public int? Spesifiseringsevne { get; set; }
        public string ForrigeVersjon { get; set; }

        public string Versjon { get; set; }
        public String? Url { get; set; }

        internal static CsvDataImporter_konvertering_hovedtypegruppe ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvDataImporter_konvertering_hovedtypegruppe()
            {
                Klasse = KlasseEnum.HTG,
                Kode = columns[0],
                ForrigeKode = columns[1],
                FoelsomhetsPresisjon = columns[2]!=""? int.Parse(columns[2]):null,
                Spesifiseringsevne = columns[3] != "" ? int.Parse(columns[3]):null,
                ForrigeVersjon = "2.3",
                Versjon = "3.0",
                Url = columns[5]
            };
        }
        public static List<CsvDataImporter_konvertering_hovedtypegruppe> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvDataImporter_konvertering_hovedtypegruppe.ParseRow).ToList();
        }
    }
}
