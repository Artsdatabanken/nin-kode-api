namespace NinKode.Database.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NiN.Database.Models.Variety;
    using NiN.Database.Models.Variety.Codes;
    using NiN.Database.Models.Variety.Enums;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Extension;
    using NinKode.Database.Model.v22;

    public class VarietyService : IVarietyService
    {
        private const string DbConnString = "NiNConnectionString";

        private readonly NiNContext _context;

        public VarietyService(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue(DbConnString, "");

            _context = string.IsNullOrEmpty(connectionString) ? new NiNContext() : new NiNContext(connectionString);
        }

        public IEnumerable<VarietyAllCodes> GetAll(string host, string version = "")
        {
            var list = new List<VarietyAllCodes>();

            return list;
        }

        public VarietyCode GetByKode(string id, string host, string version = "")
        {
            if (string.IsNullOrEmpty(id)) return null;

            var ninVersion = _context.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
            if (ninVersion == null) return null;

            id = id.Replace(" ", "_");

            var variety = _context.VariasjonKode
                .FirstOrDefault(x => x.Version.Id == ninVersion.Id && x.KodeName.Equals(id));

            if (variety == null) return null;

            VarietyCode varietyCode = null;

            switch (variety.VarietyCategory)
            {
                case VarietyEnum.VarietyLevel0:
                    var v0 = _context.VarietyLevel0s
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == variety.Id);

                    if (v0 == null) return null;

                    varietyCode = new VarietyCode
                    {
                        Name = v0.Navn,
                        Code = ConvertVarietyCode(v0.Kode, host),
                        UnderordnetKoder = CreateUnderordnetKoder(v0.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel1:
                    var v1 = _context.VarietyLevel1s
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
                        UnderordnetKoder = CreateUnderordnetKoder(v1.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel2:
                    var v2 = _context.VarietyLevel2s
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
                        UnderordnetKoder = CreateUnderordnetKoder(v2.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel3:
                    var v3 = _context.VarietyLevel3s
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
                        UnderordnetKoder = CreateUnderordnetKoder(v3.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel4:
                    var v4 = _context.VarietyLevel4s
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
                        UnderordnetKoder = CreateUnderordnetKoder(v4.UnderordnetKoder, host)
                    };
                    break;

                case VarietyEnum.VarietyLevel5:
                    var v5 = _context.VarietyLevel5s
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

        private static VarietyCodeCode ConvertVarietyCode(VariasjonKode kode, string host)
        {
            return new VarietyCodeCode
            {
                Id = kode.KodeName,
                Definition = $"{host}{kode.KodeName.Replace(" ", "_")}"
            };
        }

        private VarietyCodeCode[] CreateUnderordnetKoder(ICollection<VarietyLevel1> koder, string host)
        {
            if (!koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = _context.VarietyLevel1s
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

        private VarietyCodeCode[] CreateUnderordnetKoder(ICollection<VarietyLevel2> koder, string host)
        {
            if (!koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = _context.VarietyLevel2s
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

        private VarietyCodeCode[] CreateUnderordnetKoder(ICollection<VarietyLevel3> koder, string host)
        {
            if (!koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = _context.VarietyLevel3s
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

        private VarietyCodeCode[] CreateUnderordnetKoder(ICollection<VarietyLevel4> koder, string host)
        {
            if (!koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var varietyLevel5 in koder)
            {
                var k = _context.VarietyLevel4s
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

        private VarietyCodeCode[] CreateUnderordnetKoder(ICollection<VarietyLevel5> koder, string host)
        {
            if (!koder.Any()) return null;

            var list = new List<VarietyCodeCode>();

            foreach (var kode in koder)
            {
                var k = _context.VarietyLevel5s
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
