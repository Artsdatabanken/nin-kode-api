namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_Variabelnavn_maaleskala
    {
        public string VaribelnavnKode { get; set; }
        public string Maaleskalanavn { get; set; }
        internal static CsvdataImporter_Variabelnavn_maaleskala ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_Variabelnavn_maaleskala()
            {
                VaribelnavnKode = columns[0],
                Maaleskalanavn = columns[1]

            };
        }
        public static List<CsvdataImporter_Variabelnavn_maaleskala> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Variabelnavn_maaleskala.ParseRow).ToList();
        }
    }
}
