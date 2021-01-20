namespace NinKode.WebApi.Controllers.v2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Service.v2;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    [DisplayName("Kode")]
    [Route("v2")]
    public class CodeV2Controller : ControllerBase
    {
        private readonly ICodeV2Service _codeService;

        public CodeV2Controller(ICodeV2Service codeV2Service)
        {
            _codeService = codeV2Service;
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
