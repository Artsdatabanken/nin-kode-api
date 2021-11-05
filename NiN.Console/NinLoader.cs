﻿namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NiN.Database.Converters;
    using NiN.Database;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety;
    using NiN.Database.Models.Variety.Codes;
    using NiN.Database.Models.Variety.Enums;
    using NinKode.Common.Interfaces;
    using NinKode.Database.Services.v1;
    using NinKode.Database.Services.v2;
    using NinKode.Database.Services.v21;
    using NinKode.Database.Services.v21b;
    using NinKode.Database.Services.v22;

    public static class NinLoader
    {
        private static Stopwatch _stopwatch = new();

        public static void CreateVarietyDatabase(string version)
        {
            IVarietyService varietyService = null;

            switch (version)
            {
                case "2.1":
                    varietyService = new VarietyV21Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
                case "2.1b":
                    varietyService = new VarietyV21BService(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
                case "2.2":
                case "2.3":
                    varietyService = new VarietyV22Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
            }

            if (varietyService == null)
            {
                Console.WriteLine($"VarietyService for version '{version}' doesn't exist. Skipping...");
                return;
            }

            using (var context = new NiNContext())
            {
                var ninVersion = context.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));

                VarietyLevel0 varietyLevel0 = null;
                if (ninVersion != null)
                {
                    varietyLevel0 = context.VarietyLevel0s.FirstOrDefault(x => x.Version.Id == ninVersion.Id);
                    if (varietyLevel0 != null)
                    {
                        Console.WriteLine($"Variety version {ninVersion.Navn} exists. Skipping...");
                        return;
                    }
                }
                else
                {
                    ninVersion = new NinVersion { Navn = version };
                    context.NinVersion.Add(ninVersion);
                }

                var besys0 = varietyService.GetVariety("besys0");

                varietyLevel0 = new VarietyLevel0
                {
                    Version = ninVersion,
                    Navn = besys0.Name.Trim(),
                    Kode = new VarietyLevel0Code
                    {
                        Version = ninVersion,
                        KodeName = besys0.Code.Id.Trim()
                    }
                };

                context.VarietyLevel0s.Add(varietyLevel0);

                foreach (var v1 in besys0.UnderordnetKoder)
                {
                    var variety1 = varietyService.GetVariety(v1.Id);
                    var varietyLevel1 = new VarietyLevel1
                    {
                        Version = ninVersion,
                        OverordnetKode = varietyLevel0,
                        Navn = variety1.Name.Trim(),
                        Kode = new VarietyLevel1Code
                        {
                            Version = ninVersion,
                            KodeName = v1.Id.Trim()
                        }
                    };
                    context.VarietyLevel1s.Add(varietyLevel1);

                    if (variety1.UnderordnetKoder == null) continue;

                    foreach (var v2 in variety1.UnderordnetKoder)
                    {
                        var variety2 = varietyService.GetVariety(v2.Id);
                        var varietyLevel2 = new VarietyLevel2
                        {
                            Version = ninVersion,
                            OverordnetKode = varietyLevel1,
                            Navn = variety2.Name.Trim(),
                            Kode = new VarietyLevel2Code
                            {
                                Version = ninVersion,
                                KodeName = v2.Id.Trim()
                            }
                        };
                        context.VarietyLevel2s.Add(varietyLevel2);

                        if (variety2.UnderordnetKoder == null) continue;

                        foreach (var v3 in variety2.UnderordnetKoder)
                        {
                            var variety3 = varietyService.GetVariety(v3.Id);
                            var varietyLevel3 = new VarietyLevel3
                            {
                                Version = ninVersion,
                                OverordnetKode = varietyLevel2,
                                Navn = variety3.Name.Trim(),
                                Kode = new VarietyLevel3Code
                                {
                                    Version = ninVersion,
                                    KodeName = v3.Id.Trim()
                                }
                            };
                            context.VarietyLevel3s.Add(varietyLevel3);

                            if (variety3.UnderordnetKoder == null) continue;

                            foreach (var v4 in variety3.UnderordnetKoder)
                            {
                                var variety4 = varietyService.GetVariety(v4.Id);
                                var varietyLevel4 = new VarietyLevel4
                                {
                                    Version = ninVersion,
                                    OverordnetKode = varietyLevel3,
                                    Navn = variety4.Name.Trim(),
                                    Kode = new VarietyLevel4Code
                                    {
                                        Version = ninVersion,
                                        KodeName = v4.Id.Trim()
                                    }
                                };
                                context.VarietyLevel4s.Add(varietyLevel4);

                                if (variety4.UnderordnetKoder == null) continue;

                                foreach (var v5 in variety4.UnderordnetKoder)
                                {
                                    var variety5 = varietyService.GetVariety(v5.Id);
                                    var varietyLevel5 = new VarietyLevel5
                                    {
                                        Version = ninVersion,
                                        OverordnetKode = varietyLevel4,
                                        Navn = variety5.Name.Trim(),
                                        Kode = new VarietyLevel5Code
                                        {
                                            Version = ninVersion,
                                            KodeName = v5.Id.Trim()
                                        }
                                    };
                                    context.VarietyLevel5s.Add(varietyLevel5);

                                    if (variety5.UnderordnetKoder == null) continue;

                                    if (variety5.UnderordnetKoder != null)
                                    {
                                        throw new NotImplementedException($"'{variety5.Code}: {variety5.Name}'");
                                    }
                                }
                            }
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        public static void CreateCodeDatabase(string version)
        {
            _stopwatch.Start();

            var totalCount = 0;
            var updateCount = 0;

            ICodeService codeService = null;

            switch (version)
            {
                case "1":
                    codeService = new CodeV1Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
                case "2":
                    codeService = new CodeV2Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
                case "2.1":
                    codeService = new CodeV21Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
                case "2.1b":
                    codeService = new CodeV21BService(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
                case "2.2":
                case "2.3":
                    codeService = new CodeV22Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    break;
            }

            if (codeService == null)
            {
                Console.WriteLine($"CodeService for version '{version}' doesn't exist. Skipping...");
                return;
            }

            var na = codeService
                .GetCode("NA");
            //var options = new JsonSerializerOptions { WriteIndented = true };
            //Console.WriteLine(JsonSerializer.Serialize(na, options));

            //RemoveAll(version);

            using (var context = new NiNContext())
            {
                var ninVersion = context.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));

                Natursystem natursystem = null;
                if (ninVersion != null)
                {
                    //natursystem = context.Natursystem.FirstOrDefault(x => x.Version == ninVersion);

                    Console.WriteLine($"Code version {ninVersion.Navn} exists. Skipping...");
                    return;
                }
                Console.WriteLine($"Adding version {version}");

                ninVersion = new NinVersion { Navn = version };
                context.NinVersion.Add(ninVersion);


                if (natursystem == null)
                {
                    natursystem = new Natursystem
                    {
                        Navn = na.Navn,
                        Version = ninVersion,
                        Kode = new NatursystemKode
                        {
                            Version = ninVersion,
                            KodeName = na.Kode.Id,
                            Definisjon = na.Kode.Definition
                        }
                    };
                    context.Natursystem.Add(natursystem);
                    totalCount++;
                }

                foreach (var htgrp in na.UnderordnetKoder)
                {
                    var gruppe = codeService.GetCode(htgrp.Id);

                    Hovedtypegruppe hovedtypegruppe = null;

                    if (context.Hovedtypegruppe.Any())
                    {
                        hovedtypegruppe = context.Hovedtypegruppe
                            .FirstOrDefault(x => x.Version.Navn.Equals(ninVersion.Navn));
                        if (hovedtypegruppe != null && !hovedtypegruppe.Navn.Trim().Equals(gruppe.Navn.Trim()))
                        {
                            hovedtypegruppe = null;
                        }
                    }

                    if (hovedtypegruppe == null)
                    {
                        hovedtypegruppe = new Hovedtypegruppe
                        {
                            Version = ninVersion,
                            Natursystem = natursystem,
                            Navn = gruppe.Navn.Trim(),
                            Kode = new HovedtypegruppeKode
                            {
                                Version = ninVersion,
                                KodeName = gruppe.Kode.Id,
                                Definisjon = gruppe.Kode.Definition.Trim()
                            }
                        };
                        context.Hovedtypegruppe.Add(hovedtypegruppe);
                        totalCount++;
                    }

                    foreach (var grp in gruppe.UnderordnetKoder)
                    {
                        var hvdtype = codeService.GetCode(grp.Id);

                        Hovedtype hovedtype = null;

                        if (context.Hovedtype.Any())
                        {
                            hovedtype = context.Hovedtype
                                .Include(x => x.Kode)
                                .FirstOrDefault(x => x.Version.Navn.Equals(ninVersion.Navn));
                            if (hovedtype != null && hovedtype.Navn.Trim().Equals(hvdtype.Navn.Trim()))
                            {
                                hovedtype = null;
                            }
                        }

                        if (hovedtype == null)
                        {
                            hovedtype = new Hovedtype
                            {
                                Version = ninVersion,
                                Hovedtypegruppe = hovedtypegruppe,
                                Navn = hvdtype.Navn.Trim(),
                                Kode = new HovedtypeKode
                                {
                                    Version = ninVersion,
                                    KodeName = hvdtype.Kode.Id,
                                    Definisjon = hvdtype.Kode.Definition.Trim()
                                }
                            };
                            context.Hovedtype.Add(hovedtype);
                            totalCount++;
                        }

                        if (hvdtype.UnderordnetKoder != null)
                        {
                            foreach (var u in hvdtype.UnderordnetKoder)
                            {
                                var grtype = codeService.GetCode(u.Id);

                                Grunntype grunntype = null;

                                if (context.Grunntype.Any())
                                {
                                    grunntype = context.Grunntype
                                        .FirstOrDefault(x => x.Version.Navn.Equals(ninVersion.Navn));
                                    if (grunntype != null && grunntype.Navn.Equals(grtype.Navn.Trim()))
                                    {
                                        grunntype = null;
                                    }
                                }

                                if (grunntype == null)
                                {
                                    grunntype = new Grunntype
                                    {
                                        Version = ninVersion,
                                        Hovedtype = hovedtype,
                                        Navn = grtype.Navn.Trim(),
                                        Kode = new GrunntypeKode
                                        {
                                            Version = ninVersion,
                                            KodeName = grtype.Kode.Id,
                                            Definisjon = grtype.Kode.Definition.Trim()
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
                                    var krt = codeService.GetCode(v.Id);

                                    Kartleggingsenhet kartleggingsenhet = null;

                                    if (context.Kartleggingsenhet.Any())
                                    {
                                        kartleggingsenhet = context.Kartleggingsenhet
                                            .FirstOrDefault(x =>
                                                x.Version.Navn.Equals(ninVersion.Navn) &&
                                                x.Kode.Id.Equals(krt.ElementKode));
                                    }

                                    if (kartleggingsenhet == null)
                                    {
                                        kartleggingsenhet = new Kartleggingsenhet
                                        {
                                            Version = ninVersion,
                                            //Hovedtype = hovedtype,
                                            Definisjon = krt.Navn.Trim(),
                                            //KodeId = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {krt.Kode.Definition}"
                                            Kode = new KartleggingsenhetKode
                                            {
                                                Version = ninVersion,
                                                KodeName = $"{krt.Kode.Id}",
                                                Definisjon = krt.Kode.Definition.Trim()
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
                                        kartleggingsenhet.Definisjon = krt.Navn.Trim();
                                        //kartleggingsenhet.KodeId = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {krt.Kode.Definition}";
                                        context.Kartleggingsenhet.Update(kartleggingsenhet);
                                        updateCount++;
                                    }
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
                                        .FirstOrDefault(x =>
                                            x.Version.Navn.Equals(ninVersion.Navn) && x.Kode.Kode.Equals(m.Kode));
                                }

                                if (miljovariabel == null)
                                {
                                    miljovariabel = new Miljovariabel
                                    {
                                        Version = ninVersion,
                                        Hovedtype = hovedtype,
                                        Kode = new LKMKode
                                        {
                                            Version = ninVersion,
                                            Kode = $"{m.Kode}"
                                        },
                                        Navn = m.Navn.Trim()
                                    };

                                    if (m.LKMKategori != null)
                                        miljovariabel.Kode.LkmKategori =
                                            NinEnumConverter.Convert<LkmKategoriEnum>(m.LKMKategori).Value;

                                    foreach (var t in m.Trinn)
                                    {
                                        //Console.WriteLine($"{t.Navn.Trim()}: {t.Kode.Length}");
                                        var trinn = new Trinn
                                        {
                                            Version = ninVersion,
                                            //Navn = $"{t.Kode} - {t.Basistrinn} - {t.Navn}"
                                            Navn = t.Navn.Trim(),
                                            Kode = new TrinnKode
                                            {
                                                Version = ninVersion,
                                                //KodeName = t.Kode,
                                                KodeName = $"{t.Kode}",
                                                Kategori = KategoriEnum.Trinn
                                            }
                                        };
                                        if (t.Basistrinn != null)
                                        {
                                            foreach (var b in t.Basistrinn.Split(","))
                                            {
                                                trinn.Basistrinn.Add(new Basistrinn
                                                {
                                                    Version = ninVersion,
                                                    Navn = b.Trim(),
                                                    Kode = new BasistrinnKode
                                                    {
                                                        Version = ninVersion,
                                                        //KodeName = b,
                                                        KodeName =
                                                            $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {t.Kode} {b}",
                                                        Kategori = KategoriEnum.Basistrinn
                                                    }
                                                });
                                            }
                                        }

                                        miljovariabel.Trinn.Add(trinn);
                                        totalCount++;
                                    }

                                    context.Miljovariabel.Add(miljovariabel);
                                    totalCount++;
                                }
                                else
                                {
                                    miljovariabel.Navn = m.Navn.Trim();
                                    context.Miljovariabel.Update(miljovariabel);
                                    updateCount++;
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

        }

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
    }
}
