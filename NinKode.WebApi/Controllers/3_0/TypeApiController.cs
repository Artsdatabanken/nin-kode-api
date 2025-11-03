using Microsoft.AspNetCore.Mvc;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.type;
using NiN3.Infrastructure.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NiN3.WebApi.Controllers
{
    /// <summary>
    /// API-kontroller for håndtering av NiN typekoder (Naturtyper i Norge klassifikasjonssystem).
    /// Tilbyr funksjonalitet for å hente typeinformasjon, hovedtyper, grunntyper og kartleggingsenheter.
    /// </summary>
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v{version:apiVersion}/typer")]
    [Tags("Typekoder")]
    public class TypeApiController : ControllerBase
    {
        private readonly ITypeApiService _typeApiService;
        private readonly IConfiguration _configuration;
        private const string ApiVersion = "3.0";

        /// <summary>
        /// Initialiserer en ny instans av TypeApiController.
        /// </summary>
        /// <param name="typeApiService">Tjeneste for typekode-operasjoner</param>
        /// <param name="configuration">Konfigurasjon for applikasjonen</param>
        public TypeApiController(ITypeApiService typeApiService, IConfiguration configuration)
        {
            _typeApiService = typeApiService ?? throw new ArgumentNullException(nameof(typeApiService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Henter alle typekoder i NiN klassifikasjonssystemet, ordnet hierarkisk.
        /// Returnerer en komplett oversikt over alle tilgjengelige naturtyper.
        /// </summary>
        /// <returns>Hierarkisk liste over alle typekoder med tilhørende metadata</returns>
        /// <response code="200">Returnerer den komplette listen over typekoder</response>
        /// <response code="500">Ved intern serverfeil</response>
        [HttpGet]
        [Route("allekoder")]
        [ProducesResponseType(typeof(IEnumerable<VersjonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var versjon = await _typeApiService.AllCodesAsync(ApiVersion);
                Response.Headers["Cache-Control"] = "max-age=3600";
                return Ok(versjon);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av typekoder" });
            }
        }

        /// <summary>
        /// Henter klassetype for et objekt basert på kortkode.
        /// </summary>
        /// <param name="kortkode">Kortkoden til objektet (f.eks. "C-PE-NA")</param>
        /// <returns>Klasseinformasjon for den angitte kortkoden</returns>
        /// <response code="200">Returnerer klasseinformasjonen</response>
        /// <response code="400">Hvis kortkode er ugyldig eller mangler</response>
        /// <response code="404">Hvis ingen klasse finnes for den angitte kortkoden</response>
        [HttpGet]
        [Route("klasse/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<KlasseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetKlasseByKortkode([Required] string kortkode = "C-PE-NA")
        {
            if (string.IsNullOrWhiteSpace(kortkode))
                return BadRequest(new { error = "Kortkode kan ikke være tom" });

            try
            {
                var typeklasseDto = _typeApiService.GetTypeklasse(kortkode, ApiVersion);
                return typeklasseDto != null 
                    ? Ok(typeklasseDto) 
                    : NotFound(new { error = $"Ingen klasse funnet for kortkode: {kortkode}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av klasseinformasjon" });
            }
        }

        /// <summary>
        /// Henter typeinformasjon basert på kortkode.
        /// </summary>
        /// <param name="kortkode">Kortkoden til typen (f.eks. "A-LV-EL")</param>
        /// <returns>Typeinformasjon for den angitte kortkoden</returns>
        /// <response code="200">Returnerer typeinformasjonen</response>
        /// <response code="400">Hvis kortkode er ugyldig eller mangler</response>
        /// <response code="404">Hvis ingen type finnes for den angitte kortkoden</response>
        [HttpGet]
        [Route("kodeforType/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<TypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTypeByKortkode([Required] string kortkode = "A-LV-EL")
        {
            if (string.IsNullOrWhiteSpace(kortkode))
                return BadRequest(new { error = "Kortkode kan ikke være tom" });

            try
            {
                var typeDto = _typeApiService.GetTypeByKortkode(kortkode, ApiVersion);
                return typeDto != null 
                    ? Ok(typeDto) 
                    : NotFound(new { error = $"Ingen type funnet for kortkode: {kortkode}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av typeinformasjon" });
            }
        }

        /// <summary>
        /// Henter hovedtypegruppe-informasjon basert på kortkode.
        /// </summary>
        /// <param name="kortkode">Kortkoden til hovedtypegruppen (f.eks. "FL-G")</param>
        /// <returns>Hovedtypegruppe-informasjon for den angitte kortkoden</returns>
        /// <response code="200">Returnerer hovedtypegruppe-informasjonen</response>
        /// <response code="400">Hvis kortkode er ugyldig eller mangler</response>
        /// <response code="404">Hvis ingen hovedtypegruppe finnes for den angitte kortkoden</response>
        [HttpGet]
        [Route("kodeforHovedtypegruppe/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<HovedtypegruppeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHovedtypegruppeByKortkode([Required] string kortkode = "FL-G")
        {
            if (string.IsNullOrWhiteSpace(kortkode))
                return BadRequest(new { error = "Kortkode kan ikke være tom" });

            try
            {
                var hovedtypegruppe = _typeApiService.GetHovedtypegruppeByKortkode(kortkode, ApiVersion);
                return hovedtypegruppe != null 
                    ? Ok(hovedtypegruppe) 
                    : NotFound(new { error = $"Ingen hovedtypegruppe funnet for kortkode: {kortkode}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av hovedtypegruppe-informasjon" });
            }
        }

        /// <summary>
        /// Henter hovedtype-informasjon basert på kortkode.
        /// </summary>
        /// <param name="kortkode">Kortkoden til hovedtypen (f.eks. "NA-TI01")</param>
        /// <returns>Hovedtype-informasjon for den angitte kortkoden</returns>
        /// <response code="200">Returnerer hovedtype-informasjonen</response>
        /// <response code="400">Hvis kortkode er ugyldig eller mangler</response>
        /// <response code="404">Hvis ingen hovedtype finnes for den angitte kortkoden</response>
        [HttpGet]
        [Route("kodeForHovedtype/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<HovedtypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHovedtypeByKortkode([Required] string kortkode = "NA-TI01")
        {
            if (string.IsNullOrWhiteSpace(kortkode))
                return BadRequest(new { error = "Kortkode kan ikke være tom" });

            try
            {
                var hovedtype = _typeApiService.GetHovedtypeByKortkode(kortkode, ApiVersion);
                return hovedtype != null 
                    ? Ok(hovedtype) 
                    : NotFound(new { error = $"Ingen hovedtype funnet for kortkode: {kortkode}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av hovedtype-informasjon" });
            }
        }

        /// <summary>
        /// Henter grunntype-informasjon basert på kortkode.
        /// </summary>
        /// <param name="kortkode">Kortkoden til grunntypen (f.eks. "I01-006")</param>
        /// <returns>Grunntype-informasjon for den angitte kortkoden</returns>
        /// <response code="200">Returnerer grunntype-informasjonen</response>
        /// <response code="400">Hvis kortkode er ugyldig eller mangler</response>
        /// <response code="404">Hvis ingen grunntype finnes for den angitte kortkoden</response>
        [HttpGet]
        [Route("kodeforGrunntype/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<GrunntypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGrunntypeByKortkode([Required] string kortkode = "I01-006")
        {
            if (string.IsNullOrWhiteSpace(kortkode))
                return BadRequest(new { error = "Kortkode kan ikke være tom" });

            try
            {
                var grunntype = _typeApiService.GetGrunntypeByKortkode(kortkode, ApiVersion);
                return grunntype != null 
                    ? Ok(grunntype) 
                    : NotFound(new { error = $"Ingen grunntype funnet for kortkode: {kortkode}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av grunntype-informasjon" });
            }
        }

        /// <summary>
        /// Henter kartleggingsenhet-informasjon basert på kortkode.
        /// </summary>
        /// <param name="kortkode">Kortkoden til kartleggingsenheten (f.eks. "LA01-M005-13")</param>
        /// <returns>Kartleggingsenhet-informasjon for den angitte kortkoden</returns>
        /// <response code="200">Returnerer kartleggingsenhet-informasjonen</response>
        /// <response code="400">Hvis kortkode er ugyldig eller mangler</response>
        /// <response code="404">Hvis ingen kartleggingsenhet finnes for den angitte kortkoden</response>
        [HttpGet]
        [Route("kodeforKartleggingsenhet/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<KartleggingsenhetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetKartleggingsenhetByKortkode([Required] string kortkode = "LA01-M005-13")
        {
            if (string.IsNullOrWhiteSpace(kortkode))
                return BadRequest(new { error = "Kortkode kan ikke være tom" });

            try
            {
                var kartleggingsenhet = _typeApiService.GetKartleggingsenhetByKortkode(kortkode, ApiVersion);
                return kartleggingsenhet != null 
                    ? Ok(kartleggingsenhet) 
                    : NotFound(new { error = $"Ingen kartleggingsenhet funnet for kortkode: {kortkode}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "En feil oppstod ved henting av kartleggingsenhet-informasjon" });
            }
        }
    }
}
