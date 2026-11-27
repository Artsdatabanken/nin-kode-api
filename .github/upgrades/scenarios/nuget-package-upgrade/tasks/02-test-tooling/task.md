# 02-test-tooling: Upgrade test tooling

Upgrade Microsoft.NET.Test.Sdk 17.14.1 to 18.12.0, xunit.runner.visualstudio 3.1.0 to 4.0.1 and coverlet.collector 6.0.4 to 10.1.1 in `Directory.Packages.props`; review corresponding `apidiff` files. Assess xUnit v3 separately and migrate only if compatible with the project's tests and runner. Preserve test discovery and compare results to the existing baseline.

## Research
- `NiN3.Tests` and `NiN.Test_CmsImpExp` use CPM and all three tooling packages with xUnit 2.9.3. Runner references also occur in application projects; `NiN.CmsImpExp` references the collector as well. A central update therefore affects more than test projects.
- Visual Studio currently discovers 97 tests; the recorded baseline is CMS 17 passed and NiN3 62 passed, 18 failed. Test Explorer's cached outcome states are not a fresh run.
- The runner API diff reports no public source-breaking changes, but does not establish runner compatibility. Verify both discovery and execution, plus a real collector output.
- The xUnit v3 migration guide describes an architectural change to executable test projects and additional breaking changes. Assess that separately after validating the tooling upgrades; do not infer compatibility from framework targeting alone.

**Done when**: Test suites discover and run without new failures and coverage collection is checked, or a concrete blocker is documented.
