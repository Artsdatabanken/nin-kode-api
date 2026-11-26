# 03-upgrade-solution: Upgrade all projects to .NET 11

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
