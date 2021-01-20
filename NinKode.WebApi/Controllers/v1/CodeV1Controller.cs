namespace NinKode.WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Service.v1;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [DisplayName("Kode")]
    [Route("v1")]
    public class CodeV1Controller : ControllerBase
    {
        private readonly ICodeV1Service _codeService;

        public CodeV1Controller(ICodeV1Service codeService)
        {
            _codeService = codeService;
        }

        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<Codes> GetAll()
        {
            var result = _codeService.GetAll($"{GetHostPath()}hentkode/");
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
