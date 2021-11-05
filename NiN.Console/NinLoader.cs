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

                    Console.WriteLine($"NiN-code version {ninVersion.Navn} exists. Skipping...");
                    return;
                }
                Console.WriteLine($"Adding NiN-code version {version}");

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
                                    }
                                    else
                                    {
                                        kartleggingsenhet.Definisjon = krt.Navn.Trim();
                                        //kartleggingsenhet.KodeId = $"{hovedtype.Hovedtypegruppe.Natursystem.Kode.Definisjon} {krt.Kode.Definition}";
                                        context.Kartleggingsenhet.Update(kartleggingsenhet);
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
                                    }

                                    context.Miljovariabel.Add(miljovariabel);
                                }
                                else
                                {
                                    miljovariabel.Navn = m.Navn.Trim();
                                    context.Miljovariabel.Update(miljovariabel);
                                }
                            }
                        }
                    }
                }

                context.SaveChanges();

                _stopwatch.Stop();
                Console.WriteLine($"Added NiN-code version {ninVersion.Navn} in {_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            }
        }
    }
}
