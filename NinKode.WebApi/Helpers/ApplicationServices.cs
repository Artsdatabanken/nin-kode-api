using NinKode.Common.Interfaces;
using NinKode.Database.Services;

namespace NinKode.WebApi.Helpers;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ICodeService, CodeService>();
        services.AddSingleton<IVarietyService, VarietyService>();
        services.AddSingleton<IExportService, ExportService>();
        services.AddSingleton<IImportService, ImportService>();
        services.AddSingleton<IVersionService, VersionService>();
    }
}
