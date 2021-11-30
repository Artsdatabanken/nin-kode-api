namespace NiN.ExportImport.Model
{
    using CsvHelper.Configuration.Attributes;

    public class GrunntypeKartleggingRecord
    {
        [Index(0)]
        public int Malestokk { get; set; }
        
        [Index(1)]
        public string SammensattKode { get; set; }
        
        [Index(2)]
        public string Name { get; set; }

        [Index(3)]
        public string Grunntypenummer { get; set; }

        [Index(4)]
        public string Grunntypekoder { get; set; }
    }
}