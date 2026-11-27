# 05-application-insights: Upgrade telemetry

Upgrade Microsoft.ApplicationInsights.AspNetCore from 2.23.0 to 3.2.0 centrally. Review `apidiff/Microsoft.ApplicationInsights.AspNetCore.apidiff.md`, migrate affected APIs and preserve telemetry registration and configuration; distinguish local checks from deployed telemetry verification.

## Research
- `NinKode.WebApi` alone references the centrally managed package. `Startup.cs` calls `AddApplicationInsightsTelemetry()` only outside Development. No custom TelemetryClient, initializer, or processor calls were found in source.
- The quick diff shows 22 removed types and 39 removed members; the 3.2.0 package depends on OpenTelemetry instrumentation, so successful compilation does not establish parity of collected telemetry or configuration. No Application Insights settings were found in the repository's web appsettings files; deployment configuration and hosting are unknown.
- Validate build and non-Development startup locally without credentials or sending real telemetry. Deployed telemetry, sampling, configuration and instrumentation behavior require separate verification in the actual hosting environment.

**Done when**: The web host builds and telemetry configuration is validated to the extent possible, with outstanding deployment checks documented.
