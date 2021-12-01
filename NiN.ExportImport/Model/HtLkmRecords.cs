namespace NiN.ExportImport.Model
{
    using CsvHelper.Configuration.Attributes;

    public class HtLkmRecords
    {
        // HTK
        // Navn
        // PrK
        // PrK-tekst
        // GrL
        // Definisjonsgrunnlag-tekst
        // dLKM
        // hLKM
        // tLKM
        // uLKM
        // Kunnskapsgrunnlag - Hovedtypen generelt
        // Kunnskapsgrunnlag - Grunntypeinndelingen

        [Index(0)]
        public string HovedtypeKode { get; set; }

        [Index(1)]
        public string HovedtypeNavn { get; set; }

        [Index(2)]
        public string Prk { get; set; }

        [Index(3)]
        public string PrkTekst { get; set; }

        [Index(4)]
        public string GrL { get; set; }

        [Index(5)]
        public string DefinisjonsgrunnlagTekst { get; set; }

        [Index(6)]
        public string dLKM { get; set; }

        [Index(7)]
        public string hLKM { get; set; }

        [Index(8)]
        public string tLKM { get; set; }

        [Index(9)]
        public string uLKM { get; set; }

        [Index(10)]
        public string KunnskapsgrunnlagHovedtypenGenerelt{ get; set; }

        [Index(11)]
        public string KunnskapsgrunnlagGrunntypeinndelingen { get; set; }
    }
}