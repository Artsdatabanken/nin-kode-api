namespace NinKode.WebApi.Controllers.v2b
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using NinKode.Common;

    [ApiController]
    [Route("v2b/koder")]
    public class CodeV2BController : ControllerBase
    {
        [HttpGet]
        [Route("allekoder")]
        public async Task<KoderAllekoder[]> GetAll()
        {
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://webtjenester.artsdatabanken.no/NiN/v2b/koder/Allekoder");
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<KoderAllekoder[]>(await response.Content.ReadAsStringAsync());
        }
    }
}
