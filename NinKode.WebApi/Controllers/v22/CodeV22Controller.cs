namespace NinKode.WebApi.Controllers.v22
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v2.2")]
    [DisplayName("Kode")]
    [EnableCors]
    [Route("v2.2/koder")]
    public class CodeV22Controller : ControllerBase
    {
        private readonly ICodeV22Service _codeService;

        public CodeV22Controller(ICodeV22Service codeService)
        {
            _codeService = codeService;
        }

        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<Codes> GetAll()
        {
            var result = _codeService.GetAll(null, GetHostPath());
            foreach (var code in result)
            {
                yield return code;
            }
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public Codes GetCode(string id)
        {
            return _codeService.GetByKode(null, id, GetHostPath());
        }

        private string GetHostPath()
        {
            var path = Request.Path.Value;
            path = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var protocol = Request.IsHttps ? "s" : "";
            return $"http{protocol}://{Request.Host}{path}";
        }
    }
}
