namespace NinKode.Database.Services.v23
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NiN.Database.Converters;
    using NiN.Database.Models;
    using NiN.Database.Models.Enums;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Extension;
    using NinKode.Database.Model.v22;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Linq;
    using Hovedtype = NiN.Database.Models.Hovedtype;

    public class CodeV23Service : ICodeV23Service
    {
        private const string DbConnString = "NiNConnectionStringV23";

        private readonly DocumentStore _store;

        private readonly NiNContext _context;

        public CodeV23Service(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue(DbConnString, "data source=localhost;initial catalog=NiN_v23_test;Integrated Security=SSPI;MultipleActiveResultSets=True;App=EntityFramework");

            if (string.IsNullOrWhiteSpace(connectionString)) throw new Exception($"Missing \"{DbConnString}\"");

            _context = new NiNContext(connectionString);
        }

        public IEnumerable<Codes> GetAll(string host)
        {
            var list = new List<Codes>();

            return list;
        }

        public Codes GetByKode(string id, string host)
        {
            Codes code = null;

            if (string.IsNullOrEmpty(id)) return null;

            id = id.Replace("_", " ");

            var kode = _context.Kode.FirstOrDefault(x => x.KodeName.Equals(id));

            if (kode == null) return null;

            switch (kode.Kategori)
            {
                case KategoriEnum.Naturmangfoldnivå:
                    var natursystem = _context.Natursystem
                        .Include(x => x.Kode)
                        .Include(x => x.UnderordnetKoder)
                        .FirstOrDefault(x => x.Kode.Id == kode.Id);

                    if (natursystem == null) return null;

                    code = new Codes
                    {
                        Navn = natursystem.Navn,
                        Kategori = NinEnumConverter.GetValue<KategoriEnum>(natursystem.Kode.Kategori),
                        Kode = ConvertNinKode2Code(natursystem.Kode, host)
                    };

                    if (natursystem.UnderordnetKoder.Any())
                    {
                        code.UnderordnetKoder = CreateUnderordnetKoder(natursystem.UnderordnetKoder, host);
                    }

                    break;

                case KategoriEnum.Hovedtypegruppe:
                    var hovedtypegruppe = _context.Hovedtypegruppe
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
                        code.UnderordnetKoder = CreateUnderordnetKoder(hovedtypegruppe.UnderordnetKoder, host);
                    }

                    break;

                case KategoriEnum.Hovedtype:
                    var hovedtype = _context.Hovedtype
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
                        code.UnderordnetKoder = CreateUnderordnetKoder(hovedtype.UnderordnetKoder, host);
                    }

                    if (hovedtype.Kartleggingsenheter.Any())
                    {
                        code.Kartleggingsenheter = CreateKartleggingsenheter(hovedtype.Kartleggingsenheter, host);
                    }

                    if (hovedtype.Miljovariabler.Any())
                    {
                        code.Miljovariabler = CreateMiljovariabler(hovedtype.Miljovariabler, host);
                    }
                    
                    break;

                case KategoriEnum.Grunntype:
                    var grunntype = _context.Grunntype
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
                    var kartlegging = _context.Kartleggingsenhet
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

        #region private methods

        private static AllCodesCode ConvertNinKode2Code(NiN.Database.Models.Codes.Kode ninKode, string host)
        {
            return new AllCodesCode
            {
                Id = ninKode.KodeName,
                Definition = $"{host}{ninKode.KodeName.Replace(" ", "_")}"
            };
        }

        private EnvironmentVariable[] CreateMiljovariabler(ICollection<Miljovariabel> entities, string host)
        {
            var variables = new List<EnvironmentVariable>();

            foreach (var m in entities)
            {
                var miljovariabel = _context.Miljovariabel
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
                    Trinn = CreateTrinn(miljovariabel.Trinn)
                });
            }

            return variables.ToArray();
        }

        private Step[] CreateTrinn(ICollection<Trinn> entities)
        {
            var steps = new List<Step>();

            foreach (var t in entities)
            {
                var trinn = _context.Trinn
                    .Include(x => x.Kode)
                    .Include(x => x.Basistrinn)
                    .FirstOrDefault(x => x.Id == t.Id);

                if (trinn == null) continue;

                var trinns = "";
                foreach (var basistrinn in trinn.Basistrinn.OrderBy(x => x.Navn))
                {
                    var separator = trinns.Length > 0 ? "," : "";
                    trinns += $"{separator}{basistrinn.Navn}";
                }

                steps.Add(new Step
                {
                    Navn = trinn.Navn,
                    Kode = trinn.Kode.KodeName,
                    Basistrinn = trinns
                });
            }

            return steps.OrderBy(x => x.Kode).ToArray();
        }

        private Dictionary<string, AllCodesCode[]> CreateKartleggingsenheter(ICollection<Kartleggingsenhet> entities, string host)
        {
            var codes = new Dictionary<int, IList<AllCodesCode>>();
            foreach (var k in entities)
            {
                var kartleggingsenhet = _context.Kartleggingsenhet
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

        private AllCodesCode[] CreateUnderordnetKoder(ICollection<Hovedtypegruppe> entities, string host)
        {
            var codes = new List<AllCodesCode>();

            foreach (var g in entities)
            {
                var gu = _context.Grunntype
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == g.Id);

                if (gu == null) continue;

                codes.Add(ConvertNinKode2Code(gu.Kode, host));
            }

            return CreateOrderedList(codes);
        }

        private AllCodesCode[] CreateUnderordnetKoder(ICollection<Grunntype> entities, string host)
        {
            var codes = new List<AllCodesCode>();

            foreach (var g in entities)
            {
                var gu = _context.Grunntype
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == g.Id);

                if (gu == null) continue;

                codes.Add(ConvertNinKode2Code(gu.Kode, host));
            }

            return CreateOrderedList(codes);
        }

        private AllCodesCode[] CreateUnderordnetKoder(ICollection<Hovedtype> entities, string host)
        {
            var codes = new List<AllCodesCode>();

            foreach (var g in entities)
            {
                var gu = _context.Grunntype
                    .Include(x => x.Kode)
                    .FirstOrDefault(x => x.Id == g.Id);

                if (gu == null) continue;

                codes.Add(ConvertNinKode2Code(gu.Kode, host));
            }

            return CreateOrderedList(codes);
        }

        private static AllCodesCode[] CreateOrderedList(IList<AllCodesCode> codes)
        {
            var sortedCodes = codes.ToList();
            sortedCodes.Sort(new AllCodesCodeComparer());
            return sortedCodes.ToArray();
        }

        #endregion
    }
}
