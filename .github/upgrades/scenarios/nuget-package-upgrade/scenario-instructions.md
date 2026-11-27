# NuGet Package Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Scope**: Entire solution
- **Packages**: API versioning and API Explorer, Swashbuckle.AspNetCore, Microsoft.ApplicationInsights.AspNetCore, Microsoft.NET.Test.Sdk, xunit.runner.visualstudio, coverlet.collector, RavenDB.Client; assess xUnit v3 migration.
- **Version Policy**: Latest supported stable versions, with RavenDB server compatibility considered.
- **AutoMapper**: Keep version 14.0.0 unchanged.
- **Assessment**: Quick; do not run the optional full breaking-change scan.
- **Package management**: Central Package Management; unified target versions, with no excluded projects.
- **Targets**: RavenDB.Client 7.2.8; coverlet.collector 10.1.1; Microsoft.NET.Test.Sdk 18.12.0; xunit.runner.visualstudio 4.0.1; Swashbuckle.AspNetCore 11.0.1; Microsoft.ApplicationInsights.AspNetCore 3.2.0. Migrate deprecated versioning packages and assess xUnit v3 separately.
- **Breaking changes**: Migrate affected usage; no semantic full scan unless requested. Validate source and runtime behavior rather than relying solely on API-diff output.
- **API versioning**: Try the compatibility-adapter approach to preserve existing `2.1b` URLs and query values while migrating to `Asp.Versioning`.
- **Historical route requirement**: Preserve the existing `v2.1b` route exactly while adding any new API-version route format; do not make a global formatting change that alters it.
- **Accepted API versioning differences**: Malformed version queries return HTTP 400 instead of the default version; unsupported URL versions return HTTP 404 rather than 400; some versioning error details move within the JSON response. Accepted by the user on 2026-11-26.

## Source Control
- **Source Branch**: upgrade-dotnet-11
- **Working Branch**: nuget-package-upgrade
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)
