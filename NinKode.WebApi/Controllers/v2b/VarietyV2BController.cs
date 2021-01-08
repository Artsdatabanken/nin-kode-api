namespace NinKode.WebApi.Controllers.v2b
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Models.Variety;

    [ApiController]
    [Route("v2b/variasjon")]
    public class VarietyV2BController : ControllerBase
    {
        [HttpGet]
        [Route("allekoder")]
        public async Task<VarietyAllCodes[]> GetAll()
        {
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/variasjon/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<VarietyAllCodes[]>(await response.Content.ReadAsStringAsync());
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public async Task<VarietyCode> GetCode(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/variasjon/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            var allCodes = JsonSerializer.Deserialize<VarietyAllCodes[]>(await response.Content.ReadAsStringAsync());
            var code = GetVariasjonCodeById(allCodes, id);

            if (code == null) return null;

            uriBuilder = new UriBuilder(code.Definition);
            response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<VarietyCode>(await response.Content.ReadAsStringAsync()); //.ToJson();
        }

        private static VarietyAllCodesCode GetVariasjonCodeById(VarietyAllCodes[] codes, string id)
        {
            foreach (var code in codes)
            {
                if (code.Code != null && code.Code.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) return code.Code;
                if (!string.IsNullOrEmpty(code.OverordnetKode.Id) && code.OverordnetKode.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) return code.OverordnetKode;
                if (code.UnderordnetKoder == null) continue;
                var internalCode = GetVariasjonCodeById(code.UnderordnetKoder, id);
                if (internalCode != null) return internalCode;
            }

            return null;
        }

        private static VarietyAllCodesCode GetVariasjonCodeById(VarietyAllCodesCode[] codes, string id)
        {
            return codes.FirstOrDefault(code => code.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }
    }
}
