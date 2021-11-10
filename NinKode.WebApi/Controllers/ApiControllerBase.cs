namespace NinKode.WebApi.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using NiN.Database;

    public abstract class ApiControllerBase<T> : Controller where T : ApiControllerBase<T>
    {
        private NiNDbContext _dbContext;

        protected NiNDbContext DbContext => _dbContext ??= HttpContext.RequestServices.GetService<NiNDbContext>();

        internal string GetHostPath()
        {
            var path = Request.Path.Value;
            if (path == null) return null;

            path = path[..(path.LastIndexOf("/", StringComparison.Ordinal) + 1)];
            var protocol = Request.IsHttps ? "s" : "";
            return $"http{protocol}://{Request.Host}{path}";
        }

        internal string GetVersion(string defaultVersion)
        {
            var version = RouteData.Values["version"]?.ToString();
            if (string.IsNullOrWhiteSpace(version)) version = defaultVersion;

            if (version.StartsWith("v")) version = version[1..];

            if (version.Equals("2b")) version = "2.1b";

            return version;
        }
    }
}
