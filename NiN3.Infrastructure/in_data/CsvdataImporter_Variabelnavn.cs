using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_Variabelnavn
    {
        public String Kode { get; set; }

        public String Langkode { get; set; }
        public Variabelkategori2Enum? Variabelkategori2 { get; set; }
        public VariabeltypeEnum? Variabeltype { get; set; }
        public VariabelgruppeEnum? Variabelgruppe { get; set; }
        public string Delkode { get; set; }
        public string VariabelKode { get; set; }
        public String Navn { get; set; }

        internal static CsvdataImporter_Variabelnavn ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_Variabelnavn()
            {
                Langkode = columns[0],
                Kode = columns[1],
                Variabelkategori2 = EnumUtil.ParseEnum<Variabelkategori2Enum>(columns[4]),
                Variabelgruppe = EnumUtil.ParseEnum<VariabelgruppeEnum>(columns[6]),
                Variabeltype = EnumUtil.ParseEnum<VariabeltypeEnum>(columns[5]),
                Delkode = columns[7],
                Navn = char.ToUpper(columns[8][0]) + columns[8].Substring(1),
                VariabelKode = columns[9]

            };
        }
        public static List<CsvdataImporter_Variabelnavn> ProcessCSV(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_Variabelnavn.ParseRow).ToList();
        }
    }
}
