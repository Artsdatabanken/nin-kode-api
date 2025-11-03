using Microsoft.AspNetCore.Mvc;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.type;
using NiN3.Core.Models.DTOs.variabel;
using NiN3.Infrastructure.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NiN3.WebApi.Controllers
{
    /// <summary>
    /// API-kontroller for håndtering av variabelkoder i NiN (Natur i Norge) systemet.
    /// Tilbyr endepunkter for å hente informasjon om variabler, variabelnavn og måleskalaer.
    /// </summary>    
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v{version:apiVersion}/variabler")]
    [Tags("Variabelkoder")]
    [Produces("application/json")]
    public class VariabelApiController : ControllerBase
    {
        private readonly IVariabelApiService _variabelApiService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VariabelApiController> _logger;
        private const string API_VERSJON = "3.0";

        /// <summary>
        /// Initialiserer en ny instans av VariabelApiController.
        /// </summary>
        /// <param name="variabelApiService">Service for variabelhåndtering</param>
        /// <param name="configuration">Konfigurasjonsobjekt</param>
        /// <param name="logger">Logger for feilhåndtering og overvåkning</param>
        public VariabelApiController(
            IVariabelApiService variabelApiService,
            IConfiguration configuration,
            ILogger<VariabelApiController> logger)
        {
            _variabelApiService = variabelApiService ?? throw new ArgumentNullException(nameof(variabelApiService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Henter alle variabelkoder i NiN 3.0 systemet.
        /// </summary>
        [HttpGet]
        [Route("allekoder")]
        [ProducesResponseType(typeof(IEnumerable<VersjonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HentAlleVariabelkoder()
        {
            try
            {
                _logger.LogInformation("Henter alle variabelkoder for versjon {Versjon}", API_VERSJON);
                
                var variabelkoder = _variabelApiService.AllCodes(API_VERSJON);
                
                if (variabelkoder == null)
                {
                    _logger.LogWarning("Ingen variabelkoder funnet for versjon {Versjon}", API_VERSJON);
                    return Ok(new List<VersjonDto>());
                }

                _logger.LogInformation("Hentet {Antall} variabelkoder", 
                    variabelkoder is ICollection<VersjonDto> collection ? collection.Count : "ukjent antall");
                
                return Ok(variabelkoder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av alle variabelkoder");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved henting av variabelkoder", error = ex.Message });
            }
        }

        /// <summary>
        /// Henter klassetypen for objektet som er tilknyttet den oppgitte kortkoden.
        /// </summary>
        /// <param name="kortkode">
        /// Kortkoden for variabelen som klassetype skal hentes for.
        /// Eksempler: "AD-TE" (Areal dominert av gran), "B-M" (Biomasse tresjikt).
        /// </param>    
        [HttpGet]
        [Route("klasse/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<KlasseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HentKlassetype([Required] string kortkode = "AD-TE")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(kortkode))
                {
                    _logger.LogWarning("Tom eller ugyldig kortkode mottatt: {Kortkode}", kortkode);
                    return BadRequest(new { message = "Kortkode kan ikke være tom eller null" });
                }

                _logger.LogInformation("Henter klassetype for kortkode: {Kortkode}", kortkode);
                
                var variabelklasse = _variabelApiService.GetVariabelKlasse(kortkode, API_VERSJON);
                
                if (variabelklasse == null)
                {
                    _logger.LogWarning("Ingen klassetype funnet for kortkode: {Kortkode}", kortkode);
                    return NotFound(new { message = $"Ingen klassetype funnet for kortkode '{kortkode}'" });
                }

                _logger.LogInformation("Hentet klassetype for kortkode {Kortkode}", kortkode);
                return Ok(variabelklasse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av klassetype for kortkode: {Kortkode}", kortkode);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved henting av klassetype", error = ex.Message });
            }
        }

        /// <summary>
        /// Henter detaljert informasjon om en variabel basert på kortkode.
        /// </summary>
        /// <param name="kortkode">
        /// Kortkoden for variabelen som skal hentes.
        /// Eksempel: "B-M" for biomasse eller "pH" for pH-verdi.
        /// </param>
        [HttpGet]
        [Route("kodeForVariabel/{kortkode}")]
        [ProducesResponseType(typeof(VariabelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HentVariabelByKortkode([Required] string kortkode = "B-M")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(kortkode))
                {
                    _logger.LogWarning("Tom eller ugyldig variabelkortkode: {Kortkode}", kortkode);
                    return BadRequest(new { message = "Variabelkortkode kan ikke være tom eller null" });
                }

                _logger.LogInformation("Henter variabel for kortkode: {Kortkode}", kortkode);
                
                var variabel = _variabelApiService.GetVariabelByKortkode(kortkode, API_VERSJON);
                
                if (variabel == null)
                {
                    _logger.LogWarning("Ingen variabel funnet for kortkode: {Kortkode}", kortkode);
                    return NotFound(new { message = $"Ingen variabel funnet for kortkode '{kortkode}'" });
                }

                _logger.LogInformation("Hentet variabel for kortkode {Kortkode}", kortkode);
                return Ok(variabel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av variabel for kortkode: {Kortkode}", kortkode);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved henting av variabel", error = ex.Message });
            }
        }

        /// <summary>
        /// Henter informasjon om et variabelnavn basert på kortkode.
        /// </summary>
        /// <param name="kortkode">
        /// Kortkoden for variabelnavnet som skal hentes.
        /// Eksempel: "AD-TE" for "areal dominert av gran".
        /// </param>
        [HttpGet]
        [Route("kodeForVariabelnavn/{kortkode}")]
        [ProducesResponseType(typeof(VariabelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HentVariabelnavnByKortkode([Required] string kortkode = "AD-TE")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(kortkode))
                {
                    _logger.LogWarning("Tom eller ugyldig variabelnavnkortkode: {Kortkode}", kortkode);
                    return BadRequest(new { message = "Variabelnavnkortkode kan ikke være tom eller null" });
                }

                _logger.LogInformation("Henter variabelnavn for kortkode: {Kortkode}", kortkode);
                
                var variabelnavn = _variabelApiService.GetVariabelnavnByKortkode(kortkode, API_VERSJON);
                
                if (variabelnavn == null)
                {
                    _logger.LogWarning("Ingen variabelnavn funnet for kortkode: {Kortkode}", kortkode);
                    return NotFound(new { message = $"Ingen variabelnavn funnet for kortkode '{kortkode}'" });
                }

                _logger.LogInformation("Hentet variabelnavn for kortkode {Kortkode}", kortkode);
                return Ok(variabelnavn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av variabelnavn for kortkode: {Kortkode}", kortkode);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved henting av variabelnavn", error = ex.Message });
            }
        }

        /// <summary>
        /// Henter informasjon om en måleskala basert på måleskalanavn.
        /// </summary>
        /// <param name="maaleskalaNavn">
        /// Navnet på måleskalaen som skal hentes.
        /// Eksempel: "BK-SI" for biomasse kilogram per kvadratmeter.
        /// </param>
        [HttpGet]
        [Route("maaleskala/{maaleskalaNavn}")]
        [ProducesResponseType(typeof(MaaleskalaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HentMaaleskala([Required] string maaleskalaNavn = "BK-SI")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maaleskalaNavn))
                {
                    _logger.LogWarning("Tom eller ugyldig måleskalanavn: {MaaleskalaNavn}", maaleskalaNavn);
                    return BadRequest(new { message = "Måleskalanavn kan ikke være tom eller null" });
                }

                _logger.LogInformation("Henter måleskala for navn: {MaaleskalaNavn}", maaleskalaNavn);
                
                var maaleskala = _variabelApiService.GetMaaleskalaByMaaleskalanavn(maaleskalaNavn, API_VERSJON);
                
                if (maaleskala == null)
                {
                    _logger.LogWarning("Ingen måleskala funnet for navn: {MaaleskalaNavn}", maaleskalaNavn);
                    return NotFound(new { message = $"Ingen måleskala funnet for navn '{maaleskalaNavn}'" });
                }

                _logger.LogInformation("Hentet måleskala for navn {MaaleskalaNavn}", maaleskalaNavn);
                return Ok(maaleskala);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av måleskala for navn: {MaaleskalaNavn}", maaleskalaNavn);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved henting av måleskala", error = ex.Message });
            }
        }
    }
}