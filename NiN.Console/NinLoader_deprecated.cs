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
        
        public static void RemoveAll(NiNDbContext dbContext, string version)
        {
            //if (dbContext.Database.EnsureCreated())
            //{
            //    Console.WriteLine($"Created empty database: {dbContext.DbName}");
            //    return;
            //}

            var totalCount = 0;

            var natursystem = dbContext.Natursystem
                .Include(x => x.Version)
                .FirstOrDefault(x => x.Version.Navn.Equals(version));

            if (natursystem == null) return;

            var ninVersion = natursystem.Version;

            natursystem = dbContext.Natursystem
                .Include(x => x.Kode)
                .Include(x => x.UnderordnetKoder)
                .FirstOrDefault(x => x.Id == natursystem.Id);

            foreach (var hovedtypegruppe in natursystem.UnderordnetKoder)
            {
                var ht = dbContext.Hovedtypegruppe
                    .Include(x => x.Kode)
                    .Include(x => x.UnderordnetKoder)
                    .FirstOrDefault(x => x.Id == hovedtypegruppe.Id);

                if (ht == null) continue;

                foreach (var hovedtype in ht.UnderordnetKoder)
                {
                    var h = dbContext.Hovedtype
                        .Include(x => x.Kode)
                        .Include(x => x.Kartleggingsenheter)
                        .Include(x => x.Miljovariabler)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Id == hovedtype.Id);

                    if (h == null) continue;

                    foreach (var miljovariabel in h.Miljovariabler)
                    {
                        var m = dbContext.Miljovariabel
                            .Include(x => x.Kode)
                            .Include(x => x.Trinn)
                            .FirstOrDefault(x => x.Id == miljovariabel.Id);

                        if (m == null) continue;

                        foreach (var trinn in m.Trinn)
                        {
                            var t = dbContext.Trinn
                                .Include(x => x.Kode)
                                .Include(x => x.Basistrinn)
                                .FirstOrDefault(x => x.Id == trinn.Id);

                            if (t == null) continue;

                            totalCount += t.Basistrinn.Count;

                            dbContext.Kode.Remove(t.Kode);

                            totalCount++;
                            dbContext.Trinn.Remove(t);
                        }

                        totalCount++;
                        dbContext.LKMKode.Remove(m.Kode);

                        totalCount++;
                        dbContext.Miljovariabel.Remove(m);
                    }

                    totalCount += h.Kartleggingsenheter.Count;
                    totalCount += h.Miljovariabler.Count;
                    totalCount += h.UnderordnetKoder.Count;

                    totalCount++;
                    dbContext.Hovedtype.Remove(h);
                }

                dbContext.Hovedtypegruppe.Remove(hovedtypegruppe);
            }

            //totalCount += dbContext.Hovedtypegruppe.Count();
            //dbContext.Hovedtypegruppe.RemoveRange(dbContext.Hovedtypegruppe);

            //totalCount += dbContext.Natursystem.Count();
            dbContext.Natursystem.Remove(natursystem);

            dbContext.NinVersion.Remove(ninVersion);

            _stopwatch.Stop();
            Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            _stopwatch.Reset();
            _stopwatch.Start();
            dbContext.SaveChanges();
            Console.WriteLine($"Removed {totalCount} items");
            _stopwatch.Stop();
            Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

            _stopwatch.Start();

            dbContext.SaveChanges();

            ResetCounters(dbContext);

            _stopwatch.Stop();
            Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

            _stopwatch.Start();
        }

        #region private methods

        private static void ResetCounters(NiNDbContext dbContext)
        {
            if (!dbContext.Database.ProviderName.Equals("Microsoft.EntityFrameworkCore.SqlServer")) return;

            return;

            var n_id = dbContext.Database.ExecuteSqlRaw($"select max(id) from [{dbContext.Model.FindEntityType(typeof(Natursystem)).GetTableName()}]");

            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Natursystem)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Hovedtypegruppe)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Hovedtype)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Grunntype)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Kartleggingsenhet)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Miljovariabel)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(NinKode)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(LKMKode)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Trinn)).GetTableName()}]', RESEED, 0)");
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('[{dbContext.Model.FindEntityType(typeof(Basistrinn)).GetTableName()}]', RESEED, 0)");
        }

        #endregion
    }
}
