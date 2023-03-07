namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NiN.Database.Converters;
    using NiN.Database;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Services.v1;
    using NinKode.Database.Services.v2;
    using NinKode.Database.Services.v21;
    using NinKode.Database.Services.v21b;
    using NinKode.Database.Services.v22;
    using NinKode.Database.Services.v30;

    public static class NinLoader
    {
        private static Stopwatch _stopwatch = new();

        public static void CreateCodeDatabase(ServiceProvider serviceProvider, string version, bool allowUpdate = false, IConfiguration configuration=null)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            var dbContext = serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null) throw new Exception("Could not get DbContext");

            ICodeService codeService = null;

            switch (version)
            {
                case "1":
                    //codeService = new CodeV1Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    codeService = new CodeV1Service(configuration);
                    break;
                case "2":
                    //codeService = new CodeV2Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    codeService = new CodeV2Service(configuration);
                    break;
                case "2.1":
                    //codeService = new CodeV21Service(new ConfigurationRoot(new List<IConfigurationProvider>()));
                    codeService = new CodeV21Service(configuration);
                    break;
                case "2.1b":
                    codeService = new CodeV21BService(configuration);
                    break;
                case "2.2":
                case "2.3":
                    codeService = new CodeV22Service(configuration);
                    break;
                case "3.0":
                    codeService = new CodeV30Service(configuration);
                    Console.WriteLine("CreateCodeDatabase for 3.0, not yet impl.");
                    break;
            }

            if (codeService == null)
            {
                Console.WriteLine($"CodeService for NiN-version '{version}' doesn't exist. Skipping...");
                return;
            }

            // missing systems: LA, LD, LI
            var ninSystems = new[] { "LA", "LD", "LI", "NA" };

            foreach (var prefix in ninSystems)
            {
                var na = codeService.GetCode(prefix);
                if (na == null) continue;

                var ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
                if (ninVersion != null)
                {
                    var ninSystem = dbContext.Natursystem
                        .Include(x => x.Kode)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(prefix));
                    if (!allowUpdate && ninSystem != null)
                    {
                        Console.WriteLine($"NiN-code '{prefix}' version {ninVersion.Navn} exists. Skipping...");
                        continue;
                    }
                }
                else
                {
                    ninVersion = new NinVersion { Navn = version };
                    dbContext.NinVersion.Add(ninVersion);
                    dbContext.SaveChanges();

                    ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
                }

                AddOrUpdateNatursystem(codeService, dbContext, ninVersion, na);

                dbContext.SaveChanges();

                _stopwatch.Stop();
                Console.WriteLine($"Added NiN-code {prefix} version {ninVersion.Navn} in {_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            }
        }

        #region private methods

        private static void AddOrUpdateNatursystem(ICodeService codeService,
                                                   NiNDbContext dbContext,
                                                   NinVersion ninVersion,
                                                   Codes na)
        {
            Natursystem natursystem = null;
            natursystem = dbContext.Natursystem
                .Include(x => x.Kode)
                .FirstOrDefault(x =>
                    x.Version.Id == ninVersion.Id &&
                    x.Kode.KodeName.Equals(na.Kode.Id));
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
                dbContext.Natursystem.Add(natursystem);
            }
            else
            {
                natursystem.Navn = na.Navn;
                natursystem.Kode.KodeName = na.Kode.Id;
                natursystem.Kode.Definisjon = na.Kode.Definition;

                dbContext.Natursystem.Update(natursystem);
            }

            dbContext.SaveChanges();

            AddOrUpdateHovedtypegrupper(codeService, dbContext, ninVersion, na, natursystem);
        }

        private static void AddOrUpdateHovedtypegrupper(ICodeService codeService,
                                                        NiNDbContext dbContext,
                                                        NinVersion ninVersion,
                                                        Codes na,
                                                        Natursystem natursystem)
        {
            if (na.UnderordnetKoder == null) return;

            foreach (var child in na.UnderordnetKoder)
            {
                var gruppe = codeService.GetCode(child.Id);
                
                Hovedtypegruppe hovedtypegruppe = null;

                if (dbContext.Hovedtypegruppe.Any())
                {
                    hovedtypegruppe = dbContext.Hovedtypegruppe
                        .Include(x => x.Kode)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(gruppe.Kode.Id));
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

                    dbContext.Hovedtypegruppe.Add(hovedtypegruppe);
                }
                else
                {
                    hovedtypegruppe.Navn = gruppe.Navn.Trim();
                    hovedtypegruppe.Kode.KodeName = gruppe.Kode.Id;
                    hovedtypegruppe.Kode.Definisjon = gruppe.Kode.Definition.Trim();

                    dbContext.Hovedtypegruppe.Update(hovedtypegruppe);
                }

                AddOrUpdateHovedtyper(codeService, dbContext, ninVersion, gruppe, hovedtypegruppe);
            }
        }

        private static void AddOrUpdateHovedtyper(ICodeService codeService,
                                                  NiNDbContext dbContext,
                                                  NinVersion ninVersion,
                                                  Codes gruppe,
                                                  Hovedtypegruppe hovedtypegruppe)
        {
            if (gruppe.UnderordnetKoder == null) return;

            foreach (var child in gruppe.UnderordnetKoder)
            {
                var hvdtype = codeService.GetCode(child.Id);
                
                Hovedtype hovedtype = null;

                if (dbContext.Hovedtype.Any())
                {
                    hovedtype = dbContext.Hovedtype
                        .Include(x => x.Kode)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(hvdtype.Kode.Id.Trim()));
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

                    dbContext.Hovedtype.Add(hovedtype);
                }
                else
                {
                    hovedtype.Navn = hvdtype.Navn.Trim();
                    hovedtype.Kode.KodeName = hvdtype.Kode.Id;
                    hovedtype.Kode.Definisjon = hvdtype.Kode.Definition.Trim();

                    dbContext.Hovedtype.Update(hovedtype);
                }

                AddOrUpdateGrunntyper(codeService, dbContext, ninVersion, hvdtype, hovedtype);
                AddOrUpdateKartleggingsenheter(codeService, dbContext, ninVersion, hvdtype, hovedtype);
                AddOrUpdateMiljovariabler(dbContext, ninVersion, hvdtype, hovedtype);

                // Make basistrinn available and save changes
                dbContext.SaveChanges();
            }
        }

        private static void AddOrUpdateGrunntyper(ICodeService codeService,
                                                  NiNDbContext dbContext,
                                                  NinVersion ninVersion,
                                                  Codes hvdtype,
            Hovedtype hovedtype)
        {
            if (hvdtype.UnderordnetKoder == null) return;

            foreach (var child in hvdtype.UnderordnetKoder)
            {
                var grtype = codeService.GetCode(child.Id);

                Grunntype grunntype = null;

                if (dbContext.Grunntype.Any())
                {
                    grunntype = dbContext.Grunntype
                        .Include(x => x.Kode)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(grtype.Kode.Id.Trim()));

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

                    dbContext.Grunntype.Add(grunntype);
                }
                else
                {
                    grunntype.Navn = grtype.Navn.Trim();
                    grunntype.Kode.KodeName = grtype.Kode.Id;
                    grunntype.Kode.Definisjon = grtype.Kode.Definition.Trim();

                    dbContext.Grunntype.Update(grunntype);
                }
            }
        }

        private static void AddOrUpdateKartleggingsenheter(ICodeService codeService,
                                                           NiNDbContext dbContext,
                                                           NinVersion ninVersion,
                                                           Codes hvdtype,
                                                           Hovedtype hovedtype)
        {
            if (hvdtype.Kartleggingsenheter == null) return;

            foreach (var child in hvdtype.Kartleggingsenheter)
            {
                foreach (var v in child.Value)
                {
                    var krt = codeService.GetCode(v.Id);

                    Kartleggingsenhet kartleggingsenhet = null;

                    if (dbContext.Kartleggingsenhet.Any())
                    {
                        kartleggingsenhet = dbContext.Kartleggingsenhet
                            .Include(x => x.Kode)
                            .FirstOrDefault(x =>
                                x.Version.Id == ninVersion.Id &&
                                x.Kode.KodeName.Equals(krt.Kode.Id.Trim()));
                    }

                    if (kartleggingsenhet == null)
                    {
                        kartleggingsenhet = new Kartleggingsenhet
                        {
                            Version = ninVersion,
                            Definisjon = krt.Navn.Trim(),
                            Kode = new KartleggingsenhetKode
                            {
                                Version = ninVersion,
                                KodeName = $"{krt.Kode.Id}",
                                Definisjon = krt.Kode.Definition.Trim()
                            }
                        };
                        switch (Convert.ToInt32(child.Key))
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

                        hovedtype.Kartleggingsenheter.Add(kartleggingsenhet);
                    }
                    else
                    {
                        kartleggingsenhet.Definisjon = krt.Navn.Trim();
                        kartleggingsenhet.Kode.KodeName = $"{krt.Kode.Id}";
                        kartleggingsenhet.Kode.Definisjon = krt.Kode.Definition.Trim();

                        dbContext.Kartleggingsenhet.Update(kartleggingsenhet);
                    }
                }
            }
        }

        private static void AddOrUpdateMiljovariabler(NiNDbContext dbContext,
                                              NinVersion ninVersion,
                                              Codes hvdtype,
                                              Hovedtype hovedtype)
        {
            if (hvdtype.Miljovariabler == null) return;

            dbContext.SaveChanges();

            foreach (var child in hvdtype.Miljovariabler)
            {
                Miljovariabel miljovariabel = null;

                // Each miljøvariabel has to be unique because of different trinn/basistrinn
                if (dbContext.Miljovariabel.Any())
                {
                    miljovariabel = dbContext.Miljovariabel
                        .Include(x => x.Kode)
                        .Include(x => x.Hovedtype)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtype.Id == hovedtype.Id &&
                            x.Kode.Kode.Equals($"{child.Kode}"));
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
                            Kode = $"{child.Kode}"
                        },
                        Navn = child.Navn.Trim()
                    };

                    if (child.LKMKategori != null)
                    {
                        miljovariabel.Kode.LkmKategori =
                            NinEnumConverter.Convert<LkmKategoriEnum>(child.LKMKategori).Value;
                    }

                    AddOrUpdateTrinn(dbContext, ninVersion, child, miljovariabel);

                    dbContext.Miljovariabel.Add(miljovariabel);
                }
                else
                {
                    miljovariabel.Navn = child.Navn.Trim();
                    miljovariabel.Kode.Kode = $"{child.Kode}";
                    if (child.LKMKategori != null) miljovariabel.Kode.LkmKategori =
                            NinEnumConverter.Convert<LkmKategoriEnum>(child.LKMKategori).Value;

                    AddOrUpdateTrinn(dbContext, ninVersion, child, miljovariabel);

                    dbContext.Miljovariabel.Update(miljovariabel);
                }
            }
        }

        private static void AddOrUpdateTrinn(NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             EnvironmentVariable env,
                                             Miljovariabel miljovariabel)
        {
            foreach (var step in env.Trinn)
            {
                Trinn trinn = null;

                dbContext.SaveChanges();

                if (dbContext.Trinn.Any())
                {
                    trinn = dbContext.Trinn
                        .Include(x => x.Kode)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Miljovariabel.Kode.Kode.Equals($"{step.Kode}"));
                }

                if (trinn == null)
                {
                    trinn = new Trinn
                    {
                        Version = ninVersion,
                        Navn = step.Navn.Trim(),
                        Kode = new TrinnKode
                        {
                            Version = ninVersion,
                            KodeName = $"{step.Kode}",
                            Kategori = KategoriEnum.Trinn
                        }
                    };

                    miljovariabel.Trinn.Add(trinn);
                }
                else
                {
                    trinn.Navn = step.Navn.Trim();
                }

                AddOrUpdateBasistrinn(dbContext, ninVersion, step, trinn);

            }
        }

        private static void AddOrUpdateBasistrinn(NiNDbContext dbContext,
                                                  NinVersion ninVersion,
                                                  Step step,
                                                  Trinn trinn)
        {
            if (step.Basistrinn == null) return;

            foreach (var b in step.Basistrinn.Split(","))
            {
                var kodename = b.Trim();

                // Check if basistrinn exists
                var basistrinn = dbContext.Basistrinn
                    .FirstOrDefault(x =>
                        x.Version.Id == ninVersion.Id &&
                        x.Navn.Equals(kodename));
                if (basistrinn != null)
                {
                    trinn.Basistrinn.Add(basistrinn);
                    continue;
                }

                trinn.Basistrinn.Add(new Basistrinn
                {
                    Version = ninVersion,
                    Navn = kodename
                });
            }
        }

        #endregion
    }
}
