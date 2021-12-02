namespace NiN.ExportImport.Model
{
    using CsvHelper.Configuration.Attributes;

    public class LandformRecord
    {
        [Index(0)]
        public int Besys { get; set; }

        [Index(1)]
        public string BesysNavn { get; set; }

        [Index(2)]
        public string Nivaa1Kode { get; set; }

        [Index(3)]
        public string Nivaa1Navn { get; set; }

        [Index(4)]
        public string Nivaa2Kode { get; set; }

        [Index(5)]
        public string SammensattKode { get; set; }

        [Index(6)]
        public string Navn { get; set; }

        [Index(7)]
        public string Variabeltype { get; set; }

        [Index(8)]
        public string RomligSkala { get; set; }

        [Index(9)]
        public string Tags { get; set; }
    }
}