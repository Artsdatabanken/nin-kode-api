namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using NiN.Database;
    using NiN.Database.Models;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new NiNContext())
            {
                //Console.WriteLine($"Database: {ninContext.ConnectionString}");

                var na = context.Natursystem
                    .Include(x => x.Kode)
                    .Include(x => x.UnderordnetKoder)
                    .FirstOrDefault(x => x.Kode.KodeName.Equals("NA"));

                if (na == null)
                {
                    var natursystem = GenerateNatursystem();
                    context.Add(natursystem);
                }
                else
                {
                    var limn = context.Kode
                        .FirstOrDefault(x => x.KodeName.Equals("F"));

                    if (limn != null)
                        Console.WriteLine(limn);
                    
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
