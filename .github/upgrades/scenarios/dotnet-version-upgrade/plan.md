# .NET Version Upgrade Plan

## Overview

**Target**: .NET 11 (`net11.0`)
**Scope**: All 13 projects in `NinKodeApi.sln` (270 code files, 19,623 LOC)

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 13 low-difficulty projects all target .NET 9, with a dependency depth of three.

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Upgrade Strategy | All-at-Once | 13 low-difficulty projects all target .NET 9, with a dependency depth of three. |
| Package Management | Central Package Management | All 13 projects are SDK-style, there is no `Directory.Packages.props`, and the solution is a modern-to-modern upgrade. |
| Unsupported Packages | Defer Resolution | RavenDB.Database 3.5.9 is incompatible and is referenced by three projects; the assessment identifies no replacement, so its research is deferred. |
| Unsupported API Handling | Fix Inline | The assessment flags 4 binary- and 3 source-incompatible APIs, a manageable set to address during the solution upgrade. |

## Tasks

### 01-prerequisites: Verify .NET 11 build prerequisites

Confirm the installed .NET SDK and build tooling can target `net11.0`, and check repository-wide SDK configuration such as `global.json`. The complete solution depends on the selected SDK being available before project targets or package files are changed.

**Done when**: The required .NET 11 SDK/toolchain is available and repository SDK configuration is compatible, or a specific blocker is documented before proceeding.

### 02-central-package-management: Centralize package versions

Introduce Central Package Management for the 13 SDK-style projects by moving package version declarations into `Directory.Packages.props` while retaining each project's existing package set. Keep AutoMapper pinned at **14.0.0** as requested; the assessment flags that version as vulnerable, so this constraint leaves that finding unresolved and must not be silently changed.

**Done when**: Package versions are centrally managed, all project references resolve, and AutoMapper remains exactly 14.0.0.

### 03-upgrade-solution: Upgrade all projects to .NET 11

Retarget all 13 projects together from `net9.0` to `net11.0` and align compatible framework and package dependencies across the solution. Resolve the reported binary/source API incompatibilities and assess behavior changes, including the 33 API incidents reported for `NinKode.WebApi`. The assessment identifies RavenDB.Database 3.5.9 as incompatible in three projects: `NiN.Console`, `NinKode.Database`, and `NinKode.WebApi`. Under the confirmed deferred-resolution approach, keep any temporary compile shim clearly scoped for follow-up rather than treating it as a final replacement.

The complete project set is `NiN.CmsImpExp`, `NiN.Console`, `NiN.Database`, `NiN.ExportImport`, `NiN.Test_CmsImpExp`, `NiN3.Console`, `NiN3.Core`, `NiN3.Infrastructure`, `NiN3.Infrastructure.Mapping`, `NiN3.Tests`, `NinKode.Common`, `NinKode.Database`, and `NinKode.WebApi`. See `assessment.md` for per-project package and API findings. Do not upgrade AutoMapper from 14.0.0.

**Done when**: Every project targets `net11.0`, dependencies restore, and the full solution compiles with framework/API changes applied and any deferred package shim explicitly identified.

### 03.01-framework-package-transition: Retarget all projects and align framework packages

Apply the all-at-once .NET 11 transition to all 13 projects, align compatible framework package families, preserve AutoMapper 14.0.0, and document the deferred RavenDB compatibility warnings.

**Done when**: Every project targets `net11.0`, the framework/package transition restores and builds, and AutoMapper remains 14.0.0.

### 03.02-leaf-project-compatibility: Resolve leaf project compatibility findings

Review and fix verified API compatibility findings in NiN.CmsImpExp, NiN.Database, and NiN3.Core, preserving existing behavior and validating affected tests.

**Done when**: The affected leaf projects build on net11.0, source-incompatible APIs are resolved, and behavioral findings are reviewed or documented.

### 03.03-intermediate-project-compatibility: Resolve intermediate project compatibility findings

Review and fix verified API compatibility findings in NiN.ExportImport, NiN3.Infrastructure, NiN3.Infrastructure.Mapping, and NinKode.Common, preserving existing behavior and validating dependents.

**Done when**: The affected intermediate projects build on net11.0, verified source/binary incompatibilities are resolved, and behavioral findings are reviewed or documented.

### 03.04-host-test-compatibility: Resolve host and test compatibility findings

Review and fix verified host/test compatibility findings, including configuration, path, logging, and web API metadata behavior. Keep AutoMapper at 14.0.0 and compare test outcomes with the documented baseline.

**Done when**: Affected host/test projects build on net11.0, source/binary incompatibilities are resolved, Web API metadata behavior is assessed, and remaining test/security limitations are recorded.

### 04-ravendb-replacement: Resolve deferred RavenDB package compatibility

Replace the temporary compatibility shim associated with RavenDB.Database 3.5.9 in `NiN.Console`, `NinKode.Database`, and `NinKode.WebApi` with a supported implementation. Research existing package API usage across all three consumers and identify a viable replacement before changing behavior; do not leave a stub or compile-only fallback as the finished result.

**Done when**: All three projects no longer depend on RavenDB.Database 3.5.9 or temporary stubs, and affected functionality is implemented against a compatible supported dependency or documented as a concrete blocker.

### 05-solution-validation: Validate the upgraded solution

Run the complete solution build and test suites after package and code work is complete. Exercise the affected web API and database paths through the existing tests, and record any assessment findings that cannot be resolved—especially the retained AutoMapper 14.0.0 vulnerability.

**Done when**: The full solution builds without errors, all available tests pass, and remaining known package/security findings are explicitly documented.
