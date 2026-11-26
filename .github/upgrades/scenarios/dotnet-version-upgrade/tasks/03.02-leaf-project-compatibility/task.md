# 03.02-leaf-project-compatibility: Resolve and validate leaf project compatibility

## Objective
Resolve the .NET 11 API findings in leaf projects before their dependents.

## Scope
- Projects: NiN3.Core, NiN.Database, NiN.CmsImpExp.
- Key findings: NiN.CmsImpExp ApiClient.cs has WebClient at line 584 and JsonDocument.Parse at lines 42, 74, 128, 210, and 376. The other leaf projects have no assessed source/binary API findings.

## Research Findings
- Queried project summaries/issues for all three projects. NiN.Database and NiN3.Core have no assessed source/binary API changes; their framework package updates were already applied in 03.01. AutoMapper remains 14.0.0 by user instruction.
- In ApiClient.cs, WebClient is instantiated only in `Download`; `DownloadImages` invokes it in a `Parallel.ForEach`. The class already owns a reusable, thread-safe `HttpClient`, and image URLs are absolute HTTPS strings. Replace the WebClient download with `_httpClient.GetStreamAsync(image.Url)` copied to `File.Create(path)` to keep the synchronous method contract and stream to disk rather than buffering entire image files.
- The five JsonDocument.Parse calls receive response strings returned by the existing HttpClient and use the same array/object access paths. Assessment classifies them as behavioral findings but gives no changed parse contract; the .NET 11 overload remains available, so retain the current calls and document this review rather than introducing an unsubstantiated parsing change.
- No test directly exercises `ApiClient.Download`; the existing `NiN.Test_CmsImpExp` suite is the applicable regression suite (17 tests passed after the framework transition).

## Steps
1. Check whether the APIs compile and determine which assessed behavior changes are caller-visible.
2. Apply the smallest source-compatible fixes required; preserve existing behavior.
3. Build the leaf projects and run applicable tests.

## Done when
The leaf projects compile on net11.0 without new API warnings/errors, and applicable tests pass or any baseline failures are documented.
