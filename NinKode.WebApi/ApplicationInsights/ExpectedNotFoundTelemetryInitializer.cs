using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
namespace NinKode.WebApi.ApplicationInsights;

public class ExpectedNotFoundTelemetryInitializer : ITelemetryInitializer
{
    private readonly ILogger<ExpectedNotFoundTelemetryInitializer> _logger;

    public ExpectedNotFoundTelemetryInitializer(ILogger<ExpectedNotFoundTelemetryInitializer> logger)
    {
        _logger = logger;
    }

    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is not RequestTelemetry requestTelemetry) 
            return;

        if (requestTelemetry.ResponseCode == "404")
        {
            var isExpectedNotFound = requestTelemetry.Properties.ContainsKey("ExpectedNotFound") ||
                                    IsExpectedNotFoundEndpoint(requestTelemetry.Url?.AbsolutePath) ||
                                    IsExpectedNotFoundEndpoint(requestTelemetry.Name);

            if (isExpectedNotFound)
            {           
                requestTelemetry.Success = true;            
                requestTelemetry.Properties["ExpectedNotFound"] = "true";
                requestTelemetry.Properties["TelemetryProcessedBy"] = "ExpectedNotFoundTelemetryInitializer";
                requestTelemetry.Properties["OriginalUrl"] = requestTelemetry.Url?.ToString() ?? "N/A";
                requestTelemetry.Properties["Timestamp"] = DateTimeOffset.UtcNow.ToString("O");          

                requestTelemetry.Context.Operation.SyntheticSource = "ExpectedBusinessLogic";
                
                _logger.LogInformation("Processed expected 404 for URL: {Url}", requestTelemetry.Url?.AbsolutePath);
            }
        }
    }


    private static bool IsExpectedNotFoundEndpoint(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        var expectedNotFoundPatterns = new[]
        {           
            "/hentkode/",
            "/koder/hentkode/",
            "hentkode",
            "/v2.3/koder/hentkode/",
            "/v2.2/koder/hentkode/", 
            "/v2.1b/koder/hentkode/",
            "/v2.1/koder/hentkode/",
            "/v2/koder/hentkode/",
            "/v1/koder/hentkode/",            
            "/api/koder/hentkode/",
            "/api/v1/koder/hentkode/",            
            "GET koder/hentkode",
            "GET /koder/hentkode",
            "GetCode"
        };

        return expectedNotFoundPatterns.Any(pattern => 
            path.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}