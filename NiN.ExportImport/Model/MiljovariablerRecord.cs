namespace NiN.ExportImport.Model
{
    using CsvHelper.Configuration.Attributes;

    public class MiljovariablerRecord
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
        public string MiljovariabelType { get; set; }

        [Index(7)]
        public string MiljovariabelHovedtype { get; set; }

        [Index(8)]
        public string MiljovariabelKode { get; set; }

        [Index(9)]
        public string MiljovariabelNavn { get; set; }

        [Index(10)]
        public string TrinnKode { get; set; }

        [Index(11)]
        public string TrinnNavn { get; set; }

        [Index(12)]
        public string BasistrinnNavn { get; set; }
    }
}