using Microsoft.AspNetCore.Mvc;
using NinKode.Common.Interfaces;
using NinKode.Common.Models.Variety;

namespace NinKode.WebApi.Controllers.Initial;

[ApiVersion("2.3")]
[ApiVersion("2.2", Deprecated = true)]
[ApiVersion("2.1b", Deprecated = true)]
[ApiVersion("2.1", Deprecated = true)]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
[Route("v{version:apiVersion}/variasjon"), Tags("Variasjon")]
public class VarietyController : ApiControllerBase<VarietyController>
{
    private readonly IVarietyService _varietyService;

    public VarietyController(IVarietyService varietyService)
    {
        _varietyService = varietyService;
    }

    [HttpGet]
    [Route("allekoder")]
    [ProducesResponseType(typeof(List<VarietyAllCodes>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var allCodes = _varietyService.GetAll(DbContext, GetHostPath(), GetVersion());

        return Ok(allCodes);
    }

    /// <param name="id">Beskrivelsessystem kode</param>
    [HttpGet]
    [Route("hentkode/{id:required}")] 
    [ProducesResponseType(typeof(VarietyCode), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCode(string id = "BeSys0")
    {
        var varietyCode = _varietyService.GetByKode(DbContext, id, GetHostPath(), GetVersion());
        
        if (varietyCode == null)
            return NotFound();

        return Ok(varietyCode);
    }
}