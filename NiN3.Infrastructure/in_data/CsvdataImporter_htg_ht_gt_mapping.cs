﻿namespace NiN3.Infrastructure.in_data
{
    public class CsvdataImporter_htg_ht_gt_mapping
    {
        public string Hovedtypegruppe_kode { get; set; }
        public string Hovedtype_kode { get; set; }
        public string Grunntype_kode { get; set; }

        internal static CsvdataImporter_htg_ht_gt_mapping ParseRow(string row)
        {
            var columns = row.Split(';');
            return new CsvdataImporter_htg_ht_gt_mapping()
            {
                Hovedtypegruppe_kode = columns[0],
                Hovedtype_kode = columns[1],
                Grunntype_kode = columns[2]
            };
        }

        public static List<CsvdataImporter_htg_ht_gt_mapping> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(CsvdataImporter_htg_ht_gt_mapping.ParseRow).ToList();
        }
    }
}
