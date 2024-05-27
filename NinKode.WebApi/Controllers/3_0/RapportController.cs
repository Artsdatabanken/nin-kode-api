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
            //byte[] bom = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage).GetPreamble();
            var result = bom.Concat(csvBytes).ToArray();
            return File(result, "text/csv; charset=utf-8", "kodeoversikt.csv");            
        }

        /*
        [HttpGet("kodeoversiktExcel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult KodeoversiktExcel()
        {
            string kodeoversiktcsv = _rapportService.MakeKodeoversiktCSV("3.0", ",");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Assuming that your CSV data is comma separated and has a header row
                var lines = kodeoversiktcsv.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    var cells = lines[i].Split(',');
                    for (int j = 0; j < cells.Length; j++)
                    {
                        worksheet.Cells[i + 1, j + 1].Value = cells[j];
                    }
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Reset the position of the stream to ensure correct reading
                stream.Position = 0;

                // Return the Excel file
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "kodeoversikt.xlsx");
            }
        }*/

        [HttpGet("exceldata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetRapportData()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "report_data/3_0", "nin3_0.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "nin3_0.xlsx");
        }
    }
}
