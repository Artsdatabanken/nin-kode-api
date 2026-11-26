# 03.01-framework-package-transition: Retarget all projects and align framework packages

## Objective
Apply the selected all-at-once project transition to .NET 11 and align centrally managed framework package versions.

## Scope
- Update all 13 project TFMs from net9.0 to net11.0 in one batch.
- Update supported EF Core, ASP.NET Core testing, Microsoft.Extensions, System.Text.Json, and System.Text.Encoding.CodePages package families to stable 11.0.0 where supported. Update Newtonsoft.Json to 13.0.4. Keep AutoMapper at 14.0.0.
- Remove the redundant System.Net.Http reference from NiN.ExportImport and its orphaned central version. Retain the deferred RavenDB.Database package unless restore proves a temporary shim is required.

## Steps
1. Recheck target-framework properties and the CPM manifest before editing.
2. Apply all TFM and package updates together.
3. Restore the solution; record any RavenDB compatibility blocker and resolve only the compile prerequisite in a clearly marked, scoped way if required.

## Research Findings

- Reconfirmed the 13 project files each define a single `<TargetFramework>net9.0</TargetFramework>`; `rg` found no `Directory.Build.*` file or shared TFM override. The project set is listed in the parent task's Scope Inventory.
- Reconfirmed root `Directory.Packages.props` has `ManagePackageVersionsCentrally=true` and 45 central package versions; active references are versionless. `get_project_dependencies` confirms CPM on `NinKode.WebApi`; its project references are `NinKode.Common`, `NinKode.Database`, `NiN.Database`, and `NiN3.Infrastructure`.
- Target-framework-aware stable package resolution for `net11.0` returned 11.0.0 for EF Core, EF Core Design/Proxies/Sqlite/Tools/Abstractions, ASP.NET Core MVC Testing/TestHost, Microsoft.Extensions.Configuration and its Binder/EnvironmentVariables/Json/UserSecrets components, Logging.Console, System.Text.Json, and System.Text.Encoding.CodePages. Newtonsoft.Json resolves to 13.0.4.
- Central entries currently hold 9.0.4 for these 11.x framework package families and System.Text packages; `Newtonsoft.Json` is 13.0.3. Set only those framework-coupled versions to their supported target versions; leave unrelated packages unchanged unless restore/audit identifies a directly related compatibility or security issue.
- `NiN.ExportImport.csproj` has the sole direct `System.Net.Http` PackageReference and `Directory.Packages.props` has its central version. Remove both; framework functionality replaces the package.
- Preserve AutoMapper 14.0.0. The deferred RavenDB.Database 3.5.9 has no supported net11 package version; keep the package for this step unless the actual restore blocks, and defer its real replacement to task 04.
- Post-upgrade NuGet audit identified vulnerable transitive `System.Security.Cryptography.Pkcs` and `.Xml` 9.0.3 packages, introduced by EPPlus 8.0.3. `dotnet nuget why` confirms EPPlus as the source; the stable supported EPPlus version is 8.7.3, so update its central version to eliminate those advisories. AutoMapper 14.0.0 also has high-severity advisories, but remains unchanged per the user's explicit constraint.
- Restore also flagged direct `System.Text.Json` references in NiN.Console/NinKode.Common and `System.Text.Encoding.CodePages` in NinKode.WebApi as redundant on net11. Removed those references and their now-unused central versions. Remaining expected restore warnings are AutoMapper's advisories (user-constrained) and RavenDB.Database 3.5.9 compatibility/version conflicts (deferred to task 04).
- Existing working-tree changes before this task are the completed CPM conversion (all 13 csproj files and `Directory.Packages.props`) plus scenario artifacts. The branch is `upgrade-dotnet-11`; do not revert those changes.

## Done when
All project files declare net11.0, package versions resolve consistently, AutoMapper remains 14.0.0, and the restore outcome/blockers are recorded.
