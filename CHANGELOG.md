# Changelog

All notable changes to this project are documented in this file.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

### Added

- Add a testing policy: test kinds, configuration coverage, naming, isolation and the manual smoke check.
- Add a benchmark policy: when performance is measured, how, and how results are reported.

- Branching policy: `main` holds the released state, `dev` integrates the next release, and changes arrive through feature, fix, release and hotfix branches.

### Changed

- Move the changelog policy to `docs/policies/changelog.md` and require it in the repository validator.

- Move the commit message policy to `docs/policies/commits.md` and require it in the repository validator.

- Document the commit message policy: the Conventional Commits type set, breaking-change markers and subject rules.

- Moved package versions to central package management (`Directory.Packages.props` and `tests/Directory.Packages.props`).

## [6.0.11] - 2026-09-14

### Fixed

- Pack only `Toolkit.Localization` in CI and exclude the console sample from NuGet packaging.

## [6.0.10] - 2026-09-14

### Fixed

- Include implemented-interface resources when a class uses `UseInterfacesStringLocalizationsAttribute`.
- Preserve resource keys and pass formatting arguments through the logging localizer.
- Prevent recursive localization type relationships from overflowing the stack or adding duplicate sources.
- Return a missing-resource result from an empty multi-localizer instead of a nullable value.

### Added

- Add automated coverage for localization type selection, localizer fallback behavior, formatted lookup and dependency-injection configuration.
- Validate required repository files, navigation links, and Solution Items in CI.
- Development policy and repository guide under `docs/`, with required reading links in `AGENTS.md` and navigation from `README.md`.

### Changed

- Derive package versions from stable `vMAJOR.MINOR.PATCH` tags with MinVer and validate the package before publishing.
- Translate public API and sample documentation comments into English.
- Update PolySharp to 1.16.0.
- Standardize GitHub Actions workflow filenames and display names by responsibility.
- Configure xUnit v3 test execution through Microsoft.Testing.Platform and fail test runs when no tests are discovered.
- Establish a shared EditorConfig baseline and use the repository-policy validator as the single structural CI check.
- Complete solution items for repository documents, configuration, workflows and maintenance scripts; document the shared layout.
- Standardize local and CI SDK selection on stable .NET 10.0 through global.json, restrict roll-forward to that major/minor line, and configure setup-dotnet to read the file.
- Show documentation in Visual Studio Solution Explorer under a `docs` solution folder with matching subfolders.
