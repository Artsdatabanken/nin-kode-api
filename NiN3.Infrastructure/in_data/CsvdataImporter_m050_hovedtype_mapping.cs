namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_m050_hovedtype_mapping
    {
        public string m050kode { get; set; }
        public string Hovedtype_kode { get; set; }

        internal static CsvdataImporter_m050_hovedtype_mapping ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_m050_hovedtype_mapping()
            {
                m050kode = columns[0],
                Hovedtype_kode = columns[1]
            };
        }

        public static List<CsvdataImporter_m050_hovedtype_mapping> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_m050_hovedtype_mapping.ParseRow).ToList();
        }
    }
}
