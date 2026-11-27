# NuGet Package Upgrade Plan

## Overview

**Target**: Upgrade the five requested package groups to supported stable versions while retaining AutoMapper 14.0.0.
**Scope**: All 13 solution projects; package versions are centrally managed.

## Tasks

### 01-ravendb-client: Upgrade RavenDB.Client

Evaluate existing RavenDB client usage and deployed server compatibility; upgrade centrally from 7.0.2 to 7.2.8 if safe. Review `apidiff/RavenDB.Client.apidiff.md` and migrate any affected usage. Record any unverified server compatibility without claiming runtime validation.

**Done when**: The supported client upgrade builds and relevant behavior is validated, or a concrete compatibility blocker is documented.

### 02-test-tooling: Upgrade test tooling

Upgrade Microsoft.NET.Test.Sdk 17.14.1 to 18.12.0, xunit.runner.visualstudio 3.1.0 to 4.0.1 and coverlet.collector 6.0.4 to 10.1.1 in `Directory.Packages.props`; review corresponding `apidiff` files. Assess xUnit v3 separately and migrate only if compatible with the project's tests and runner. Preserve test discovery and compare results to the existing baseline.

**Done when**: Test suites discover and run without new failures and coverage collection is checked, or a concrete blocker is documented.

### 03-api-versioning: Migrate API versioning packages

Replace deprecated Microsoft.AspNetCore.Mvc.Versioning and its ApiExplorer package in NinKode.WebApi with supported Asp.Versioning equivalents, adjust startup and controllers, and preserve route versions and API Explorer/Swagger behavior.

**Done when**: The web application builds and versioned endpoints and API metadata retain their behavior, or a concrete blocker is documented.

### 04-swashbuckle: Upgrade Swagger tooling

Upgrade Swashbuckle.AspNetCore from 8.1.1 to 11.0.1 centrally. Review `apidiff/Swashbuckle.AspNetCore.apidiff.md`, handle transitive OpenAPI API changes and verify generated versioned Swagger documents.

**Done when**: The web application builds and Swagger documents preserve routes, schemas and response metadata, or a concrete blocker is documented.

### 05-application-insights: Upgrade telemetry

Upgrade Microsoft.ApplicationInsights.AspNetCore from 2.23.0 to 3.2.0 centrally. Review `apidiff/Microsoft.ApplicationInsights.AspNetCore.apidiff.md`, migrate affected APIs and preserve telemetry registration and configuration; distinguish local checks from deployed telemetry verification.

**Done when**: The web host builds and telemetry configuration is validated to the extent possible, with outstanding deployment checks documented.

### 06-solution-validation: Validate upgrades

Restore and build the solution, run all discovered tests and coverage collection, compare the NiN3 failures to baseline, verify AutoMapper remains 14.0.0, and document remaining security and runtime limitations.

**Done when**: Build succeeds, suites have no new failures, runtime checks are reported accurately, and remaining blockers are documented.
