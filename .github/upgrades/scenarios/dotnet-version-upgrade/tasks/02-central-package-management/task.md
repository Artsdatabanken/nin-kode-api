# 02-central-package-management: Centralize package versions

Introduce Central Package Management for the 13 SDK-style projects by moving package version declarations into `Directory.Packages.props` while retaining each project's existing package set. Keep AutoMapper pinned at **14.0.0** as requested; the assessment flags that version as vulnerable, so this constraint leaves that finding unresolved and must not be silently changed.

## Research Findings

- No `Directory.Packages.props` exists at the repository root; the assessment marks all 13 projects as SDK-style.
- The solution baseline now succeeds: `dotnet build NinKodeApi.sln` reports 0 errors and 596 existing warnings, including the AutoMapper 14.0.0 NU1903 security advisory; the warning is retained per the user's explicit pin.
- The .NET 11 `dotnet package list --project NinKodeApi.sln --format json` snapshot covers all 13 projects and 45 distinct top-level package IDs. The audit found one resolved version per package ID, so no CPM conflict decisions or `VersionOverride` exceptions are needed.
- Package references are defined directly in the 13 SDK-style project files; no imported custom package references, version properties, active conditional package references, existing `VersionOverride` entries, or existing `Directory.Packages.props` were found.
- Exact baseline package versions are captured in `cpm-artifacts\baseline-packages.json`; the successful solution build log is `cpm-artifacts\baseline.binlog`. Dependency inventories confirm AutoMapper 14.0.0 in all 13 projects.
- Package file changes should only remove active `Version` attributes and transfer their unchanged versions to the root `Directory.Packages.props`; preserve each project's `PrivateAssets` and `IncludeAssets` metadata and leave commented legacy package references untouched.

**Done when**: Package versions are centrally managed, all project references resolve, and AutoMapper remains exactly 14.0.0.
