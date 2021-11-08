namespace NiN.Console
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
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Services.v1;
    using NinKode.Database.Services.v2;
    using NinKode.Database.Services.v21;
    using NinKode.Database.Services.v21b;
    using NinKode.Database.Services.v22;

    public static class NinLoader
    {
        private static Stopwatch _stopwatch = new();

        public static void CreateCodeDatabase(string version)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

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
                Console.WriteLine($"CodeService for NiN-version '{version}' doesn't exist. Skipping...");
                return;
            }

            var na = codeService.GetCode("NA");

            using (var context = new NiNContext())
            {
                var ninVersion = context.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));

                Natursystem natursystem = null;
                if (ninVersion != null)
                {
                    natursystem = context.Natursystem.FirstOrDefault(x => x.Version.Id == ninVersion.Id);
                    if (natursystem != null)
                    {
                        Console.WriteLine($"NiN-code version {ninVersion.Navn} exists. Skipping...");
                        return;
                    }
                }
                else
                {
                    ninVersion = new NinVersion { Navn = version };
                    context.NinVersion.Add(ninVersion);
                }

                AddNatursystem(codeService, context, ninVersion, na);

                context.SaveChanges();

                _stopwatch.Stop();
                Console.WriteLine($"Added NiN-code version {ninVersion.Navn} in {_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            }
        }

        #region private methods

        private static void AddNatursystem(ICodeService codeService,
                                           NiNContext context,
                                           NinVersion ninVersion,
                                           Codes na)
        {
            var natursystem = new Natursystem
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

            AddHovedtypegrupper(codeService, context, ninVersion, na, natursystem);
        }

        private static void AddHovedtypegrupper(ICodeService codeService,
                                                NiNContext context,
                                                NinVersion ninVersion,
                                                Codes na,
                                                Natursystem natursystem)
        {
            if (na.UnderordnetKoder == null) return;

            foreach (var child in na.UnderordnetKoder)
            {
                var gruppe = codeService.GetCode(child.Id);
                
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
                }

                AddHovedtyper(codeService, context, ninVersion, gruppe, hovedtypegruppe);
            }

        }

        private static void AddHovedtyper(ICodeService codeService,
                                          NiNContext context,
                                          NinVersion ninVersion,
                                          Codes gruppe,
                                          Hovedtypegruppe hovedtypegruppe)
        {
            if (gruppe.UnderordnetKoder == null) return;

            foreach (var child in gruppe.UnderordnetKoder)
            {
                var hvdtype = codeService.GetCode(child.Id);
                
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
                }

                AddGrunntyper(codeService, context, ninVersion, hvdtype, hovedtype);
                AddKartleggingsenheter(codeService, context, ninVersion, hvdtype, hovedtype);
                AddMiljovariabler(context, ninVersion, hvdtype, hovedtype);
            }
        }

        private static void AddGrunntyper(ICodeService codeService,
            NiNContext context,
            NinVersion ninVersion,
            Codes hvdtype,
            Hovedtype hovedtype)
        {
            if (hvdtype.UnderordnetKoder == null) return;

            foreach (var child in hvdtype.UnderordnetKoder)
            {
                var grtype = codeService.GetCode(child.Id);

                Grunntype grunntype = null;

                if (context.Grunntype.Any())
                {
                    grunntype = context.Grunntype
                        .FirstOrDefault(x => x.Version.Navn.Equals(ninVersion.Navn));
                    if (grunntype != null && grunntype.Navn.Equals(grtype.Navn.Trim()))
                    {
                        grunntype = null;
                    }

                    if (grunntype != null) continue;
                }

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
            }
        }

        private static void AddKartleggingsenheter(ICodeService codeService,
                                                   NiNContext context,
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
                        context.Kartleggingsenhet.Update(kartleggingsenhet);
                    }
                }
            }
        }

        private static void AddMiljovariabler(NiNContext context,
                                              NinVersion ninVersion,
                                              Codes hvdtype,
                                              Hovedtype hovedtype)
        {
            if (hvdtype.Miljovariabler == null) return;

            foreach (var child in hvdtype.Miljovariabler)
            {
                Miljovariabel miljovariabel = null;

                if (context.Miljovariabel.Any())
                {
                    miljovariabel = context.Miljovariabel
                        .FirstOrDefault(x =>
                            x.Version.Navn.Equals(ninVersion.Navn) &&
                            x.Kode.Kode.Equals(child.Kode));
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

                        AddTrinn(ninVersion, child, miljovariabel, hovedtype);
                    }

                    context.Miljovariabel.Add(miljovariabel);
                }
                else
                {
                    miljovariabel.Navn = child.Navn.Trim();
                    context.Miljovariabel.Update(miljovariabel);
                }
            }
        }

        private static void AddTrinn(NinVersion ninVersion,
                                     EnvironmentVariable env,
                                     Miljovariabel miljovariabel,
                                     Hovedtype hovedtype)
        {
            foreach (var child in env.Trinn)
            {
                var trinn = new Trinn
                {
                    Version = ninVersion,
                    Navn = child.Navn.Trim(),
                    Kode = new TrinnKode
                    {
                        Version = ninVersion,
                        KodeName = $"{child.Kode}",
                        Kategori = KategoriEnum.Trinn
                    }
                };

                AddBasistrinn(ninVersion, child, trinn, hovedtype);

                miljovariabel.Trinn.Add(trinn);
            }
        }

        private static void AddBasistrinn(NinVersion ninVersion,
                                          Step child,
                                          Trinn trinn,
                                          Hovedtype hovedtype)
        {
            if (child.Basistrinn == null) return;

            foreach (var b in child.Basistrinn.Split(","))
            {
                trinn.Basistrinn.Add(new Basistrinn
                {
                    Version = ninVersion,
                    Navn = b.Trim(),
                    Kode = new BasistrinnKode
                    {
                        Version = ninVersion,
                        Kategori = KategoriEnum.Basistrinn,
                        KodeName = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {child.Kode} {b}"
                    }
                });
            }
        }

        #endregion
    }
}
