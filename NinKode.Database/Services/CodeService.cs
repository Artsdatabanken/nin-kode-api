﻿namespace NinKode.Database.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.EntityFrameworkCore;
    using NiN.Database.Converters;
    using NiN.Database;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Extension;

    public class CodeService : ICodeService
    {
        public IEnumerable<Codes> GetAll(NiNDbContext context, string host, string version = "")
        {
            var list = new List<Codes>();

            return list;
        }

        public Codes GetByKode(NiNDbContext context, string id, string host, string version = "")
        {
            if (string.IsNullOrEmpty(id)) return null;

            var ninVersion = context.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
            if (ninVersion == null) return null;

            id = id.Replace("_", " ");

            var kode = context.Kode
                .FirstOrDefault(x => x.Version.Id == ninVersion.Id && x.KodeName.Equals(id));

            if (kode == null) return null;

            Codes code = null;

            switch (kode.Kategori)
            {
                case KategoriEnum.Naturmangfoldnivå:
                    var natursystem = context.Natursystem
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == kode.Id);

                    if (natursystem == null) return null;

                    code = new Codes
                    {
                        Navn = natursystem.Navn,
                        Kategori = natursystem.Kategori,
                        Kode = ConvertNinKode2Code(natursystem.Kode, host)
                    };

                    if (natursystem.UnderordnetKoder.Any())
                    {
                        code.UnderordnetKoder = CreateUnderordnetKoder(context, natursystem.UnderordnetKoder, host);
                    }

                    break;

                case KategoriEnum.Hovedtypegruppe:
                    var hovedtypegruppe = context.Hovedtypegruppe
                        .Include(x => x.Natursystem)
                        .Include(x => x.Natursystem.Kode)
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == kode.Id);

                    if (hovedtypegruppe == null) break;

                    code = new Codes
                    {
                        Navn = hovedtypegruppe.Navn,
                        Kategori = hovedtypegruppe.Kategori,
                        Kode = ConvertNinKode2Code(hovedtypegruppe.Kode, host),
                        OverordnetKode = ConvertNinKode2Code(hovedtypegruppe.Natursystem.Kode, host)
                    };

                    if (hovedtypegruppe.UnderordnetKoder.Any())
                    {
                        code.UnderordnetKoder = CreateUnderordnetKoder(context, hovedtypegruppe.UnderordnetKoder, host);
                    }

                    break;

                case KategoriEnum.Hovedtype:
                    var hovedtype = context.Hovedtype
                        .Include(x => x.Hovedtypegruppe)
                        .Include(x => x.Hovedtypegruppe.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .Include(x => x.Kartleggingsenheter)
                        .Include(x => x.Miljovariabler)
                        .FirstOrDefault(x => x.Kode.Id == kode.Id);

                    if (hovedtype == null) break;

                    code = new Codes
                    {
                        Navn = hovedtype.Navn,
                        Kategori = hovedtype.Kategori,
                        Kode = ConvertNinKode2Code(hovedtype.Kode, host),
                        OverordnetKode = ConvertNinKode2Code(hovedtype.Hovedtypegruppe.Kode, host)
                    };

                    if (hovedtype.UnderordnetKoder.Any())
                    {
                        code.UnderordnetKoder = CreateUnderordnetKoder(context, hovedtype.UnderordnetKoder, host);
                    }

                    if (hovedtype.Kartleggingsenheter.Any())
                    {
                        code.Kartleggingsenheter = CreateKartleggingsenheter(context, hovedtype.Kartleggingsenheter, host);
                    }

                    if (hovedtype.Miljovariabler.Any())
                    {
                        code.Miljovariabler = CreateMiljovariabler(context, hovedtype.Miljovariabler);
                    }

                    break;

                case KategoriEnum.Grunntype:
                    var grunntype = context.Grunntype
                        .Include(x => x.Hovedtype)
                        .Include(x => x.Hovedtype.Kode)
                        .FirstOrDefault(x => x.Kode.Id == kode.Id);

                    if (grunntype == null) break;

                    code = new Codes
                    {
                        Navn = grunntype.Navn,
                        Kategori = grunntype.Kategori,
                        Kode = ConvertNinKode2Code(grunntype.Kode, host),
                        OverordnetKode = ConvertNinKode2Code(grunntype.Hovedtype.Kode, host)
                    };

                    break;

                case KategoriEnum.Kartleggingsenhet:
                    var kartlegging = context.Kartleggingsenhet
                        .Include(x => x.Hovedtype)
                        .Include(x => x.Hovedtype.Kode)
                        .FirstOrDefault(x => x.Kode.Id == kode.Id);

                    if (kartlegging == null) break;

                    code = new Codes
                    {
                        Navn = kartlegging.Definisjon,
                        Kategori = NinEnumConverter.GetValue<KategoriEnum>(kartlegging.Kode.Kategori),
                        Kode = ConvertNinKode2Code(kartlegging.Kode, host),
                        OverordnetKode = ConvertNinKode2Code(kartlegging.Hovedtype.Kode, host)
                    };

                    break;
            }

            return code;
        }

        public Codes GetCode(string id)
        {
            return null;
        }

        #region private methods

        private static AllCodesCode ConvertNinKode2Code(NinKode ninKode, string host)
        {
            return new AllCodesCode
            {
                Id = ninKode.KodeName,
                Definition = $"{host}{ninKode.KodeName.Replace(" ", "_")}"
            };
        }

        private EnvironmentVariable[] CreateMiljovariabler(NiNDbContext context, IEnumerable<Miljovariabel> entities)
        {
            var variables = new List<EnvironmentVariable>();

            foreach (var m in entities)
            {
                var miljovariabel = context.Miljovariabel
                    .Include(x => x.Kode)
                    .Include(x => x.Trinn)
                    .FirstOrDefault(x => x.Id == m.Id);

                if (miljovariabel == null) continue;

                variables.Add(new EnvironmentVariable
                {
                    Kode = miljovariabel.Kode.Kode,
                    LKMKategori = miljovariabel.LkmKategori,
                    Navn = miljovariabel.Navn,
                    Type = miljovariabel.Type,
                    Trinn = CreateTrinn(context, miljovariabel.Trinn)
                });
            }

            return variables.OrderBy(x => x.Kode).ToArray();
        }

        private Step[] CreateTrinn(NiNDbContext context, IEnumerable<Trinn> entities)
        {
            var steps = new List<Step>();

            foreach (var t in entities)
            {
                var trinn = context.Trinn
                    .Include(x => x.Kode)
                    .Include(x => x.Basistrinn)
                    .FirstOrDefault(x => x.Id == t.Id);

                if (trinn == null) continue;

                steps.Add(new Step
                {
                    Navn = trinn.Navn,
                    Kode = trinn.Kode.KodeName,
                    Basistrinn = string.Join(", ", trinn.Basistrinn.Select(x => x.Navn).ToList().OrderByList<IList<string>>())
                });
            }

            return steps.OrderBy(x => x.Kode).ToArray();
        }

        private Dictionary<string, AllCodesCode[]> CreateKartleggingsenheter(NiNDbContext context, IEnumerable<Kartleggingsenhet> entities, string host)
        {
            var codes = new Dictionary<int, IList<AllCodesCode>>();

            foreach (var k in entities)
            {
                var kartleggingsenhet = context.Kartleggingsenhet
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == k.Id);

                if (kartleggingsenhet == null) continue;

                var enumValue = NinEnumConverter.GetValue<MalestokkEnum>(kartleggingsenhet.Malestokk);
                var scale = int.Parse(Regex.Match(enumValue, @"\d+").Value, NumberFormatInfo.InvariantInfo);
                if (!codes.ContainsKey(scale)) codes.Add(scale, new List<AllCodesCode>());

                codes[scale].Add(ConvertNinKode2Code(kartleggingsenhet.Kode, host));
            }

            return codes.ToDictionary(code => code.Key.ToString(), code => CreateOrderedList(code.Value));
        }

        private AllCodesCode[] CreateUnderordnetKoder(NiNDbContext context, IEnumerable<Hovedtypegruppe> entities, string host)
        {
            var codes = new List<AllCodesCode>();

            foreach (var g in entities)
            {
                var hovedtypegruppe = context.Hovedtypegruppe
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == g.Id);

                if (hovedtypegruppe == null) continue;

                codes.Add(ConvertNinKode2Code(hovedtypegruppe.Kode, host));
            }

            return CreateOrderedList(codes);
        }

        private AllCodesCode[] CreateUnderordnetKoder(NiNDbContext context, IEnumerable<Grunntype> entities, string host)
        {
            var codes = new List<AllCodesCode>();

            foreach (var g in entities)
            {
                var grunntype = context.Grunntype
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == g.Id);

                if (grunntype == null) continue;

                codes.Add(ConvertNinKode2Code(grunntype.Kode, host));
            }

            return CreateOrderedList(codes);
        }

        private AllCodesCode[] CreateUnderordnetKoder(NiNDbContext context, IEnumerable<Hovedtype> entities, string host)
        {
            var codes = new List<AllCodesCode>();

            foreach (var h in entities)
            {
                var hovedtype = context.Hovedtype
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == h.Id);

                if (hovedtype == null) continue;

                codes.Add(ConvertNinKode2Code(hovedtype.Kode, host));
            }

            return CreateOrderedList(codes);
        }

        private static AllCodesCode[] CreateOrderedList(IEnumerable<AllCodesCode> codes)
        {
            var sorted = codes.ToList();
            sorted.Sort(new AllCodesCodeComparer());
            return sorted.ToArray();
        }

        #endregion
    }
}
