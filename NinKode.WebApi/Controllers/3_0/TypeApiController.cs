using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.type;
using NiN3.Infrastructure.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NiN3.WebApi.Controllers
{
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v{version:apiVersion}/typer"), Tags("Typekoder")]
    public class TypeApiController : Controller
    {
        private readonly ITypeApiService _typeApiService;
        /*private readonly ILogger _logger;*/
        private readonly IConfiguration _configuration;

        private string _versjon =  "3.0";
        //private readonly int _cacheDuration;
        //<summary>
        // This constructor initializes the TypeApiController class with the necessary services,
        // logger, and configuration. The ITypeApiService, ILogger,
        // and IConfiguration parameters are injected into the constructor
        // and assigned to the corresponding private fields. 
        // This allows the TypeApiController to access the services, logger, and configuration when needed.
        //</summary>
        public TypeApiController(ITypeApiService typeApiService, /*ILogger logger,*/ IConfiguration configuration/*, IOptions<CacheSettings> cacheSettings*/)
        {
            _typeApiService = typeApiService;
            /*_logger = logger;*/
            _configuration = configuration;
            //_cacheDuration = cacheSettings.Value.Duration;
        }

        /// <summary> 
        /// This method gets all 'Type'-codes, arranged heir 
        /// </summary>
        /// <returns> 
        /// The list of 'Type'-codes. 
        /// </returns>
        [HttpGet]
        [Route("allekoder")]
        //[OutputCache(Duration = 0/*_cacheDuration*/)]// 24 timer // almost no use in serverside-cache here,
        //browser side rendering is the penalty here
        [ProducesResponseType(typeof(IEnumerable<VersjonDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var versjon = await _typeApiService.AllCodesAsync("3.0");
            Response.Headers.Add("Cache-Control", "max-age=3600");
            return Ok(versjon);
        }

        /// <summary> 
        /// This method retrieves a 'Type'-kode by its 'kortkode'. 
        /// </summary>
        /// <param name="kortkode"> The 'kortkode' of the 'Type'-kode to retrieve. </param>
        /// <returns> 
        /// An 'IEnumerable' of the 'KlasseDto' class for the requested 'Type'-kode. 
        /// </returns>
        /// <response code="200"> 
        /// Returns an 'IEnumerable' of the 'KlasseDto' class, along with a status code of 200 (OK).
        /// </response>
        /// <response code="400"> 
        /// If the 'kortkode' parameter is not provided, returns a status code of 400 (Bad Request). 
        /// </response>
        /// <response code="404"> 
        /// If the requested 'Type'-kode does not exist, returns a status code of 404 (Not Found). 
        /// </response>
        [HttpGet]
        [Route("klasse/{kortkode}")]
        [Description("Henter klassetypen til objektet som kortkoden er tilknyttet")]
        [ProducesResponseType(typeof(IEnumerable<KlasseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult HentKlasse([Required] string kortkode = "C-PE-NA")
        {
            var typeklasseDto = _typeApiService.GetTypeklasse(kortkode, _versjon);
            if (typeklasseDto != null)
            {
                return Ok(typeklasseDto);
            }
            else
            {
                return NotFound("Ugyldig kortkode");
            }
        }

        [HttpGet]
        [Route("kodeforType/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<TypeDto>), StatusCodes.Status200OK)]
        public IActionResult hentkodeForType([Required] string kortkode = "A-LV-EL")
        {
            var typeDto = _typeApiService.GetTypeByKortkode(kortkode, _versjon);
            if (typeDto != null)
            {
                return Ok(typeDto);
            }
            else
            {
                return NotFound("Ugyldig kortkode");
            }
        }

        [HttpGet]
        [Route("kodeforHovedtypegruppe/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<HovedtypegruppeDto>), StatusCodes.Status200OK)]
        public IActionResult hentkodeForHovedtypegruppe([Required] string kortkode = "FL-G")
        {
            var hovedtypegruppe = _typeApiService.GetHovedtypegruppeByKortkode(kortkode, _versjon);

            if (hovedtypegruppe != null)
            {
                return Ok(hovedtypegruppe);
            }

            return NotFound("Ugyldig kortkode");
        }

        [HttpGet]
        [Route("kodeForHovedtype/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<HovedtypeDto>), StatusCodes.Status200OK)]
        public IActionResult HentKodeForHovedtype([Required] string kortkode = "NA-TI01")
        { 
            var hovedtype = _typeApiService.GetHovedtypeByKortkode(kortkode, _versjon);
            if (hovedtype != null)
            {
                return Ok(hovedtype);
            }

            return NotFound("Ugyldig kortkode");
        }

        [HttpGet]
        [Route("kodeforGrunntype/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<GrunntypeDto>), StatusCodes.Status200OK)]
        public IActionResult hentkodeForGrunntype([Required] string kortkode = "I01-006")
        {
            var grunntype = _typeApiService.GetGrunntypeByKortkode(kortkode, _versjon);
            if (grunntype != null)
            {
                return Ok(grunntype);
            }
            return NotFound("Ugyldig kortkode");
        }

        [HttpGet]
        [Route("kodeforKartleggingsenhet/{kortkode}")]
        [ProducesResponseType(typeof(IEnumerable<KartleggingsenhetDto>), StatusCodes.Status200OK)]
        public IActionResult hentkodeForKartleggingsenhet([Required] string kortkode = "LA01-M005-13")
        {
            var kartleggingsenhet = _typeApiService.GetKartleggingsenhetByKortkode(kortkode, _versjon);
            if (kartleggingsenhet != null)
            {
                return Ok(kartleggingsenhet);
            }
            return NotFound("Ugyldig kortkode");
        }
    }
}
