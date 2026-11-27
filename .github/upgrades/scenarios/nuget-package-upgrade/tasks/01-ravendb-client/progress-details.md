# RavenDB.Client upgrade

- Updated the central RavenDB.Client version from 7.0.2 to 7.2.8; no source changes were required.
- Visual Studio build of `NinKode.Database` succeeded. The compiled Raven session helpers have no active importer entry point and no corresponding runtime tests were found.
- Server compatibility is not verified: `NiN.Console/README.md` references legacy RavenDB 3.5 databases but the deployed server/version is unknown. Do not rely on the client for a live server until the actual server version and operations are validated. The previous package was already 7.0.2.
- AutoMapper remains 14.0.0; its known security advisories remain outside this task.
