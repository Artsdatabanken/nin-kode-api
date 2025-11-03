using Microsoft.AspNetCore.Mvc;
using NiN3.Infrastructure.Services;
using System.Text;

namespace NiN3.WebApi.Controllers
{
    /// <summary>
    /// API-kontroller for generering og nedlasting av rapporter fra NiN (Natur i Norge) systemet.
    /// Tilbyr endepunkter for å generere kodeoversikter, eksportfiler og datadumper i ulike formater.
    /// </summary>
    /// <remarks>
    /// Denne kontrolleren gir tilgang til rapportgenerering for NiN-systemet, inkludert:
    /// - CSV-eksport av komplette kodeoversikter
    /// - Spesialtilpassede eksportfiler for Drupal CMS
    /// - Excel-filer med strukturerte data
    /// - Metadata om datakvalitet og oppdateringstidspunkt
    /// 
    /// Alle rapporter følger NiN 3.0 standarden og inneholder UTF-8 kodede data med BOM.
    /// </remarks>
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v{version:apiVersion}/rapporter")]
    [Tags("Rapporter")]
    [Produces("application/json", "text/csv", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public class RapportController : ControllerBase
    {
        private readonly IRapportService _rapportService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RapportController> _logger;
        private const string API_VERSJON = "3.0";

        /// <summary>
        /// Initialiserer en ny instans av RapportController.
        /// </summary>
        /// <param name="rapportService">Service for rapportgenerering</param>
        /// <param name="configuration">Konfigurasjonsobjekt</param>
        /// <param name="logger">Logger for feilhåndtering og overvåkning</param>
        public RapportController(
            IRapportService rapportService, 
            IConfiguration configuration,
            ILogger<RapportController> logger)
        {
            _rapportService = rapportService ?? throw new ArgumentNullException(nameof(rapportService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Genererer og laster ned en komplett kodeoversikt for NiN 3.0 som CSV-fil.
        /// </summary>
        /// <returns>
        /// En CSV-fil med alle koder, navn, beskrivelser og hierarkiske sammenhenger
        /// i NiN 3.0 systemet. Filen inneholder UTF-8 kodede data med BOM for optimal
        /// kompatibilitet med Excel og andre verktøy.
        /// </returns>
        /// <response code="200">Returnerer CSV-fil med kodeoversikt</response>
        /// <response code="500">Intern serverfeil ved generering av rapport</response>
        [HttpGet("kodeoversikt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LastNedKodeoversikt()
        {
            try
            {
                _logger.LogInformation("Genererer kodeoversikt CSV for versjon {Versjon}", API_VERSJON);
                
                var kodeoversiktCsv = _rapportService.MakeKodeoversiktCSV(API_VERSJON);
                
                if (string.IsNullOrEmpty(kodeoversiktCsv))
                {
                    _logger.LogWarning("Tom kodeoversikt generert for versjon {Versjon}", API_VERSJON);
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                        new { message = "Kunne ikke generere kodeoversikt" });
                }

                var csvBytes = Encoding.UTF8.GetBytes(kodeoversiktCsv);
                var bom = Encoding.UTF8.GetPreamble();
                var resultat = bom.Concat(csvBytes).ToArray();

                _logger.LogInformation("Kodeoversikt CSV generert, størrelse: {Storrelse} bytes", resultat.Length);
                
                return File(resultat, "text/csv; charset=utf-8", "nin3_0_kodeoversikt.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved generering av kodeoversikt CSV");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved generering av kodeoversikt", error = ex.Message });
            }
        }

        /// <summary>
        /// Genererer en spesialtilpasset CSV-fil for import til Drupal CMS.
        /// Denne funksjonen er skjult fra offentlig API-dokumentasjon.
        /// </summary>
        /// <returns>
        /// En tab-separert CSV-fil optimalisert for import i Drupal CMS-systemer.
        /// Inneholder kartleggingsenheter strukturert for automatisert behandling.
        /// </returns>
        /// <response code="200">Returnerer tab-separert CSV-fil for Drupal import</response>
        /// <response code="500">Intern serverfeil ved generering</response>
        [HttpGet("drupalimport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult LastNedDrupalImport()
        {
            try
            {
                _logger.LogInformation("Genererer Drupal import CSV for versjon {Versjon}", API_VERSJON);
                
                var importCsv = _rapportService.MakeKartleggingsoversiktCSV(API_VERSJON, '\t'.ToString());
                
                if (string.IsNullOrEmpty(importCsv))
                {
                    _logger.LogWarning("Tom Drupal import generert for versjon {Versjon}", API_VERSJON);
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                        new { message = "Kunne ikke generere Drupal import-fil" });
                }

                var csvBytes = Encoding.UTF8.GetBytes(importCsv);
                var bom = Encoding.UTF8.GetPreamble();
                var resultat = bom.Concat(csvBytes).ToArray();

                _logger.LogInformation("Drupal import CSV generert, størrelse: {Storrelse} bytes", resultat.Length);
                
                return File(resultat, "text/csv; charset=utf-8", "nin3_0_drupalimport_kartleggingsenheter.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved generering av Drupal import CSV");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved generering av Drupal import-fil", error = ex.Message });
            }
        }

        /// <summary>
        /// Laster ned en forhåndsgenerert Excel-fil med komplette NiN 3.0 data.
        /// </summary>
        /// <returns>
        /// En Excel-fil (.xlsx) som inneholder alle NiN 3.0 koder strukturert
        /// i flere arbeidsark for enkel analyse og videre bearbeiding.
        /// </returns>
        /// <response code="200">Returnerer Excel-fil med NiN data</response>
        /// <response code="404">Excel-fil ikke funnet på serveren</response>
        /// <response code="500">Intern serverfeil ved filhåndtering</response>
        [HttpGet("exceldata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LastNedExcelData()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "report_data", API_VERSJON.Replace(".", "_"), "nin3_0.xlsx");
                
                _logger.LogInformation("Forsøker å laste Excel-fil fra: {FilSti}", filePath);
                
                if (!System.IO.File.Exists(filePath))
                {
                    _logger.LogWarning("Excel-fil ikke funnet: {FilSti}", filePath);
                    return NotFound(new { message = "Excel-datafil ikke tilgjengelig" });
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                _logger.LogInformation("Excel-fil lastet, størrelse: {Storrelse} bytes", fileBytes.Length);
                
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "nin3_0_komplett_datasett.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved lasting av Excel-datafil");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved lasting av Excel-fil", error = ex.Message });
            }
        }

        /// <summary>
        /// Henter metadata om når datagrunnlaget sist ble oppdatert.
        /// </summary>
        /// <returns>
        /// Informasjon om siste oppdateringstidspunkt for datagrunnlaget,
        /// inkludert versjonsinformasjon og datakvalitetsstatus.
        /// </returns>
        /// <response code="200">Returnerer informasjon om dataoppdatering</response>
        /// <response code="500">Intern serverfeil ved henting av metadata</response>
        [HttpGet("dataDateTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HentDataOppdateringstidspunkt()
        {
            try
            {
                _logger.LogInformation("Henter data-oppdateringstidspunkt");
                
                var dataOppdatering = _rapportService.GetDataDate();
                
                _logger.LogInformation("Hentet data-oppdateringstidspunkt: {Tidspunkt}", dataOppdatering);
                
                return Ok(new { 
                    oppdateringstidspunkt = dataOppdatering,
                    versjon = API_VERSJON,
                    hentetTidspunkt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av data-oppdateringstidspunkt");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "En feil oppstod ved henting av oppdateringstidspunkt", error = ex.Message });
            }
        }
    }
}
