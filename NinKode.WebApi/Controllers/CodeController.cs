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
        public IEnumerable<Codes> GetAll()
        {
            return _codeService.GetAll(base.GetHostPath(), base.GetVersion(DefaultNinVersion));
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public Codes GetCode(string id)
        {
            return _codeService.GetByKode(id, GetHostPath(), base.GetVersion(DefaultNinVersion));
        }
    }
}
