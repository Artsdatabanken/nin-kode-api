namespace NinKode.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;

    [ApiController]
    [ApiExplorerSettings(GroupName = "beta")]
    [DisplayName("Kode")]
    [EnableCors]
    [Route("api/v{version}/koder")]
    public class CodeController : ApiControllerBase
    {
        private const string DefaultNinVersion = "2.3";
        private readonly ICodeService _codeService;

        public CodeController(ICodeService codeService)
        {
            _codeService = codeService;
        }

        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<Codes> GetAll(string version = DefaultNinVersion)
        {
            return _codeService.GetAll(base.GetHostPath(), base.GetVersion(version));
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public Codes GetCode(string id = "NA", string version = DefaultNinVersion)
        {
            return _codeService.GetByKode(id, GetHostPath(), base.GetVersion(version));
        }
    }
}
