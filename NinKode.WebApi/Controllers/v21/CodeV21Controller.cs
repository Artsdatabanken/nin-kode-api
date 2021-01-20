namespace NinKode.WebApi.Controllers.v21
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v2.1")]
    [DisplayName("Kode")]
    [Route("v2.1/koder")]
    public class CodeV21Controller : ControllerBase
    {
        private readonly ICodeV21Service _codeService;

        public CodeV21Controller(ICodeV21Service codeService)
        {
            _codeService = codeService;
        }

        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<Codes> GetAll()
        {
            var result = _codeService.GetAll(GetHostPath());
            foreach (var code in result)
            {
                yield return code;
            }
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public Codes GetCode(string id)
        {
            return _codeService.GetByKode(id, GetHostPath());
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
