using Microsoft.AspNetCore.Mvc;
using NinKode.Common.Interfaces;

namespace NinKode.WebApi.Controllers.Initial;

[ApiVersion("2.3")]
[ApiVersion("2.2", Deprecated = true)]
[ApiVersion("2.1b", Deprecated = true)]
[ApiVersion("2.1", Deprecated = true)]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
[Route("versjoner"), Tags("Versjon")]
public class VersionController : ApiControllerBase<VersionController>
{
    private readonly IVersionService _versionService;

    public VersionController(IVersionService versionService)
    {
        _versionService = versionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public IActionResult GetVersions()
    {
        var versions = _versionService.GetVersions(DbContext);

        return Ok(versions);
    }
}