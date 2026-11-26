# 03.03-intermediate-project-compatibility: Resolve and validate intermediate project compatibility

## Objective
Resolve .NET 11 compatibility findings in intermediate projects after their dependencies.

## Scope
- Projects: NiN.Test_CmsImpExp, NiN.ExportImport, Infrastructure.Mapping, NinKode.Common, NiN3.Infrastructure.
- Key findings: MemoryStream behavior in NinCodeExport.cs (lines 43 and 162) and RapportService.cs (line 159); Uri construction in SService.cs (lines 21 and 35); the public Uri property in Codes.cs (lines 34-36).

## Research Findings
- Queried assessment summaries/issues for all five scoped projects. The assessment snapshot predates the 03.01 transition and still reports net9.0/old package versions; current project references and central package versions were verified with `get_project_dependencies`.
- NiN.ExportImport and NiN3.Infrastructure use default expandable `MemoryStream` instances for CSV/ZIP generation and workbook output. The Uri findings are in NiN3.Infrastructure.Services.SService (one constructor is commented; the active one receives a fixed absolute HTTPS Key Vault URI) and the System.Uri-typed `Codes.Beskrivelse` serialization contract. No assessment detail establishes a changed behavior for these call patterns, and the constructors remain supported, so preserve the current implementations pending runtime/API evidence.
- NiN3.Infrastructure directly references Azure.Identity 1.13.2, which the assessment flags as deprecated because it depends on an obsolete MSAL version. The target-framework-aware resolver reports stable Azure.Identity 1.21.0 for net11; update the central version, then validate the project and its dependents.
- The scoped project graph is NiN3.Infrastructure -> NiN3.Core and NiN3.Infrastructure.Mapping; NiN.ExportImport and NinKode.Common depend on shared libraries. Existing NiN.Test_CmsImpExp remains the relevant regression suite for the CSV/export path.

## Steps
1. Review the changed APIs and verify no behavior-sensitive fixes are needed beyond code that fails to compile.
2. Preserve existing behavior and make targeted compatibility edits.
3. Build projects in dependency order and run the NiN.Test_CmsImpExp tests.

## Done when
All scoped projects compile on net11.0 and applicable tests pass or their known baseline failures are recorded.
