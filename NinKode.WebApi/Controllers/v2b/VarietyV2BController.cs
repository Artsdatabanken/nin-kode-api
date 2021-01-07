namespace NinKode.WebApi.Controllers.v2b
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using NinKode.Common;

    [ApiController]
    [Route("v2b/variasjon")]
    public class VarietyV2BController : ControllerBase
    {
        [HttpGet]
        [Route("allekoder")]
        public async Task<VariasjonAllekoder[]> GetAll()
        {
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/variasjon/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<VariasjonAllekoder[]>(await response.Content.ReadAsStringAsync());
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public async Task<Kodegreier> GetCode(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/variasjon/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            var allCodes = JsonConvert.DeserializeObject<VariasjonAllekoder[]>(await response.Content.ReadAsStringAsync());
            var code = GetVariasjonCodeById(allCodes, id);

            if (code == null) return null;

            uriBuilder = new UriBuilder(code.Definisjon);
            response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Kodegreier>(await response.Content.ReadAsStringAsync());
        }

        public VariasjonKode GetVariasjonCodeById(VariasjonAllekoder[] codes, string id)
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

        public VariasjonKode GetVariasjonCodeById(VariasjonKode[] codes, string id)
        {
            return codes.FirstOrDefault(code => code.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

    }
}
