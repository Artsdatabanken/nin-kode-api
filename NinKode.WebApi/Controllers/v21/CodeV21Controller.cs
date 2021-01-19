namespace NinKode.WebApi.Controllers.v21
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Service.v21;

    [ApiController]
    [Route("v2.1/koder")]
    public class CodeV21Controller : ControllerBase
    {
        private readonly ICodeV21Service _codeService;

        public CodeV21Controller(ICodeV21Service codeV21Service)
        {
            _codeService = codeV21Service;
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
