﻿namespace NinKode.WebApi.Controllers.v21
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using NinKode.Common.Models.Variety;
    using NinKode.Database.Service.v21;

    [ApiController]
    [Route("v2.1/variasjon")]
    public class VarietyV21Controller : ControllerBase
    {
        private readonly IVarietyV21Service _varietyService;

        public VarietyV21Controller(IVarietyV21Service varietyV21Service)
        {
            _varietyService = varietyV21Service;
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
