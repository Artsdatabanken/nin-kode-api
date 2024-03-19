using Microsoft.AspNetCore.Mvc;
using NinKode.Common.Interfaces;
using NinKode.Common.Models.Code;

namespace NinKode.WebApi.Controllers.Initial;

[ApiVersion("2.3")]
[ApiVersion("2.2", Deprecated = true)]
[ApiVersion("2.1b", Deprecated = true)]
[ApiVersion("2.1", Deprecated = true)]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
[Route("v{version:apiVersion}/koder"), Tags("Kode")]
public class CodeController : ApiControllerBase<CodeController>
{
    private readonly ICodeService _codeService;

    public CodeController(ICodeService codeService)
    {
        _codeService = codeService;
    }

    [HttpGet]
    [Route("allekoder")]
    [ProducesResponseType(typeof(IEnumerable<Codes>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var allCodes = _codeService.GetAll(DbContext, GetHostPath(), GetVersion());
        
        return Ok(allCodes);
    }

    /// <param name="id">NiN-kode</param>
    [HttpGet]
    [Route("hentkode/{id:required}")]
    [ProducesResponseType(typeof(Codes), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCode(string id = "NA")
    {
        var codes = _codeService.GetByKode(DbContext, id, GetHostPath(), GetVersion());

        if (codes == null)
            return NotFound();

        return Ok(codes);
    }
}