using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using NiN3.Core.Models.DTOs;
//using NiN3.Core.Models.DTOs.type;
//using NiN3.Infrastructure.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NinKode.WebApi.Controllers._3_0
{
    [ApiVersion("3.0")]
    [ApiController]
    //[Route("typer"), Tags("Typekoder")]
    [Route("v{version:apiVersion}/typer"), Tags("Typekoder")]
    public class TypeApiController : ApiControllerBase<TypeApiController>
    {
        [HttpGet]
        [Route("test")]
        public IActionResult Index()
        {
            return Ok("hello");
        }
    }
}
