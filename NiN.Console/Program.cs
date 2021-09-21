namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using NiN.Database;
    using NiN.Database.Models;
    using NiN.Database.Models.Enums;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new NiNContext())
            {
                //Console.WriteLine($"Database: {ninContext.ConnectionString}");

                var na = context.Natursystem
                    .Include(x => x.UnderordnetKoder)
                    .FirstOrDefault(x => x.NatursystemKode.KodeName.Equals("NA"));

                if (na == null)
                {
                    var natursystem = new Natursystem
                    {
                        Navn = "Natursystem",
                        NatursystemKode = new NatursystemKode { KodeName = "NA" }
                    };
                    natursystem.UnderordnetKoder.Add(new Hovedtypegruppe
                    {
                        Navn = "Limniske vannmasser",
                        HovedtypegruppeKode = new HovedtypegruppeKode
                        {
                            KodeName = "F",
                            Definisjon = "definisjon"
                        }
                    });

                    context.Add(GenerateNatursystem());
                }
                else
                {
                    var limn = context.HovedtypegruppeKode
                        .FirstOrDefault(x => x.KodeName.Equals("F"));

                    if (limn != null)
                        Console.WriteLine(limn);

                    //if (na.UnderordnetKoder != null)
                    //    foreach (var child in na.UnderordnetKoder)
                    //    {
                    //        context.Remove(child);
                    //    }

                    //if (na.Kartleggingsenheter != null)
                    //    foreach (var child in na.Kartleggingsenheter)
                    //    {
                    //        context.Remove(child);
                    //    }

                    //if (na.Miljovariabler != null)
                    //    foreach (var child in na.Miljovariabler)


                    //        context.Remove(child);
                    //    }

                    context.Remove(na);
                    Console.WriteLine("NA exist");
                }

                context.SaveChanges();
            }
        }

        private static Natursystem GenerateNatursystem()
        {
            var na = new Natursystem
            {
                Navn = "Natursystem",
                NatursystemKode = new NatursystemKode { KodeName = "NA" },
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
                HovedtypegruppeKode = new HovedtypegruppeKode
                {
                    KodeName = "F",
                    Definisjon = "definisjon"
                }
            });

            return htg;
        }
    }
}
