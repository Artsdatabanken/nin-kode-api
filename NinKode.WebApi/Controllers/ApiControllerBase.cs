namespace NinKode.WebApi.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    public class ApiControllerBase : ControllerBase
    {
        internal string GetHostPath()
        {
            var path = Request.Path.Value;
            path = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var protocol = Request.IsHttps ? "s" : "";
            return $"http{protocol}://{Request.Host}{path}";
        }

        internal string GetVersion(string defaultVersion)
        {
            var version = RouteData.Values["version"]?.ToString();
            if (string.IsNullOrWhiteSpace(version)) version = defaultVersion;

            if (version.StartsWith("v")) version = version[1..];

            return version;
        }
    }
}
