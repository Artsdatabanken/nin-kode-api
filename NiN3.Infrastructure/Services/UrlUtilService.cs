using Microsoft.AspNetCore.Http;

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
