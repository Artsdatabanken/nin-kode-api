# Progress: .NET 11 framework and package transition

- Retargeted all 13 projects from `net9.0` to `net11.0` and centrally aligned the framework package families. AutoMapper remains pinned at 14.0.0; Newtonsoft.Json is 13.0.4.
- Removed the redundant `System.Net.Http` package from NiN.ExportImport. NuGet restore also identified `System.Text.Json` (NiN.Console, NinKode.Common) and `System.Text.Encoding.CodePages` (NinKode.WebApi) as framework-provided and unnecessary, so their references and central pins were removed.
- NuGet audit traced vulnerable transitive `System.Security.Cryptography.Pkcs`/`.Xml` 9.0.3 to EPPlus 8.0.3. Updated EPPlus centrally to stable 8.7.3; a solution audit no longer reports those crypto packages as vulnerable. AutoMapper 14.0.0 continues to report high-severity advisories and is deliberately unchanged per the user's explicit requirement.
- `dotnet restore .\NinKodeApi.sln`: succeeded. Remaining restore warnings are AutoMapper's advisories (user-constrained) and RavenDB.Database 3.5.9's unsupported target/client-version mismatch (deferred to task 04).
- Full solution build: succeeded.
- `NiN.Test_CmsImpExp`: passed 17/17. `NiN3.Tests`: 62/80 passed; the same 18 known fixture/data assertion failures were observed in the pre-upgrade baseline. Created the expected `NiN3.Console\temp` directory for the run, then removed the generated log and directory.
- Test Explorer's prior project filter did not target test assemblies correctly, so tests were executed with explicit `dotnet test` commands.
- No production API compatibility edits were required for this package-transition subtask; API review/fixes remain in the subsequent solution-upgrade subtasks.
