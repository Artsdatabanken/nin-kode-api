# Application Insights upgrade

- Updated `Microsoft.ApplicationInsights.AspNetCore` centrally from 2.23.0 to 3.2.0; existing non-Development registration remains unchanged. The web project builds.
- Started the web host in Staging with a dummy connection string pointed exclusively at a local listener. `/versjoner` and Swagger documents v2.1b and v3.0 returned HTTP 200, and the listener received an HTTP POST to `/v2.1/track`. This confirms local export activity, not the contents or equivalence of individual telemetry items.
- Version 3 uses OpenTelemetry instrumentation and removes APIs from v2. No custom usage was found in source. Actual hosting, deployment settings, telemetry ingestion, request/dependency coverage, sampling, dimensions, and production parity remain unverified and require deployment-specific validation before rollout.
