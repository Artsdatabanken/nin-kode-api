using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiN3.Infrastructure.Services;
//using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace NiN3.WebApi.Controllers
{
    [ApiVersion("3.0")]
    [ApiController]
    [Route("v3.0/rapporter"), Tags("Rapporter")]
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
        

        /*
        public enum ReportFormat
        {
            csv,
            xlsx,
        }

        /// <summary>
        /// Returns a csv or xlsx file based on the codeoversikt dataset.
        /// </summary>
        /// <remarks>
        /// To download the report in CSV or XLSX format, add '?format={csv|xlsx}' parameter in Swagger UI. The default format is CSV. 
        /// </remarks>
        /// <param name="format"></param>
        /// <response code="200">Returns the report file in CSV or XLSX format</response>
        [HttpGet("kodeoversikt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Kodeoversikt([FromQuery][SwaggerParameter("The report file format")] ReportFormat format = ReportFormat.csv)
        {
            byte[] bytes;
            string contentType;
            string filename;

            if (format == ReportFormat.xlsx)
            {
                //bytes = _rapportService.MakeKodeoversiktXlsx("3.0");
                bytes =_rapportService.MakeKodeoversiktXlsx("3.0");
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=iso-8859-1";
                filename = "kodeoversikt.xlsx";
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(_rapportService.MakeKodeoversiktCSV("3.0"));
                contentType = "text/csv; charset=utf-8";
                filename = "kodeoversikt.csv";
            }

            byte[] bom = Encoding.UTF8.GetPreamble();
            var result = bom.Concat(bytes).ToArray();
            return File(result, contentType, filename);
        }        
        
        
        
        public IActionResult Kodeoversikt([FromQuery(Name = "format")][SwaggerParameter("SelectCSVorXlsx")] string format = "csv")
        {
            string kodeoversikt = _rapportService.MakeKodeoversiktCSV("3.0");
            if (format == "xlsx")
            {
                //byte[] xlsxBytes = _rapportService.MakeKodeoversiktXlsx("3.0");
                //return File(xlsxBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "kodeoversikt.xlsx");
                byte[] csvBytes = Encoding.UTF8.GetBytes(kodeoversikt);
                byte[] bom = Encoding.UTF8.GetPreamble();
                var result = bom.Concat(csvBytes).ToArray();
                return File(result, "text/csv; charset=utf-8", "kodeoversikt.csv");
            }
            else
            {
                byte[] csvBytes = Encoding.UTF8.GetBytes(kodeoversikt);
                byte[] bom = Encoding.UTF8.GetPreamble();
                var result = bom.Concat(csvBytes).ToArray();
                return File(result, "text/csv; charset=utf-8", "kodeoversikt.csv");
            }
        }
        */

    }
}
