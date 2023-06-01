using Microsoft.AspNetCore.Mvc.Versioning.Conventions;

namespace NinKode.WebApi.Helpers;

public static class VersioningConfiguration
{
    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.Conventions.Add(new VersionByNamespaceConvention());
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVS";
                options.SubstitutionFormat = "VVS";
                options.SubstituteApiVersionInUrl = true;
            });
    }   
}