namespace NiN.ExportImport.Model
{
    using CsvHelper.Configuration.Attributes;

    public class GrunntyperRecord
    {
        [Index(0)]
        public string NatursystemNavn { get; set; }

        [Index(1)]
        public string NatursystemKode { get; set; }

        [Index(2)]
        public string HovedtypegruppeNavn { get; set; }

        [Index(3)]
        public string HovedtypegruppeKode { get; set; }

        [Index(4)]
        public string HovedtypeNavn { get; set; }

        [Index(5)]
        public string HovedtypeKode { get; set; }

        [Index(6)]
        public string GrunntypeNavn { get; set; }

        [Index(7)]
        public string GrunntypeKode { get; set; }
    }
}