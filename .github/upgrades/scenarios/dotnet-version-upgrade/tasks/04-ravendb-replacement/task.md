# 04-ravendb-replacement: Resolve deferred RavenDB package compatibility

Replace the temporary compatibility shim associated with RavenDB.Database 3.5.9 in `NiN.Console`, `NinKode.Database`, and `NinKode.WebApi` with a supported implementation. Research existing package API usage across all three consumers and identify a viable replacement before changing behavior; do not leave a stub or compile-only fallback as the finished result.

## Research Findings
- `NinKode.Database.csproj` is the only project that directly references RavenDB.Database 3.5.9; `NiN.Console` and `NinKode.WebApi` receive it transitively through their reference to NinKode.Database. Its central version is in `Directory.Packages.props`.
- The three consumers have no active RavenDB.Database calls. `NinKode.Database.Program.Main` exits immediately; the older server-side code in it and in the importer/services is commented out. The remaining compiled Raven client session/document types resolve from RavenDB.Client 7.0.2. `RavenJObject` and `Raven.Abstractions.Indexing.IndexDefinition` resolve from RavenDB.Database but appear only in commented code; the corresponding `using` directives are the only active references to its legacy namespaces.
- The currently executed legacy code reads local JSON rather than RavenDB. Keep RavenDB.Client 7.0.2 and preserve existing runtime behavior; remove the obsolete server package and only the now-unneeded legacy namespace imports, then validate the three consumers and solution.

**Done when**: All three projects no longer depend on RavenDB.Database 3.5.9 or temporary stubs, and affected functionality is implemented against a compatible supported dependency or documented as a concrete blocker.
