namespace NinKode.WebApi.Controllers.v2b
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Models.Code;
    using NinKode.Database.Service.v2b;

    [ApiController]
    [Route("v2b/koder")]
    public class CodeV2BController : ControllerBase
    {
        private const string DatabaseUrl = "http://localhost:8080/";
        private const string DefaultDatabase = "SOSINiNv2.0b";

        private readonly CodeV2BService _codeService;

        public CodeV2BController()
        {
            _codeService = new CodeV2BService(DatabaseUrl, DefaultDatabase);
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
