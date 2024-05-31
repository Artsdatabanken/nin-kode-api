namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_m050
    {
        public string Kode { get; set; }
        public string Kortkode { get; set; }
        public string Navn { get; set; }

        internal static CsvdataImporter_m050 ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_m050()
            {
                Kode = columns[0],
                Navn = char.ToUpper(columns[1][0]) + columns[1].Substring(1),
                Kortkode = columns[2]
            };
        }

        public static List<CsvdataImporter_m050> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_m050.ParseRow).ToList();
        }
    }
}