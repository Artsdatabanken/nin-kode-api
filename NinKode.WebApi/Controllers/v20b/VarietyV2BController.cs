namespace NinKode.WebApi.Controllers.v20b
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Service.v21b;

    [ApiController]
    [ApiExplorerSettings(GroupName = "v2b")]
    [DisplayName("Variasjon")]
    [Route("v2b/variasjon")]
    public class VarietyV2BController : ControllerBase
    {
        private readonly IVarietyV21BService _varietyService;

        public VarietyV2BController(IVarietyV21BService varietyService)
        {
            _varietyService = varietyService;
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
