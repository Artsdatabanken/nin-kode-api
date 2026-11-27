using Asp.Versioning;
using Asp.Versioning.Conventions;

namespace NinKode.WebApi.Helpers;

public static class VersioningConfiguration
{
    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddErrorObjects().AddProblemDetails();
        services.AddSingleton<IApiVersionParser, LegacyApiVersionParser>();
        services
            .AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddMvc(options => options.Conventions.Add(new VersionByNamespaceConvention()))
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVS";
                options.SubstitutionFormat = "VVS";
                options.SubstituteApiVersionInUrl = true;
            });
    }   
}