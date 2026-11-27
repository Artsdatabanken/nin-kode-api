# 06-solution-validation: Validate upgrades

Restore and build the solution, run all discovered tests and coverage collection, compare the NiN3 failures to baseline, verify AutoMapper remains 14.0.0, and document remaining security and runtime limitations.

## Research
- Current changes cover central RavenDB, test tooling, Application Insights and Swashbuckle versions plus web OpenAPI filters. API versioning remains at 5.1.0 because the replacement rejects published 2.1b. xUnit itself remains 2.9.3.
- Prior baseline records CMS 17/17 and NiN3 62/80; no earlier list of failed names was found. A new named TRX exists from the tooling pass, so a final run can be compared to that exact set, while the original baseline can only be compared by counts.
- Earlier checks generated a coverage attachment, all seven Swagger groups responded with equivalent operation/schema/response metadata except for new top-level tags, and staging telemetry made a local POST. Deployed telemetry and RavenDB server compatibility remain unverified. AutoMapper must remain 14.0.0 and carries known advisories.

**Done when**: Build succeeds, suites have no new failures, runtime checks are reported accurately, and remaining blockers are documented.
