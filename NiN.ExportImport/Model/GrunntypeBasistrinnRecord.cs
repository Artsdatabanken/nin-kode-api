namespace NiN.ExportImport.Model
{
    using CsvHelper.Configuration.Attributes;

    public class GrunntypeBasistrinnRecord
    {
        [Index(0)]
        public string GrunntypeKode { get; set; }

        [Index(1)]
        public string Basistrinn { get; set; }
    }
}