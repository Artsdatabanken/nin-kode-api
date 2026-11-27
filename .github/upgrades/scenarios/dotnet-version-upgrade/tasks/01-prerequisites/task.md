# 01-prerequisites: Verify .NET 11 build prerequisites

Confirm the installed .NET SDK and build tooling can target `net11.0`, and check repository-wide SDK configuration such as `global.json`. The complete solution depends on the selected SDK being available before project targets or package files are changed.

## Research Findings

- `dotnet --version` in the repository resolves to SDK `11.0.100`; SDK `9.0.321` is also installed.
- Root `global.json` requests SDK `6.0.100` with `rollForward: latestMajor`, which allows the installed .NET 11 SDK to be selected.
- The .NET SDK validation confirms a compatible SDK and Visual Studio MSBuild installation are available.

**Done when**: The required .NET 11 SDK/toolchain is available and repository SDK configuration is compatible, or a specific blocker is documented before proceeding.
