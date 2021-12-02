namespace NinKode.WebApi.Controllers
{
    using System.ComponentModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "api")]
    [DisplayName("Eksport")]
    [EnableCors]
    [Route("{version:required}/eksport")]
    public class ExportController : ApiControllerBase<ExportController>
    {
        private const string DefaultNinVersion = "v2.3";

        private readonly IExportService _exportService;

        /// <summary>
        /// 
        /// </summary>
        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("csv")]
        public ActionResult ExportToCsv(string version = DefaultNinVersion)
        {
            var stream = _exportService.ExportToCsv(base.DbContext, base.GetVersion(version));
            if (stream == null) return new BadRequestResult();

            return File(
                stream,
                "application/zip",
                $"NiN_v{base.GetVersion(version)}.zip");
        }
    }
}
