# 03.04-host-test-compatibility: Resolve and validate application and test compatibility

## Objective
Resolve .NET 11 API findings in the remaining application, host, and test projects after library projects.

## Scope
- Projects: NiN.Console, NiN3.Console, NinKode.Database, NiN3.Tests, NinKode.WebApi.
- Key findings: configuration environment provider at NiN.Console Program.cs line 24 and NinKode.Database Program.cs line 63; NiN3.Console Program.cs has three ConfigurationBinder.GetValue<T> sites (lines 21, 143, 153), two Path.Combine sites (lines 144 and 154), and AddConsole at line 25; NiN3.Tests VariabelApiServiceTest.cs line 95 has ConfigurationBinder.GetValue<T>; NinKode.WebApi has 33 behavioral findings on ProducesResponseType and Produces metadata.

## Research Findings
- Confirmed the five scoped projects target net11.0. NiN3.Console's ConfigurationBinder.GetValue<T> resolves from Microsoft.Extensions.Configuration.Binder 11.0.0; Path.Combine has its existing string-array overload plus the new span overload; and the console logging registration compiles. The single-argument Path.Combine calls are path identity operations, so changing them would not address a demonstrated compatibility issue.
- Both parameterless AddEnvironmentVariables calls remain supported and retain their default unprefixed environment-variable behavior.
- Inventoried all 34 Web API metadata attributes (33 ProducesResponseType annotations plus the base controller's Produces attribute) across eight controller files. ASP.NET Core MVC still exposes the attribute constructors and StatusCode/Type/Description members. Startup continues to register controllers, API versioning, ApiExplorer, and Swashbuckle; the annotations continue to define response/content-type metadata used by those existing docs. No behavior-changing incompatibility was found, so no mechanical attribute rewrite is warranted.
- Test Explorer discovers 80 NiN3.Tests tests. The known baseline is 62 passing and 18 existing fixture/data assertion failures; no Web API metadata-specific test was found.

## Steps
1. Fix any verified compile incompatibilities with the narrowest changes.
2. Verify Web API response metadata and behavior without mechanically rewriting attributes.
3. Build the scoped projects and run applicable tests, then run a full solution build.

## Done when
The host and test projects compile on net11.0, behavior-changing API findings are reviewed, and the solution build has no errors. Document any existing warning/test baseline that cannot be resolved within this task.
