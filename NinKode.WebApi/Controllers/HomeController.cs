namespace NinKode.WebApi.Controllers
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc;

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Redirect("../swagger/index.html");
        }

        [HttpGet]
        [Route("css/{id}")]
        public FileStreamResult Css(string id)
        {
            if (string.IsNullOrEmpty(id)) return new FileStreamResult(Stream.Null, "text/css");

            var path = Assembly.GetExecutingAssembly().Location;
            if (path.IndexOf("/", StringComparison.Ordinal) >= 0)
            {
                path = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal));
            }
            else if (path.IndexOf("\\", StringComparison.Ordinal) >= 0)
            {
                path = path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal));
            }
            else
            {
                return new FileStreamResult(Stream.Null, "text/css");
            }
            path = Path.Combine(path, "Swagger", "themes", id);

            return System.IO.File.Exists(path)
                ? new FileStreamResult(System.IO.File.OpenRead(path), "text/css")
                : new FileStreamResult(Stream.Null, "text/css");
        }
    }
}
