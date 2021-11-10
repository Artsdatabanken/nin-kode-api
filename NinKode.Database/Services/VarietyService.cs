namespace NinKode.Database.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using NiN.Database;
    using NiN.Database.Models.Variety;
    using NiN.Database.Models.Variety.Codes;
    using NiN.Database.Models.Variety.Enums;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Extension;

    public class VarietyService : IVarietyService
    {
        public IEnumerable<VarietyAllCodes> GetAll(NiNDbContext dbContext, string host, string version = "")
        {
            if (host.EndsWith("variasjon/", StringComparison.OrdinalIgnoreCase)) host += "hentkode/";
            var varietyList = dbContext.VariasjonKode.Include(x => x.Version).Where(x => x.Version.Navn.Equals(version)).ToList();
            foreach (var variety in varietyList)
            {
                VarietyAllCodes codes = null;

                switch (variety.VarietyCategory)
                {
                    case VarietyEnum.VarietyLevel0:
                        var v0 = dbContext.VarietyLevel0s
                            .Include(x => x.Kode)
                            //.Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Kode.Id == variety.Id);

                        if (v0 == null) continue;

                        codes = CreateVarietyAllCodes(v0, host);

                        break;

                    case VarietyEnum.VarietyLevel1:
                        var v1 = dbContext.VarietyLevel1s
                            .Include(x => x.Kode)
                            .Include(x => x.OverordnetKode.Kode)
                            //.Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Kode.Id == variety.Id);

                        if (v1 == null) continue;

                        codes = CreateVarietyAllCodes(v1, host);

                        break;

                    case VarietyEnum.VarietyLevel2:
                        var v2 = dbContext.VarietyLevel2s
                            .Include(x => x.Kode)
                            .Include(x => x.OverordnetKode.Kode)
                            //.Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Kode.Id == variety.Id);

                        if (v2 == null) continue;

                        codes = CreateVarietyAllCodes(v2, host);

                        break;

                    case VarietyEnum.VarietyLevel3:
                        var v3 = dbContext.VarietyLevel3s
                            .Include(x => x.Kode)
                            .Include(x => x.OverordnetKode.Kode)
                            //.Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Kode.Id == variety.Id);

                        if (v3 == null) continue;

                        codes = CreateVarietyAllCodes(v3, host);

                        break;

                    case VarietyEnum.VarietyLevel4:
                        var v4 = dbContext.VarietyLevel4s
                            .Include(x => x.Kode)
                            .Include(x => x.OverordnetKode.Kode)
                            //.Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Kode.Id == variety.Id);

                        if (v4 == null) continue;

                        codes = CreateVarietyAllCodes(v4, host);

                        break;

                    case VarietyEnum.VarietyLevel5:
                        var v5 = dbContext.VarietyLevel5s
                            .Include(x => x.Kode)
                            .Include(x => x.OverordnetKode.Kode)
                            //.Include(x => x.UnderordnetKoder)
                            .FirstOrDefault(x => x.Kode.Id == variety.Id);

                        if (v5 == null) continue;

                        codes = CreateVarietyAllCodes(v5, host);

                        break;
                }

                if (codes == null) continue;

                yield return codes;
            }
        }

        public VarietyCode GetByKode(NiNDbContext dbContext, string id, string host, string version = "")
        {
            if (string.IsNullOrEmpty(id)) return null;

            var ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
            if (ninVersion == null) return null;

            id = id.Replace(" ", "_");

            var variety = dbContext.VariasjonKode
                .FirstOrDefault(x => x.Version.Id == ninVersion.Id && x.KodeName.Equals(id));

            if (variety == null) return null;

            VarietyCode varietyCode = null;

            switch (variety.VarietyCategory)
            {
                case VarietyEnum.VarietyLevel0:
                    var v0 = dbContext.VarietyLevel0s
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v0 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v0.Navn,
                        Code = ConvertVarietyCode(v0.Kode, host),
                        UnderordnetKoder = CreateUnderordnetKoder(dbContext, v0.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel1:
                    var v1 = dbContext.VarietyLevel1s
                        .Include(x => x.Kode)
                        .Include(x => x.OverordnetKode.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v1 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v1.Navn,
                        Code = ConvertVarietyCode(v1.Kode, host),
                        OverordnetKode = new VarietyCodeCode
                        {
                            Id = v1.OverordnetKode.Kode.KodeName,
                            Definition = $"{host}{v1.OverordnetKode.Kode.KodeName}"
                        },
                        UnderordnetKoder = CreateUnderordnetKoder(dbContext, v1.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel2:
                    var v2 = dbContext.VarietyLevel2s
                        .Include(x => x.Kode)
                        .Include(x => x.OverordnetKode.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v2 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v2.Navn,
                        Code = ConvertVarietyCode(v2.Kode, host),
                        OverordnetKode = new VarietyCodeCode
                        {
                            Id = v2.OverordnetKode.Kode.KodeName,
                            Definition = $"{host}{v2.OverordnetKode.Kode.KodeName}"
                        },
                        UnderordnetKoder = CreateUnderordnetKoder(dbContext, v2.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel3:
                    var v3 = dbContext.VarietyLevel3s
                        .Include(x => x.Kode)
                        .Include(x => x.OverordnetKode.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v3 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v3.Navn,
                        Code = ConvertVarietyCode(v3.Kode, host),
                        OverordnetKode = new VarietyCodeCode
                        {
                            Id = v3.OverordnetKode.Kode.KodeName,
                            Definition = $"{host}{v3.OverordnetKode.Kode.KodeName}"
                        },
                        UnderordnetKoder = CreateUnderordnetKoder(dbContext, v3.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel4:
                    var v4 = dbContext.VarietyLevel4s
                        .Include(x => x.Kode)
                        .Include(x => x.OverordnetKode.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v4 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v4.Navn,
                        Code = ConvertVarietyCode(v4.Kode, host),
                        OverordnetKode = new VarietyCodeCode
                        {
                            Id = v4.OverordnetKode.Kode.KodeName,
                            Definition = $"{host}{v4.OverordnetKode.Kode.KodeName}"
                        },
                        UnderordnetKoder = CreateUnderordnetKoder(dbContext, v4.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel5:
                    var v5 = dbContext.VarietyLevel5s
                        .Include(x => x.Kode)
                        .Include(x => x.OverordnetKode.Kode)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v5 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v5.Navn,
                        Code = ConvertVarietyCode(v5.Kode, host),
                        OverordnetKode = new VarietyCodeCode
                        {
                            Id = v5.OverordnetKode.Kode.KodeName,
                            Definition = $"{host}{v5.OverordnetKode.Kode.KodeName}"
                        }
                    };
                    break;
            }

            return varietyCode;
        }

        public VarietyCode GetVariety(string id)
        {
            throw new NotImplementedException();
        }

        #region private methods

        private static VarietyAllCodes CreateVarietyAllCodes(VarietyLevel0 variety, string host)
        {
            if (variety == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variety.Kode.KodeName, host),
                Name = variety.Navn
            };
        }

        private static VarietyAllCodes CreateVarietyAllCodes(VarietyLevel1 variety, string host)
        {
            if (variety == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variety.Kode.KodeName, host),
                Name = variety.Navn,
                OverordnetKode = CreateVarietyAllCodesCode(variety.OverordnetKode.Kode.KodeName, host)
            };
        }

        private static VarietyAllCodes CreateVarietyAllCodes(VarietyLevel2 variety, string host)
        {
            if (variety == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variety.Kode.KodeName, host),
                Name = variety.Navn,
                OverordnetKode = CreateVarietyAllCodesCode(variety.OverordnetKode.Kode.KodeName, host)
            };
        }

        private static VarietyAllCodes CreateVarietyAllCodes(VarietyLevel3 variety, string host)
        {
            if (variety == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variety.Kode.KodeName, host),
                Name = variety.Navn,
                OverordnetKode = CreateVarietyAllCodesCode(variety.OverordnetKode.Kode.KodeName, host)
            };
        }

        private static VarietyAllCodes CreateVarietyAllCodes(VarietyLevel4 variety, string host)
        {
            if (variety == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variety.Kode.KodeName, host),
                Name = variety.Navn,
                OverordnetKode = CreateVarietyAllCodesCode(variety.OverordnetKode.Kode.KodeName, host)
            };
        }

        private static VarietyAllCodes CreateVarietyAllCodes(VarietyLevel5 variety, string host)
        {
            if (variety == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variety.Kode.KodeName, host),
                Name = variety.Navn,
                OverordnetKode = CreateVarietyAllCodesCode(variety.OverordnetKode.Kode.KodeName, host)
            };
        }

        private static VarietyAllCodesCode CreateVarietyAllCodesCode(string kode, string host)
        {
            if (string.IsNullOrEmpty(kode)) return null;

            return new VarietyAllCodesCode
            {
                Id = kode,
                Definition = $"{host}{kode.Replace(" ", "_")}"
            };
        }

        private static VarietyCodeCode ConvertVarietyCode(VariasjonKode kode, string host)
        {
            return new VarietyCodeCode
            {
                Id = kode.KodeName,
                Definition = $"{host}{kode.KodeName.Replace(" ", "_")}"
            };
        }

        private VarietyCodeCode[] CreateUnderordnetKoder(NiNDbContext dbContext, ICollection<VarietyLevel1> koder, string host)
        {
            if (koder == null || !koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = dbContext.VarietyLevel1s
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == kode.Id);

                if (k == null) continue;

                list.Add(new VarietyCodeCode
                {
                    Id = k.Kode.KodeName,
                    Definition = $"{host}{k.Kode.KodeName.Replace(" ", "_")}"
                });
            }

            return CreateOrderedList(list);
        }

        private VarietyCodeCode[] CreateUnderordnetKoder(NiNDbContext dbContext, ICollection<VarietyLevel2> koder, string host)
        {
            if (koder == null || !koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = dbContext.VarietyLevel2s
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == kode.Id);

                if (k == null) continue;

                list.Add(new VarietyCodeCode
                {
                    Id = k.Kode.KodeName,
                    Definition = $"{host}{k.Kode.KodeName.Replace(" ", "_")}"
                });
            }

            return CreateOrderedList(list);
        }

        private VarietyCodeCode[] CreateUnderordnetKoder(NiNDbContext dbContext, ICollection<VarietyLevel3> koder, string host)
        {
            if (koder == null || !koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = dbContext.VarietyLevel3s
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == kode.Id);

                if (k == null) continue;

                list.Add(new VarietyCodeCode
                {
                    Id = k.Kode.KodeName,
                    Definition = $"{host}{k.Kode.KodeName.Replace(" ", "_")}"
                });
            }

            return CreateOrderedList(list);
        }

        private VarietyCodeCode[] CreateUnderordnetKoder(NiNDbContext dbContext, ICollection<VarietyLevel4> koder, string host)
        {
            if (koder == null || !koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var varietyLevel5 in koder)
            {
                var k = dbContext.VarietyLevel4s
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == varietyLevel5.Id);

                if (k == null) continue;

                list.Add(new VarietyCodeCode
                {
                    Id = k.Kode.KodeName,
                    Definition = $"{host}{k.Kode.KodeName.Replace(" ", "_")}"
                });
            }

            return CreateOrderedList(list);
        }

        private VarietyCodeCode[] CreateUnderordnetKoder(NiNDbContext dbContext, ICollection<VarietyLevel5> koder, string host)
        {
            if (koder == null || !koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = dbContext.VarietyLevel5s
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == kode.Id);

                if (k == null) continue;

                list.Add(new VarietyCodeCode
                {
                    Id = k.Kode.KodeName,
                    Definition = $"{host}{k.Kode.KodeName.Replace(" ", "_")}"
                });
            }

            return CreateOrderedList(list);
        }

        private static VarietyCodeCode[] CreateOrderedList(IEnumerable<VarietyCodeCode> codes)
        {
            var sorted = codes.ToList();
            sorted.Sort(new VarietyCodeCodeComparer());
            return sorted.ToArray();
        }

        #endregion
    }
}
