namespace NiN.Console
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using NiN.Database;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;

    public static class NinLoader_deprecated
    {
        private static Stopwatch _stopwatch = new();
        
        public static void RemoveAll(string version)
        {
            using (var context = new NiNContext())
            {
                //if (context.Database.EnsureCreated())
                //{
                //    Console.WriteLine($"Created empty database: {context.DbName}");
                //    return;
                //}

                var totalCount = 0;

                var natursystem = context.Natursystem
                    .Include(x => x.Version)
                    .FirstOrDefault(x => x.Version.Navn.Equals(version));

                if (natursystem == null) return;

                var ninVersion = natursystem.Version;

                natursystem = context.Natursystem
                    .Include(x => x.Kode)
                    .Include(x => x.UnderordnetKoder)
                    .FirstOrDefault(x => x.Id == natursystem.Id);

                foreach (var hovedtypegruppe in natursystem.UnderordnetKoder)
                {
                    var ht = context.Hovedtypegruppe
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Id == hovedtypegruppe.Id);

                    if (ht == null) continue;

                    foreach (var hovedtype in ht.UnderordnetKoder)
                    {
                        var h = context.Hovedtype
                            .Include(x => x.Kode)
                            .Include(x => x.Kartleggingsenheter)
                            .Include(x => x.Miljovariabler)
                            .Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Id == hovedtype.Id);

                        if (h == null) continue;

                        foreach (var miljovariabel in h.Miljovariabler)
                        {
                            var m = context.Miljovariabel
                                .Include(x => x.Kode)
                                .Include(x => x.Trinn)
                                .FirstOrDefault(x => x.Id == miljovariabel.Id);

                            if (m == null) continue;

                            foreach (var trinn in m.Trinn)
                            {
                                var t = context.Trinn
                                    .Include(x => x.Kode)
                                    .Include(x => x.Basistrinn)
                                    .FirstOrDefault(x => x.Id == trinn.Id);

                                if (t == null) continue;

                                totalCount += t.Basistrinn.Count;

                                context.Kode.Remove(t.Kode);

                                totalCount++;
                                context.Trinn.Remove(t);
                            }

                            totalCount++;
                            context.LKMKode.Remove(m.Kode);

                            totalCount++;
                            context.Miljovariabel.Remove(m);
                        }

                        totalCount += h.Kartleggingsenheter.Count;
                        totalCount += h.Miljovariabler.Count;
                        totalCount += h.UnderordnetKoder.Count;

                        totalCount++;
                        context.Hovedtype.Remove(h);
                    }

                    context.Hovedtypegruppe.Remove(hovedtypegruppe);
                }

                //totalCount += context.Hovedtypegruppe.Count();
                //context.Hovedtypegruppe.RemoveRange(context.Hovedtypegruppe);

                //totalCount += context.Natursystem.Count();
                context.Natursystem.Remove(natursystem);

                context.NinVersion.Remove(ninVersion);

                _stopwatch.Stop();
                Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
                _stopwatch.Reset();
                _stopwatch.Start();
                context.SaveChanges();
                Console.WriteLine($"Removed {totalCount} items");
                _stopwatch.Stop();
                Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

                _stopwatch.Start();

                context.SaveChanges();

                ResetCounters(context);
            }

            _stopwatch.Stop();
            Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

            _stopwatch.Start();
        }

        #region private methods

        private static void ResetCounters(NiNContext context)
        {
            if (!context.Database.ProviderName.Equals("Microsoft.EntityFrameworkCore.SqlServer")) return;

            return;

            var n_id = context.Database.ExecuteSqlRaw($"select max(id) from [{context.Model.FindEntityType(typeof(Natursystem)).GetTableName()}]");

            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Natursystem)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Hovedtypegruppe)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Hovedtype)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Grunntype)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Kartleggingsenhet)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Miljovariabel)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(NinKode)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(LKMKode)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Trinn)).GetTableName()}]', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{context.Model.FindEntityType(typeof(Basistrinn)).GetTableName()}]', RESEED, 0)");
        }

        #endregion
    }
}
