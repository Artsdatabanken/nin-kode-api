using Microsoft.AspNetCore.Mvc;
using NinKode.Common.Interfaces;

namespace NinKode.WebApi.Controllers.Initial;

[ApiVersion("2.3")]
[ApiVersion("2.2", Deprecated = true)]
[ApiVersion("2.1b", Deprecated = true)]
[ApiVersion("2.1", Deprecated = true)]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
[Route("v{version:apiVersion}/eksport"), Tags("Eksport")]
public class ExportController : ApiControllerBase<ExportController>
{
    private readonly IExportService _exportService;

    public ExportController(IExportService exportService)
    {
        _exportService = exportService;
    }

    [HttpGet]
    [Route("csv")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult ExportToCsv()
    {
        var stream = _exportService.ExportToCsv(DbContext, GetVersion());
            
        return File(stream, "application/zip", $"NiN_v{GetVersion()}.zip");
    }
}