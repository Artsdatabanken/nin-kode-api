namespace NinKode.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Variety;

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "beta")]
    [DisplayName("Variasjon")]
    [EnableCors]
    [Route("{version:required}/variasjon")]
    public class VarietyController : ApiControllerBase<VarietyController>
    {
        private const string DefaultNinVersion = "v2.2";
        private readonly IVarietyService _varietyService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varietyService"></param>
        public VarietyController(IVarietyService varietyService)
        {
            _varietyService = varietyService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version">NiN-versjon</param>
        /// <returns></returns>
        [HttpGet]
        [Route("allekoder")]
        public IEnumerable<VarietyAllCodes> GetAll(string version = DefaultNinVersion)
        {
            var result = _varietyService.GetAll(base.DbContext, GetHostPath(), base.GetVersion(version));
            foreach (var variety in result)
            {
                yield return variety;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Beskrivelsessystem kode</param>
        /// <param name="version">NiN-versjon</param>
        /// <returns></returns>
        [HttpGet]
        [Route("hentkode/{id:required}")]
        public VarietyCode GetCode(string id = "BeSys0", string version = DefaultNinVersion)
        {
            return _varietyService.GetByKode(base.DbContext, id, GetHostPath(), base.GetVersion(version));
        }
    }
}
