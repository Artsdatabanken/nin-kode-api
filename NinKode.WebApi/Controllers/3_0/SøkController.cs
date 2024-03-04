/*
using Microsoft.AspNetCore.Mvc;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.Services;

namespace NiN3.WebApi.Controllers
{
    [ApiVersion("3.0")]
    [ApiController]
    public class SøkController : Controller
    {
    private readonly ISearchService _searchService;

        public SøkController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("enkeltSøk")]
        public IActionResult SimpleSearch(string searchTerm, KlasseEnum klasseEnum, SearchMethodEnum searchMethodEnum)
        {
            var results = _searchService.SimpleSearch(searchTerm, klasseEnum, searchMethodEnum);
            return Ok(results);
        }
    }
}
*/