using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NinKode.Common.Utilities;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace NinKode.WebApi.Helpers.Swagger;

public class SwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "NinKode API",
            Version = description.ApiVersion.ToString(),
            Description =
                $"<i>BuildTime: {NorwayDateTime.Convert(File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location)):yyyy-MM-dd HH:mm:ss}</i>"
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}