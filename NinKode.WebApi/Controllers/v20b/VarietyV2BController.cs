namespace NinKode.WebApi.Controllers.v20b
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Service.v20b;

    [ApiController]
    [Route("v2b/variasjon")]
    public class VarietyV2BController : ControllerBase
    {
        private const string DatabaseUrl = "http://localhost:8080/";
        private const string DefaultDatabase = "SOSINiNv2.0b";

        private readonly VarietyV2BService _varietyService;

        public VarietyV2BController()
        {
            _varietyService = new VarietyV2BService(DatabaseUrl, DefaultDatabase);
        }

        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<VarietyAllCodes> GetAll()
        {
            var result = _varietyService.GetAll(GetHostPath());
            foreach (var variety in result)
            {
                yield return variety;
            }
        }

        [HttpGet]
        [Route("hentkode/{id}")]
        public VarietyCode GetCode(string id)
        {
            return _varietyService.GetByKode(id, GetHostPath());
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
