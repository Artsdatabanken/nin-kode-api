namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using NiN.Database;
    using NiN.Database.Models;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Enums;
    using NinKode.Database.Services.v22;

    public class Program
    {
        public static void Main(string[] args)
        {
            var totalCount = 0;
            var updateCount = 0;

            var options = new JsonSerializerOptions { WriteIndented = true };

            var code22Service = new CodeV22Service(new ConfigurationRoot(new List<IConfigurationProvider>()));

            
            var htgruppe = code22Service
                .GetAll("")
                .ToList()
                .Where(x => x.Kategori.StartsWith("Natur", StringComparison.OrdinalIgnoreCase));

            var na = code22Service
                .GetAll("")
                .FirstOrDefault(x => x.Kategori.StartsWith("Natur", StringComparison.OrdinalIgnoreCase));
            //Console.WriteLine(JsonSerializer.Serialize(na, options));

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
                    var gruppe = code22Service.GetByKode(htgrp.Id, "");

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
                        var hvdtype = code22Service.GetByKode(grp.Id, "");

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
                                var grtype = code22Service.GetByKode(u.Id, "");

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
                                    var krt = code22Service.GetByKode(v.Id, "");

                                    Kartleggingsenhet kartleggingsenhet = null;

                                    if (context.Kartleggingsenhet.Any())
                                    {
                                        kartleggingsenhet = context.Kartleggingsenhet
                                            .FirstOrDefault(x => x.KodeId.Equals(krt.ElementKode));
                                    }

                                    if (kartleggingsenhet == null)
                                    {
                                        kartleggingsenhet = new Kartleggingsenhet
                                        {
                                            Hovedtype = hovedtype,
                                            Definisjon = krt.Navn,
                                            KodeId = krt.Kode.Definition
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

                                        context.Kartleggingsenhet.Add(kartleggingsenhet);
                                        totalCount++;
                                    }
                                    else
                                    {
                                        kartleggingsenhet.Definisjon = krt.Navn;
                                        kartleggingsenhet.KodeId = krt.Kode.Definition;
                                        context.Kartleggingsenhet.Update(kartleggingsenhet);
                                        updateCount++;
                                    }
                                }

                            }
                        }
                    }
                }

                context.SaveChanges();
            }

            Console.WriteLine($"Added {totalCount} items");
            Console.WriteLine($"Updated {updateCount} items");

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
