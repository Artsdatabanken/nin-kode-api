# 01-ravendb-client: Upgrade RavenDB.Client

Evaluate existing RavenDB client usage and deployed server compatibility; upgrade centrally from 7.0.2 to 7.2.8 if safe. Review `apidiff/RavenDB.Client.apidiff.md` and migrate any affected usage. Record any unverified server compatibility without claiming runtime validation.

## Research
- `NinKode.Database` is the only direct RavenDB.Client consumer; its version is centralized in `Directory.Packages.props` and flows to NiN.Console and NinKode.WebApi via project references. The 7.2.8 API diff identifies two removed types, 32 removed members, and five changed signatures; no corresponding active uses were identified in the project.
- Compiled client operations are in private methods of an importer whose entry point is commented out. Legacy service database initialization is disabled. `NiN.Console/README.md` mentions a RavenDB 3.5 server; its current deployment status is unknown and no server was accessed. The existing client already targets 7.0.2. A successful build alone cannot establish compatibility with any deployed server.

**Done when**: The supported client upgrade builds and relevant behavior is validated, or a concrete compatibility blocker is documented.
