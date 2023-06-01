using Microsoft.EntityFrameworkCore;
using NiN.Database;
using NinKode.Common.Interfaces;
using NinKode.Database.Services;
using NinKode.WebApi.Helpers.Swagger;

namespace NinKode.WebApi.Helpers;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        // database
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Could not find 'NinApiConnectionString'");
        services.AddDbContext<NiNDbContext>(o =>
        {
            o.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(NiNDbContext).Module.Name));
        });

        services.AddSingleton<ICodeService, CodeService>();
        services.AddSingleton<IVarietyService, VarietyService>();
        services.AddSingleton<IExportService, ExportService>();
        services.AddSingleton<IImportService, ImportService>();
        services.AddSingleton<IVersionService, VersionService>();

        // API versioning https://github.com/dotnet/aspnet-api-versioning
        services.ConfigureVersioning();

        // Swagger
        services.AddSwagger();
        services.ConfigureOptions<SwaggerOptions>();
    }
}