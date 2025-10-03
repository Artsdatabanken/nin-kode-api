
using Microsoft.AspNetCore.Mvc;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace NiN3.WebApi.Controllers
{
    /// <summary>
    /// Søkekontroller for NiN (Naturtyper i Norge) klassifikasjonssystemet.
    /// Tilbyr funksjonalitet for å søke gjennom alle typer naturklassifikasjoner.
    /// </summary>
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v{version:apiVersion}/soek")]
    [Tags("Søk")]
    public class SoekController : ControllerBase
    {
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initialiserer en ny instans av SoekController.
        /// </summary>
        /// <param name="searchService">Tjeneste for søkefunksjonalitet</param>
        public SoekController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Utfører et enkelt søk i NiN klassifikasjonssystemet.
        /// </summary>
        [HttpGet("enkeltSoek")]
        [Tags("Enkelt søk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SimpleSearch(
            [FromQuery, Required] 
            string searchTerm,            
            [FromQuery] 
            KlasseEnum klasseEnum = KlasseEnum.ALL,            
            [FromQuery] 
            SearchMethodEnum searchMethodEnum = SearchMethodEnum.C)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest(new { 
                        error = "Søketerm kan ikke være tom", 
                        message = "Vennligst oppgi en gyldig søketerm" 
                    });
                }
                var results = _searchService.SimpleSearch(searchTerm, klasseEnum, searchMethodEnum);

                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { 
                    error = "En intern feil oppstod under søket",
                    message = "Vennligst prøv igjen senere"
                });
            }
        }
    }
}
