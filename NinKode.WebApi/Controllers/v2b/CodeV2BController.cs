namespace NinKode.WebApi.Controllers.v2b
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Models.Code;
    using NinKode.Database.Model.v2b;
    using NinKode.Database.Service.v2b;
    using Environment = NinKode.Common.Models.Code.Environment;

    [ApiController]
    [Route("v2b/koder")]
    public class CodeV2BController : ControllerBase
    {
        private const string Url = "http://localhost:8080/";
        private const string DefaultDatabase = "SOSINiNv2.0b";
        private readonly CodeV2BService _codeService;

        public CodeV2BController()
        {
            _codeService = new CodeV2BService(Url, DefaultDatabase);
        }

        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<Codes> GetAll()
        {
            var naturTyper = _codeService.GetAll();

            //var list = new List<Codes>();
            foreach (var naturType in naturTyper)
            {
                yield return CreateCodesByNaturtype(naturType);
                //list.Add(CreateCodesByNaturtype(naturType));
            }

            //return list;

            //var client = new HttpClient();
            //var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/koder/Allekoder");
            //var response = await client.GetAsync(uriBuilder.Uri);
            //response.EnsureSuccessStatusCode();

            //return JsonSerializer.Deserialize<Codes[]>(await response.Content.ReadAsStringAsync()); //.ToJson();
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public Codes GetCode(string id)
        {
            var naturType = _codeService.GetByKode(id);

            return naturType == null ? null : CreateCodesByNaturtype(naturType);

            //if (string.IsNullOrEmpty(id)) return null;
            //var client = new HttpClient();
            //var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/koder/Allekoder");
            //var response = await client.GetAsync(uriBuilder.Uri);
            //response.EnsureSuccessStatusCode();

            //var allCodes = JsonSerializer.Deserialize<Codes[]>(await response.Content.ReadAsStringAsync());
            ////var result = await response.Content.ReadAsStringAsync();
            ////var allCodes = JsonSerializer.Deserialize<Codes[]>(result);
            //var code = GetCodeById(allCodes, id);

            //if (code == null) return null;

            //uriBuilder = new UriBuilder(code.Definition);
            //response = await client.GetAsync(uriBuilder.Uri);
            //response.EnsureSuccessStatusCode();

            //return JsonSerializer.Deserialize<Codes>(await response.Content.ReadAsStringAsync()); //.ToJson();
        }

        private Codes CreateCodesByNaturtype(NaturTypeV2B naturType)
        {
            var path = Request.Path.Value;
            path = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var protocol = Request.IsHttps ? "s" : "";
            var codes = new Codes
            {
                Navn = naturType.Navn,
                Kategori = naturType.Kategori,
                Kode = new AllCodesCode
                {
                    Id = naturType.Kode,
                    Definition = $"http{protocol}://{Request.Host}{path}{naturType.Kode.Replace(" ", "_")}"
                },
                ElementKode = naturType.ElementKode,
                OverordnetKode = new AllCodesCode
                {
                    Id = naturType.OverordnetKode,
                    Definition = !string.IsNullOrEmpty(naturType.OverordnetKode) ? $"http{protocol}://{Request.Host}{path}{naturType.OverordnetKode.Replace(" ", "_")}" : ""
                },
                UnderordnetKoder = naturType.UnderordnetKoder == null ? null : CreateCodesByNaturtype(naturType.UnderordnetKoder).ToArray(),
                Kartleggingsenheter = naturType.Kartleggingsenheter == null ? null : CreateKartleggingsenheter(naturType.Kartleggingsenheter),
                Miljovariabler = naturType.Trinn == null ? null : CreateTrinn(naturType.Trinn).ToArray()
            };
            return codes;
        }

        private IEnumerable<Environment> CreateTrinn(IEnumerable<TrinnV2B> naturTypeTrinn)
        {
            return naturTypeTrinn.Select(x => new Environment
            {
                Kode = x.Kode,
                Navn = x.Navn,
                Type = x.Type,
                Trinn = x.Trinn == null ? null : CreateTrinn(x.Trinn).ToArray()
            });
        }

        private IEnumerable<Step> CreateTrinn(IEnumerable<SubTrinnV2B> naturTypeTrinn)
        {
            return naturTypeTrinn.Select(x => new Step
            {
                Kode = x.Kode,
                Navn = x.Navn
            });
        }

        private Dictionary<string, AllCodesCode[]> CreateKartleggingsenheter(IDictionary<string, string[]> kartleggingsenheter)
        {
            if (kartleggingsenheter == null) return null;

            var result = new Dictionary<string, AllCodesCode[]>();

            foreach (var kartlegging in kartleggingsenheter)
            {
                result.Add(kartlegging.Key, CreateCodesByNaturtype(kartlegging.Value).ToArray());
            }

            return result;
        }

        private IEnumerable<AllCodesCode> CreateCodesByNaturtype(string[] koder)
        {
            if (koder == null) yield break;

            var path = Request.Path.Value;
            path = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var protocol = Request.IsHttps ? "s" : "";
            foreach (var kode in koder)
            {
                yield return new AllCodesCode
                {
                    Id = kode,
                    Definition = $"http{protocol}://{Request.Host}{path}{kode.Replace(" ", "_")}"
                };
            }
        }

        [HttpGet]
        [Route("hentkode_/{id}")]
        public async Task<Codes> GetCode_(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/koder/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            var allCodes = JsonSerializer.Deserialize<Codes[]>(await response.Content.ReadAsStringAsync());
            var code = GetCodeById(allCodes, id);

            if (code == null) return null;

            uriBuilder = new UriBuilder(code.Definition);
            response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<Codes>(await response.Content.ReadAsStringAsync());
        }

        private static AllCodesCode GetCodeById(Codes[] codes, string id)
        {
            foreach (var code in codes)
            {
                if (code.Kode != null && code.Kode.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) return code.Kode;
                if (!string.IsNullOrEmpty(code.OverordnetKode.Id) && code.OverordnetKode.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) return code.OverordnetKode;
                if (code.UnderordnetKoder == null) continue;
                var internalCode = GetVariasjonCodeById(code.UnderordnetKoder, id);
                if (internalCode != null) return internalCode;
            }

            return null;
        }

        private static AllCodesCode GetVariasjonCodeById(AllCodesCode[] codes, string id)
        {
            return codes.FirstOrDefault(code => code.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }
    }
}
