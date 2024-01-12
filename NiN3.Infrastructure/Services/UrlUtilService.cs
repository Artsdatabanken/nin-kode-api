using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.Services
{
    public class UrlUtilService : IUrlUtilService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlUtilService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRootUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var scheme = request.Scheme;

            return $"{scheme}://{host}";
        }
    }
}
