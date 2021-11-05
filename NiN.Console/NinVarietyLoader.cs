﻿namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety;
    using NiN.Database.Models.Variety.Codes;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Services.v21;
    using NinKode.Database.Services.v21b;
    using NinKode.Database.Services.v22;

    public static class NinVarietyLoader
    {
        private static Stopwatch _stopwatch = new();

        public static void CreateVarietyDatabase(string version)
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

                AddVarietyLevel0(varietyService, context, ninVersion, new VarietyCodeCode { Id = "besys0" });
                
                context.SaveChanges();

                _stopwatch.Stop();
                Console.WriteLine($"Added NiN-variety version {ninVersion.Navn} in {_stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            }
        }

        #region private methods

        private static void AddVarietyLevel0(IVarietyService varietyService,
                                             NiNContext context,
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
            context.VarietyLevel0s.Add(varietyLevel);

            AddVarietyLevel1(varietyService, context, ninVersion, variety, varietyLevel);
        }

        private static void AddVarietyLevel1(IVarietyService varietyService,
                                             NiNContext context,
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
                context.VarietyLevel1s.Add(varietyLevel);

                AddVarietyLevel2(varietyService, context, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel2(IVarietyService varietyService,
                                             NiNContext context,
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
                context.VarietyLevel2s.Add(varietyLevel);

                AddVarietyLevel3(varietyService, context, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel3(IVarietyService varietyService,
                                             NiNContext context,
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
                context.VarietyLevel3s.Add(varietyLevel);

                AddVarietyLevel4(varietyService, context, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel4(IVarietyService varietyService,
                                             NiNContext context,
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
                context.VarietyLevel4s.Add(varietyLevel);

                AddVarietyLevel5(varietyService, context, ninVersion, variety, varietyLevel);
            }
        }

        private static void AddVarietyLevel5(IVarietyService varietyService,
                                             NiNContext context,
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
                context.VarietyLevel5s.Add(varietyLevel);

                if (variety.UnderordnetKoder == null) return;

                throw new NotImplementedException($"'{variety.Code.Id}: {variety.Name}'");
            }
        }

        #endregion
    }
}
