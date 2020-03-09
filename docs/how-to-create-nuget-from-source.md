To create NuGet packages from the source, execute these 3 scripts (at the repository root):

- (this can be skipped if you don't need to update the version) `version-update.ps1 [old version] [new version]` (e.g. `version-update.ps1 2.4.0 2.4.1`
- build-publish.ps1
- nuget-update.ps1

This creates NuGet packages in the `nuget` and `nuget-dotnetcli` directories (at the repository root).