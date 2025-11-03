using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace NinKode.WebApi.Filters;

/// <summary> 
/// Custom TelemetryInitializer that overrides the default sdk behavior of treating certain response codes as failed requests
/// https://stackoverflow.com/questions/37533431/how-to-tell-application-insights-to-ignore-404-responses
/// https://learn.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling#addmodify-properties-itelemetryinitializer
/// </summary>
public class TelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is not RequestTelemetry requestTelemetry)
            return;

        // check for 401 "Unauthorized", 404 "Not Found" or 499 "Client Closed Request"
        string[] ignoredResponseCodes = ["401", "404", "499"];

        if (!ignoredResponseCodes.Contains(requestTelemetry.ResponseCode))
            return;

        // if we set the success property, the sdk won't change it
        requestTelemetry.Success = true;

        // allow us to filter these requests in the portal
        requestTelemetry.Properties["Overridden400s"] = "true";
    }
}