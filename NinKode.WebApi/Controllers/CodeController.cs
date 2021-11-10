namespace NinKode.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "beta")]
    [DisplayName("Kode")]
    [EnableCors]
    [Route("api/{version:required}/koder")]
    public class CodeController : ApiControllerBase<CodeController>
    {
        private const string DefaultNinVersion = "v2.3";
        private readonly ICodeService _codeService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeService"></param>
        public CodeController(ICodeService codeService)
        {
            _codeService = codeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version">NiN-versjon</param>
        /// <returns></returns>
        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<Codes> GetAll(string version = DefaultNinVersion)
        {
            return _codeService.GetAll(base.DbContext, base.GetHostPath(), base.GetVersion(version));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">NiN-kode</param>
        /// <param name="version">NiN-versjon</param>
        /// <returns></returns>
        [HttpGet]
        [Route("hentkode/{id:required}")]
        public Codes GetCode(string id = "NA", string version = DefaultNinVersion)
        {
            return _codeService.GetByKode(base.DbContext, id, GetHostPath(), base.GetVersion(version));
        }
    }
}
