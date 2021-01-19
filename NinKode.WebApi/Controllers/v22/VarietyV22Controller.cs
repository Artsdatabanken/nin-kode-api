namespace NinKode.WebApi.Controllers.v22
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Service.v22;

    [ApiController]
    //[Route("variasjon")]
    [Route("v2.2/variasjon")]
    public class VarietyV22Controller : ControllerBase
    {
        private readonly VarietyV22Service _varietyService;

        public VarietyV22Controller(IConfiguration configuration)
        {
            _varietyService = new VarietyV22Service(configuration);
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

        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "VarietyV22Controller";
        }
    }
}
