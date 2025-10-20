using Microsoft.ApplicationInsights.Extensibility;
namespace NinKode.WebApi.ApplicationInsights;

public static class ApplicationInsightsConfiguration
{
    public static void ConfigureApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        var aiConnectionString = configuration.GetConnectionString("ApplicationInsights") 
                                ?? configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        
        if (!string.IsNullOrEmpty(aiConnectionString))
        {
            services.AddSingleton<ITelemetryInitializer>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ExpectedNotFoundTelemetryInitializer>>();
                return new ExpectedNotFoundTelemetryInitializer(logger);
            });

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.EnableAdaptiveSampling = true;
                options.EnableQuickPulseMetricStream = false;
                options.EnableAuthenticationTrackingJavaScript = false;
            });
        }
    }
}