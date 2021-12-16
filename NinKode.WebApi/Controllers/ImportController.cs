namespace NinKode.WebApi.Controllers
{
    using System.ComponentModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "api")]
    [DisplayName("Import")]
    [EnableCors]
    [Route("import")]
    public class ImportController : ApiControllerBase<ImportController>
    {
        private readonly IImportService _importService;

        /// <summary>
        /// 
        /// </summary>
        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Check if user has access
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("isauthorized")]
        public string IsAuthorized()
        {
            return "ok";
        }

        /// <summary>
        /// Check if user has access
        /// </summary>
        /// <returns></returns>
        [Authorize("WriteAccess")]
        [HttpPost]
        [Route("isauthorizedwithrole")]
        public string IsAuthorizedWithRole()
        {
            return "ok";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [Authorize("WriteAccess")]
        [HttpPost]
        [Route("csv")]
        public ActionResult ImportFromCsv(IFormFile file, string version = "")
        {
            if (file == null || file.Length == 0) return new BadRequestResult();

            var result = _importService.ImportFromCsv(file.OpenReadStream(), base.DbContext, base.GetVersion(version));

            if (!result) return new BadRequestResult();

            return Ok($"Imported {file.FileName}");
        }
    }
}
