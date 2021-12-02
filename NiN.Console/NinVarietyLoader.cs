namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NiN.Database;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety;
    using NiN.Database.Models.Variety.Codes;
    using NiN.Database.Models.Variety.Enums;
    using NiN.ExportImport.Model;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Services.v21;
    using NinKode.Database.Services.v21b;
    using NinKode.Database.Services.v22;

    public static class NinVarietyLoader
    {
        private static Stopwatch _stopwatch = new();

        public static void CreateVarietyDatabase(ServiceProvider serviceProvider, string version)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

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

            var dbContext = serviceProvider.GetService<NiNDbContext>();
            var ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));

            VarietyLevel0 varietyLevel0 = null;
            if (ninVersion != null)
            {
                varietyLevel0 = dbContext.VarietyLevel0s.FirstOrDefault(x => x.Version.Id == ninVersion.Id);
                if (varietyLevel0 != null)
                {
                    Console.WriteLine($"Variety version {ninVersion.Navn} exists. Skipping...");
                    return;
                }
            }
            else
            {
                ninVersion = new NinVersion { Navn = version };
                dbContext.NinVersion.Add(ninVersion);
            }

            AddVarietyLevel0(varietyService, dbContext, ninVersion, new VarietyCodeCode { Id = "besys0" });
            
            dbContext.SaveChanges();

            _stopwatch.Stop();
            Console.WriteLine($"Added NiN-variety version {ninVersion.Navn} in {_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }

        public static void FixLandform(ServiceProvider serviceProvider, string basePath, string version)
        {
            var dbContext = serviceProvider.GetService<NiNDbContext>();
            var ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));

            var path = Path.Combine(basePath, $"import_landform_v{version}.csv");

            var records = GetLandformRecords(path);

            var i = 0;
            foreach (var @record in records)
            {
                i++;
                VariasjonKode variasjonKode = null;
                VarietyLevel1 varietyLevel1 = null;
                VarietyLevel2 varietyLevel2 = null;
                VarietyLevel3 varietyLevel3 = null;
                VarietyLevel4 varietyLevel4 = null;

                var kodeName = $"{record.Besys}{record.Nivaa1Kode.Trim()}";

                variasjonKode = dbContext.VariasjonKode
                    .FirstOrDefault(x =>
                        x.Version.Id == ninVersion.Id &&
                        x.KodeName.Equals(kodeName));

                if (variasjonKode == null)
                {
                    variasjonKode = dbContext.VariasjonKode
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.KodeName.Equals($"BeSys{record.Besys}"));
                    if (variasjonKode == null) continue;

                    if (variasjonKode.VarietyCategory == VarietyEnum.VarietyLevel1)
                    {
                        var varietyKodeLevel1 = (VarietyLevel1Code)variasjonKode;

                        varietyLevel1 = dbContext.VarietyLevel1s
                            .FirstOrDefault(x => x.Id == varietyKodeLevel1.VarietyLevelId);

                        //Console.WriteLine($"{i:#000} {kodeName} - null");
                        varietyLevel2 = new VarietyLevel2
                        {
                            Version = ninVersion,
                            OverordnetKode = varietyLevel1,
                            Navn = record.Nivaa1Navn,
                            Kode = new VarietyLevel2Code
                            {
                                Version = ninVersion,
                                KodeName = kodeName
                            }
                        };
                        dbContext.VarietyLevel2s.Add(varietyLevel2);

                        dbContext.SaveChanges();

                        Console.WriteLine($"{i:#000} {kodeName}\t{record.Nivaa1Navn}");
                    }
                }

                varietyLevel2 = dbContext.VarietyLevel2s
                    .Include(x => x.Kode)
                    .FirstOrDefault(x =>
                        x.Version.Id == ninVersion.Id &&
                        x.Kode.KodeName.Equals(kodeName));

                kodeName = $"{record.SammensattKode.Trim()}";
                //Console.WriteLine($"{i:#000} {kodeName}");
                variasjonKode = dbContext.VariasjonKode
                    .FirstOrDefault(x =>
                        x.Version.Id == ninVersion.Id &&
                        x.KodeName.Equals(kodeName));

                if (variasjonKode != null) continue;

                if (!record.SammensattKode.Contains(record.Nivaa2Kode))
                {
                    //var shit = 'k';
                    //varietyLevel3 = dbContext.VarietyLevel3s
                    //    .Include(x => x.Kode)
                    //    .Include(x => x.UnderordnetKoder)
                    //    .FirstOrDefault(x =>
                    //        x.Version.Id == ninVersion.Id &&
                    //        x.Kode.KodeName.Equals(varietyLevel2.Kode.KodeName));
                    //if (varietyLevel2.UnderordnetKoder.Any())
                    //{
                    //    foreach (var level3 in varietyLevel2.UnderordnetKoder)
                    //    {
                    //        dbContext.VarietyLevel3s.Remove(level3);
                    //    }

                    //    varietyLevel2.UnderordnetKoder.Clear();
                    //    dbContext.SaveChanges();
                    //    continue;
                    //}
                    //continue;
                    varietyLevel4 = dbContext.VarietyLevel4s
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(kodeName));
                    
                    if (varietyLevel4 != null) continue;

                    varietyLevel3 = dbContext.VarietyLevel3s
                        .Include(x => x.Kode)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(kodeName.Substring(0,
                                kodeName.LastIndexOf("-", StringComparison.Ordinal))));

                    varietyLevel4 = new VarietyLevel4
                    {
                        Version = ninVersion,
                        OverordnetKode = varietyLevel3,
                        Navn = record.Navn,
                        Kode = new VarietyLevel4Code
                        {
                            Version = ninVersion,
                            KodeName = kodeName
                        }
                    };

                    dbContext.VarietyLevel4s.Add(varietyLevel4);

                    dbContext.SaveChanges();

                    Console.WriteLine($"{i:#000} {kodeName}\t{record.Navn}");

                    continue;
                }


                varietyLevel3 = new VarietyLevel3
                {
                    Version = ninVersion,
                    OverordnetKode = varietyLevel2,
                    Navn = record.Navn,
                    Kode = new VarietyLevel3Code
                    {
                        Version = ninVersion,
                        KodeName = kodeName
                    }
                };

                dbContext.VarietyLevel3s.Add(varietyLevel3);

                dbContext.SaveChanges();

                Console.WriteLine($"{i:#000} {kodeName}\t{record.Navn}");
            }
        }

        #region private methods

        private static void AddVarietyLevel0(IVarietyService varietyService,
                                             NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             VarietyCodeCode varietyCodeCode)
        {
            var variety = varietyService.GetVariety(varietyCodeCode.Id);

            var varietyLevel = new VarietyLevel0
            {
                Version = ninVersion,
                Navn = variety.Name.Trim(),
                Kode = new VarietyLevel0Code
                {
                    Version = ninVersion,
                    KodeName = variety.Code.Id.Trim()
                }
            };
            dbContext.VarietyLevel0s.Add(varietyLevel);

            AddVarietyLevel1(varietyService, dbContext, ninVersion, variety, varietyLevel);
        }

        private static void AddVarietyLevel1(IVarietyService varietyService,
                                             NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             VarietyCode varietyCode,
                                             VarietyLevel0 parentVarietyLevel)
        {
            if (varietyCode.UnderordnetKoder == null) return;

            foreach (var child in varietyCode.UnderordnetKoder)
            {
                var variety = varietyService.GetVariety(child.Id);

                var varietyLevel = new VarietyLevel1
                {
                    Version = ninVersion,
                    OverordnetKode = parentVarietyLevel,
                    Navn = variety.Name.Trim(),
                    Kode = new VarietyLevel1Code
                    {
                        Version = ninVersion,
                        KodeName = variety.Code.Id.Trim()
                    }
                };
                dbContext.VarietyLevel1s.Add(varietyLevel);

                AddVarietyLevel2(varietyService, dbContext, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel2(IVarietyService varietyService,
                                             NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             VarietyCode varietyCode,
                                             VarietyLevel1 parentVarietyLevel)
        {
            if (varietyCode.UnderordnetKoder == null) return;

            foreach (var child in varietyCode.UnderordnetKoder)
            {
                var variety = varietyService.GetVariety(child.Id);

                var varietyLevel = new VarietyLevel2
                {
                    Version = ninVersion,
                    OverordnetKode = parentVarietyLevel,
                    Navn = variety.Name.Trim(),
                    Kode = new VarietyLevel2Code
                    {
                        Version = ninVersion,
                        KodeName = variety.Code.Id.Trim()
                    }
                };
                dbContext.VarietyLevel2s.Add(varietyLevel);

                AddVarietyLevel3(varietyService, dbContext, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel3(IVarietyService varietyService,
                                             NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             VarietyCode varietyCode,
                                             VarietyLevel2 parentVarietyLevel)
        {
            if (varietyCode.UnderordnetKoder == null) return;

            foreach (var child in varietyCode.UnderordnetKoder)
            {
                var variety = varietyService.GetVariety(child.Id);

                var varietyLevel = new VarietyLevel3
                {
                    Version = ninVersion,
                    OverordnetKode = parentVarietyLevel,
                    Navn = variety.Name.Trim(),
                    Kode = new VarietyLevel3Code
                    {
                        Version = ninVersion,
                        KodeName = variety.Code.Id.Trim()
                    }
                };
                dbContext.VarietyLevel3s.Add(varietyLevel);

                AddVarietyLevel4(varietyService, dbContext, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel4(IVarietyService varietyService,
                                             NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             VarietyCode varietyCode,
                                             VarietyLevel3 parentVarietyLevel)
        {
            if (varietyCode.UnderordnetKoder == null) return;

            foreach (var child in varietyCode.UnderordnetKoder)
            {
                var variety = varietyService.GetVariety(child.Id);

                var varietyLevel = new VarietyLevel4
                {
                    Version = ninVersion,
                    OverordnetKode = parentVarietyLevel,
                    Navn = variety.Name.Trim(),
                    Kode = new VarietyLevel4Code
                    {
                        Version = ninVersion,
                        KodeName = variety.Code.Id.Trim()
                    }
                };
                dbContext.VarietyLevel4s.Add(varietyLevel);

                AddVarietyLevel5(varietyService, dbContext, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel5(IVarietyService varietyService,
                                             NiNDbContext dbContext,
                                             NinVersion ninVersion,
                                             VarietyCode varietyCode,
                                             VarietyLevel4 parentVarietyLevel)
        {
            if (varietyCode.UnderordnetKoder == null) return;

            foreach (var child in varietyCode.UnderordnetKoder)
            {
                var variety = varietyService.GetVariety(child.Id);

                var varietyLevel = new VarietyLevel5
                {
                    Version = ninVersion,
                    OverordnetKode = parentVarietyLevel,
                    Navn = variety.Name.Trim(),
                    Kode = new VarietyLevel5Code
                    {
                        Version = ninVersion,
                        KodeName = variety.Code.Id.Trim()
                    }
                };
                dbContext.VarietyLevel5s.Add(varietyLevel);

                if (variety.UnderordnetKoder == null) return;

                throw new NotImplementedException($"'{variety.Code.Id}: {variety.Name}'");
            }
        }

        private static IEnumerable<LandformRecord> GetLandformRecords(string path)
        {
            if (!File.Exists(path)) yield break;

            var csvConfiguration = new CsvConfiguration(new CultureInfo("nb-NO"));

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, csvConfiguration);

            var records = csv.GetRecords<LandformRecord>();
            foreach (var record in records)
            {
                yield return record;
            }
        }

        #endregion
    }
}
