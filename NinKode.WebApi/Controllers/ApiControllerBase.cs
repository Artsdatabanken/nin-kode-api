using Microsoft.AspNetCore.Mvc;
using NiN.Database;

namespace NinKode.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase<T> : ControllerBase where T : ApiControllerBase<T>
{
    private NiNDbContext _dbContext;

    protected NiNDbContext DbContext => _dbContext ??= HttpContext.RequestServices.GetService<NiNDbContext>();

    protected string GetHostPath()
    {
        var path = Request.Path.Value;
        
        if (path == null) 
            return null;

        path = path[..(path.LastIndexOf("/", StringComparison.Ordinal) + 1)];
        
        var protocol = Request.IsHttps ? "s" : "";
        
        return $"http{protocol}://{Request.Host}{path}";
    }

    protected string GetVersion()
    {
        var requestedApiVersion = HttpContext.GetRequestedApiVersion() ?? throw new Exception("ApiVersion is required");
        var apiVersion = requestedApiVersion.ToString("VVS");

        if (apiVersion.EndsWith(".0"))
            apiVersion = apiVersion.Replace(".0", string.Empty);

        return apiVersion;
    }
}