namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.Json;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NiN.Database.Converters;
    using NiN.Database.Models;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Enums;
    using NinKode.Database.Services.v22;

    public class Program
    {
        private static Stopwatch _stopwatch = new Stopwatch();

        public static void Main(string[] args)
        {
            _stopwatch.Start();

            var totalCount = 0;
            var updateCount = 0;

            var options = new JsonSerializerOptions { WriteIndented = true };

            var code22Service = new CodeV22Service(new ConfigurationRoot(new List<IConfigurationProvider>()));

            var na = code22Service
                .GetCode("NA");
            //Console.WriteLine(JsonSerializer.Serialize(na, options));

            RemoveAll();

            using (var context = new NiNContext())
            {
                var natursystem = context.Natursystem.FirstOrDefault();

                if (natursystem == null)
                {
                    natursystem = new Natursystem
                    {
                        Navn = na.Navn,
                        Kode = new NatursystemKode
                        {
                            KodeName = na.Kode.Id,
                            Definisjon = na.Kode.Definition
                        }
                    };
                    context.Natursystem.Add(natursystem);
                    totalCount++;
                }

                foreach (var htgrp in na.UnderordnetKoder)
                {
                    var gruppe = code22Service.GetCode(htgrp.Id);

                    Hovedtypegruppe hovedtypegruppe = null;

                    if (context.Hovedtypegruppe.Any())
                    {
                        hovedtypegruppe = context.Hovedtypegruppe
                            .FirstOrDefault(x => x.Navn.Equals(gruppe.Navn));
                    }

                    if (hovedtypegruppe == null)
                    {
                        hovedtypegruppe = new Hovedtypegruppe
                        {
                            Natursystem = natursystem,
                            Navn = gruppe.Navn,
                            Kode = new HovedtypegruppeKode
                            {
                                KodeName = gruppe.Kode.Id,
                                Definisjon = gruppe.Kode.Definition
                            }
                        };
                        context.Hovedtypegruppe.Add(hovedtypegruppe);
                        totalCount++;
                    }

                    foreach (var grp in gruppe.UnderordnetKoder)
                    {
                        var hvdtype = code22Service.GetCode(grp.Id);

                        Hovedtype hovedtype = null;

                        if (context.Hovedtype.Any())
                        {
                            hovedtype = context.Hovedtype
                                .Include(x => x.Kode)
                                .FirstOrDefault(x => x.Navn.Equals(hvdtype.Navn));
                        }

                        if (hovedtype == null)
                        {
                            hovedtype = new Hovedtype
                            {
                                Hovedtypegruppe = hovedtypegruppe,
                                Navn = hvdtype.Navn,
                                Kode = new HovedtypeKode
                                {
                                    KodeName = hvdtype.Kode.Id,
                                    Definisjon = hvdtype.Kode.Definition
                                }
                            };
                            context.Hovedtype.Add(hovedtype);
                            totalCount++;
                        }

                        if (hvdtype.UnderordnetKoder != null)
                        {
                            foreach (var u in hvdtype.UnderordnetKoder)
                            {
                                var grtype = code22Service.GetCode(u.Id);

                                Grunntype grunntype = null;

                                if (context.Grunntype.Any())
                                {
                                    grunntype = context.Grunntype
                                        .FirstOrDefault(x => x.Navn.Equals(grtype.Navn));
                                }

                                if (grunntype == null)
                                {
                                    grunntype = new Grunntype
                                    {
                                        Hovedtype = hovedtype,
                                        Navn = grtype.Navn,
                                        Kode = new GrunntypeKode
                                        {
                                            KodeName = grtype.Kode.Id,
                                            Definisjon = grtype.Kode.Definition
                                        }
                                    };
                                    context.Grunntype.Add(grunntype);
                                    totalCount++;
                                }
                            }
                        }

                        if (hvdtype.Kartleggingsenheter != null)
                        {
                            foreach (var k in hvdtype.Kartleggingsenheter)
                            {
                                foreach (var v in k.Value)
                                {
                                    var krt = code22Service.GetCode(v.Id);

                                    Kartleggingsenhet kartleggingsenhet = null;

                                    if (context.Kartleggingsenhet.Any())
                                    {
                                        kartleggingsenhet = context.Kartleggingsenhet
                                            .FirstOrDefault(x => x.Kode.Id.Equals(krt.ElementKode));
                                    }

                                    if (kartleggingsenhet == null)
                                    {
                                        kartleggingsenhet = new Kartleggingsenhet
                                        {
                                            //Hovedtype = hovedtype,
                                            Definisjon = krt.Navn,
                                            //KodeId = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {krt.Kode.Definition}"
                                            Kode = new KartleggingsenhetKode
                                            {
                                                KodeName = $"{krt.Kode.Id}",
                                                Definisjon = krt.Kode.Definition
                                            }
                                        };
                                        switch (Convert.ToInt32(k.Key))
                                        {
                                            case 2500:
                                                kartleggingsenhet.Malestokk = MalestokkEnum.Malestokk2500;
                                                break;
                                            case 5000:
                                                kartleggingsenhet.Malestokk = MalestokkEnum.Malestokk5000;
                                                break;
                                            case 10000:
                                                kartleggingsenhet.Malestokk = MalestokkEnum.Malestokk10000;
                                                break;
                                            case 20000:
                                                kartleggingsenhet.Malestokk = MalestokkEnum.Malestokk20000;
                                                break;
                                        }

                                        //context.Kartleggingsenhet.Add(kartleggingsenhet);
                                        hovedtype.Kartleggingsenheter.Add(kartleggingsenhet);
                                        totalCount++;
                                    }
                                    else
                                    {
                                        kartleggingsenhet.Definisjon = krt.Navn;
                                        //kartleggingsenhet.KodeId = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {krt.Kode.Definition}";
                                        context.Kartleggingsenhet.Update(kartleggingsenhet);
                                        updateCount++;
                                    }
                                }

                            }

                            if (hvdtype.Miljovariabler != null)
                            {
                                foreach (var m in hvdtype.Miljovariabler)
                                {
                                    Miljovariabel miljovariabel = null;

                                    if (context.Miljovariabel.Any())
                                    {
                                        miljovariabel = context.Miljovariabel
                                            .FirstOrDefault(x => x.Kode.Kode.Equals(m.Kode));
                                    }

                                    if (miljovariabel == null)
                                    {
                                        miljovariabel = new Miljovariabel
                                        {
                                            Hovedtype = hovedtype,
                                            Kode = new LKMKode
                                            {
                                                Kode = $"{m.Kode}",
                                                LkmKategori = NinEnumConverter.Convert<LkmKategoriEnum>(m.LKMKategori).Value
                                            },
                                            Navn = m.Navn
                                        };
                                        foreach (var t in m.Trinn)
                                        {
                                            var trinn = new Trinn
                                            {
                                                //Navn = $"{t.Kode} - {t.Basistrinn} - {t.Navn}"
                                                Navn = t.Navn,
                                                Kode = new TrinnKode
                                                {
                                                    //KodeName = t.Kode,
                                                    KodeName = $"{t.Kode}",
                                                    Kategori = KategoriEnum.Trinn
                                                }
                                            };
                                            foreach (var b in t.Basistrinn.Split(","))
                                            {
                                                trinn.Basistrinn.Add(new Basistrinn
                                                {
                                                    Navn = b.Trim(),
                                                    Kode = new BasistrinnKode
                                                    {
                                                        //KodeName = b,
                                                        KodeName = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {t.Kode} {b}",
                                                        Kategori = KategoriEnum.Basistrinn
                                                    }
                                                });
                                            }
                                            miljovariabel.Trinn.Add(trinn);
                                            totalCount++;
                                        }

                                        context.Miljovariabel.Add(miljovariabel);
                                        totalCount++;
                                    }
                                    else
                                    {
                                        miljovariabel.Navn = m.Navn;
                                        context.Miljovariabel.Update(miljovariabel);
                                        updateCount++;
                                    }
                                }
                            }
                        }
                    }
                }
                _stopwatch.Stop();
                Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
                _stopwatch.Reset();
                _stopwatch.Start();

                context.SaveChanges();
            }

            Console.WriteLine($"Added {totalCount} items");
            Console.WriteLine($"Updated {updateCount} items");

            _stopwatch.Stop();
            Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

            //using (var context = new NiNContext())
            //{
            //    //Console.WriteLine($"Database: {ninContext.ConnectionString}");

            //    var na = context.Natursystem
            //        .Include(x => x.Kode)
            //        .Include(x => x.UnderordnetKoder)
            //        .FirstOrDefault(x => x.Kode.KodeName.Equals("NA"));

            //    if (na == null)
            //    {
            //        var natursystem = GenerateNatursystem();
            //        context.Add(natursystem);
            //    }
            //    else
            //    {
            //        var limn = context.Hovedtypegruppe
            //            .Include(x => x.Kode)
            //            .FirstOrDefault();

            //        if (limn != null)
            //            Console.WriteLine(limn);

            //        context.Remove(na);
            //        Console.WriteLine("NA exist");
            //    }

            //    context.SaveChanges();
            //}
        }

        private static void RemoveAll()
        {
            using (var context = new NiNContext())
            {
                if (context.Database.EnsureCreated())
                {
                    Console.WriteLine($"Created empty database: {context.DbName}");
                    return;
                }

                var totalCount = 0;

                var natursystem = context.Natursystem
                    .Include(x => x.Kode)
                    .Include(x => x.UnderordnetKoder)
                    .FirstOrDefault();

                if (natursystem == null) return;

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
                }

                totalCount += context.Hovedtypegruppe.Count();
                context.Hovedtypegruppe.RemoveRange(context.Hovedtypegruppe);

                totalCount += context.Natursystem.Count();
                context.Natursystem.RemoveRange(context.Natursystem);
                _stopwatch.Stop();
                Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
                _stopwatch.Reset();
                _stopwatch.Start();
                context.SaveChanges();
                Console.WriteLine($"Removed {totalCount} items");
                _stopwatch.Stop();
                Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

                ResetCounters(context);

                _stopwatch.Start();

                context.SaveChanges();
            }

            _stopwatch.Stop();
            Console.WriteLine($"{_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

            _stopwatch.Start();
        }

        private static void ResetCounters(NiNContext context)
        {
            if (!context.Database.ProviderName.Equals("Microsoft.EntityFrameworkCore.SqlServer")) return;
            
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Natursystem)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Hovedtypegruppe)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Hovedtype)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Grunntype)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Kartleggingsenhet)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Miljovariabel)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Kode)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(LKMKode)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Trinn)).GetTableName()}', RESEED, 0)");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{context.Model.FindEntityType(typeof(Basistrinn)).GetTableName()}', RESEED, 0)");
        }

        private static Natursystem GenerateNatursystem()
        {
            var na = new Natursystem
            {
                Navn = "Natursystem",
                Kode = new NatursystemKode { KodeName = "NA" },
                UnderordnetKoder = GenerateHovedtypegrupper()
            };

            return na;
        }

        private static ICollection<Hovedtypegruppe> GenerateHovedtypegrupper()
        {
            var htg = new List<Hovedtypegruppe>();

            htg.Add(new Hovedtypegruppe
            {
                Navn = "Limniske vannmasser",
                Kode = new HovedtypegruppeKode
                {
                    KodeName = "F",
                    Definisjon = "definisjon"
                }
            });

            return htg;
        }
    }
}
