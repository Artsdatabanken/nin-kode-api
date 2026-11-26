# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net11.0
- **Scope**: Entire solution
- **AutoMapper**: Keep the existing version unchanged; do not upgrade this package

## Upgrade Options

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Package Management: Central Package Management

### Compatibility
- Unsupported Packages: Defer Resolution (RavenDB.Database 3.5.9; three consuming projects)
- Unsupported API Handling: Fix Inline

## Strategy
**Selected**: All-At-Once

### Execution Constraints
- Upgrade all projects in one atomic operation; do not order work by dependency tiers.
- Verify the .NET 11 SDK and global.json compatibility before changing project targets.
- Update target frameworks and package references across the solution, restore, then fix compilation errors in one bounded pass.
- Run tests after the atomic upgrade and finish with a full solution build and test run.

## Source Control
- **Source Branch**: staging
- **Working Branch**: upgrade-dotnet-11
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)
