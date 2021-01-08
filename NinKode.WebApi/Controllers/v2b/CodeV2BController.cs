namespace NinKode.WebApi.Controllers.v2b
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Models.Code;

    [ApiController]
    [Route("v2b/koder")]
    public class CodeV2BController : ControllerBase
    {
        [HttpGet]
        [Route("allekoder")]
        public async Task<Codes[]> GetAll()
        {
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/koder/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<Codes[]>(await response.Content.ReadAsStringAsync()); //.ToJson();
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public async Task<Codes> GetCode(string id)
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

            return JsonSerializer.Deserialize<Codes>(await response.Content.ReadAsStringAsync()); //.ToJson();
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
