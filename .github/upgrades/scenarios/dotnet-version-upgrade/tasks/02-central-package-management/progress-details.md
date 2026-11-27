# Progress: Central package management

- Added root `Directory.Packages.props` centralizing all 45 baseline package IDs for the 13 SDK-style projects.
- Removed active inline `Version` attributes from all 122 project package references, retaining package versions and `PrivateAssets`/`IncludeAssets` metadata. AutoMapper remains 14.0.0 in all 13 projects.
- Captured baseline and post-conversion package JSON and MSBuild binlogs in `cpm-artifacts`. Compared package resolution for every project/package pair: no additions, removals, or version changes.
- Clean solution build after conversion succeeded with 0 errors and 596 warnings, equal to the baseline warning count. The AutoMapper NU1903 advisory remains per user instruction.
- Test results: `NiN.Test_CmsImpExp` passed 17/17. `NiN3.Tests` passed 62/80; 18 failures are existing fixture/data expectation mismatches (including expected 3571 vs actual 3585 and expected 429 vs actual 430). The initial run had additional failures because the test expects an untracked `NiN3.Console\temp` directory; creating that expected directory removed those path failures. No package resolution differences were found to explain the remaining assertion failures.
- Created `cpm-artifacts\convert-to-cpm.md` with the conversion overview, comparison, and follow-up advisories.
