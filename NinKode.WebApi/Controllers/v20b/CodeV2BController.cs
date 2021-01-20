namespace NinKode.WebApi.Controllers.v20b
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Service.v21b;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v2b")]
    [DisplayName("Kode")]
    [Route("v2b/koder")]
    public class CodeV2BController : ControllerBase
    {
        private readonly ICodeV21BService _codeService;

        public CodeV2BController(ICodeV21BService codeV21BService)
        {
            _codeService = codeV21BService;
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
