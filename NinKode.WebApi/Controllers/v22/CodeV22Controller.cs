namespace NinKode.WebApi.Controllers.v22
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Models.Code;
    using NinKode.Database.Service.v22;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v2.2")]
    [Route("v2.2/koder")]
    public class CodeV22Controller : ControllerBase
    {
        private readonly ICodeV22Service _codeService;

        public CodeV22Controller(ICodeV22Service codeV22Service)
        {
            _codeService = codeV22Service;
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

        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "CodeV22Controller";
        }
    }
}
