
using Microsoft.AspNetCore.Mvc;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.Services;

namespace NiN3.WebApi.Controllers
{
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v{version:apiVersion}/soek"), Tags("Søk")]
    public class SoekController : ControllerBase
    {
    private readonly ISearchService _searchService;

        public SoekController(ISearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpGet("enkeltSoek"), Tags("Enkelt søk")]
        public IActionResult SimpleSearch(string searchTerm, KlasseEnum klasseEnum, SearchMethodEnum searchMethodEnum)
        {
            var results = _searchService.SimpleSearch(searchTerm, klasseEnum, searchMethodEnum);
            return Ok(results);
        }
    }
}
