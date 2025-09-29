using NiN3.Core.Models.Enums;

namespace NiN3KodeAPI.in_data
{
    public class CsvdataImporter_Type
    {
        public EcosystnivaaEnum? Ecosystnivaa { get; set; }
        public TypekategoriEnum? Typekategori { get; set; }
        public Typekategori2Enum? Typekategori2 { get; set; }
        public string Kode { get; set; }
        public string Langkode { get; set; }

        internal static CsvdataImporter_Type ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_Type()
            {
                Ecosystnivaa = EnumUtil.ParseEnum<EcosystnivaaEnum>(columns[0]),
                Typekategori = EnumUtil.ParseEnum <TypekategoriEnum>(columns[1]),
                Typekategori2 = EnumUtil.ParseEnum <Typekategori2Enum>(columns[2]),
                Kode = columns[3]
            };
        }


        public static List<CsvdataImporter_Type> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Type.ParseRow).ToList();
        }
    }
}
