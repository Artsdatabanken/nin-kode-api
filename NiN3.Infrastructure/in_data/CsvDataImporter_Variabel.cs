using NiN3.Core.Models.Enums;

namespace NiN3.Infrastructure.in_data
{
    public class CsvDataImporter_Variabel
    {
        public String Kode { get; set; }
        public EcosystnivaaEnum? Ecosystnivaa { get; set; }
        public VariabelkategoriEnum? Variabelkategori { get; set; }

        internal static CsvDataImporter_Variabel ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvDataImporter_Variabel()
            {
                Kode = columns[2],
                Ecosystnivaa = EnumUtil.ParseEnum<EcosystnivaaEnum>(columns[0]),
                Variabelkategori = EnumUtil.ParseEnum<VariabelkategoriEnum>(columns[1])

            };
        }
        public static List<CsvDataImporter_Variabel> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvDataImporter_Variabel.ParseRow).ToList();
        }
    }
}
