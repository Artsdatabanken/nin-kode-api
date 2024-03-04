using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiN3.Infrastructure.Services;
//using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace NiN3.WebApi.Controllers
{
    [ApiVersion("3.0")]
    [ApiController]
    //[Route("v3.0/rapporter"), Tags("Rapporter")]
    [Route("v{version:apiVersion}/rapporter"), Tags("Rapporter")]
    public class RapportController : ControllerBase
    {
        private readonly IRapportService _rapportService;
        private readonly IConfiguration _configuration;

        public RapportController(IRapportService rapportService, IConfiguration configuration)
        {
            _rapportService = rapportService;
            _configuration = configuration;
        }

        
        [HttpGet("kodeoversikt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Kodeoversikt()
        {
            string kodeoversiktcsv = _rapportService.MakeKodeoversiktCSV("3.0");
            byte[] csvBytes = Encoding.UTF8.GetBytes(kodeoversiktcsv);
            byte[] bom = Encoding.UTF8.GetPreamble();
            var result = bom.Concat(csvBytes).ToArray();
            return File(result, "text/csv; charset=utf-8", "kodeoversikt.csv");
        }

        [HttpGet("exceldata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetRapportData()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "report_data", "nin3_0.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "nin3_0.xlsx");
        }
    }
}
