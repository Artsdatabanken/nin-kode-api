namespace NinKode.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "api")]
    [DisplayName("Versjon")]
    [EnableCors]
    [Route("")]
    public class VersionController : ApiControllerBase<VersionController>
    {
        private readonly IVersionService _versionService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionService"></param>
        public VersionController(IVersionService versionService)
        {
            _versionService = versionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("versjoner")]
        public IEnumerable<string> GetVersions()
        {
            return _versionService.GetVersions(base.DbContext);
        }
    }
}
