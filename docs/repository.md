# Repository guide

Paths in this document are relative to the repository root.
Read and follow the [development policy](policies/development.md) alongside this guide.

## About

`VolocyNazad.Toolkit.Localization` extends `Microsoft.Extensions.Localization` with string
localizers, localization type selection, attributes, and support for generic types and inheritance.
It has no Revit API dependency and is usable from any .NET application.

## Repository structure

```
.
├── src/
│   ├── Toolkit.Localization/          the library
│   └── Toolkit.Localization.Testing/  console sample, not a package
└── tests/
    └── Toolkit.Localization.Tests/    headless tests
```

- `Toolkit.Localization.sln` - solution with the `docs`, `solutionItems`, `.github`, `scripts` and
  `tests` folders.
- `Directory.Build.props` - analyzer mode, code-style enforcement and the public API documentation
  switch.
- `Directory.Packages.props` - central package versions for the library and the sample.
- `tests/Directory.Packages.props` - test-only package versions, importing the root file.
- `global.json` - .NET SDK selection and the Microsoft.Testing.Platform runner.
- `.github/workflows/` - CI, repository policy and publishing workflows.

## Technology stack

- `Microsoft.NET.Sdk`; the library targets `netstandard2.0` and `netstandard2.1`.
- C# 14, nullable reference types, implicit usings.
- `Microsoft.Extensions.Localization` 6.0.0, AutoConstructor and PolySharp; package versions use
  central package management (`Directory.Packages.props`), with test-only versions in
  `tests/Directory.Packages.props`.
- MinVer 8 derives the package version from stable `vMAJOR.MINOR.PATCH` Git tags;
  `MinVerMinimumMajorMinor` is `6.0`. Publish only tagged commits.
- The console sample targets `net9.0` and uses `Microsoft.Extensions.Hosting` and `System.Memory`.
- Tests use xunit.v3 through Microsoft.Testing.Platform on `net10.0`.
- No Revit API dependency. `src/Toolkit.Localization.Testing` is a console sample, not an automated
  test suite; the test framework is referenced only by `tests/Toolkit.Localization.Tests`.

## Documentation layout

- `AGENTS.md` links to the required repository guidance and routes a change to the document that
  owns it.
- `docs/policies/` contains the repository policies - the rules a change must follow. The
  [policy index](policies/README.md) says what each policy covers.
- `docs/repository.md` describes the project, repository structure, and technology stack.
- `changelog.d/` holds the pending changelog fragments: a consumer-visible change adds one, and the
  release preparation composes them into `CHANGELOG.md`; the rules are in the
  [changelog policy](policies/changelog.md). Git does not track empty directories, so the folder
  exists only while a fragment is pending.
- `scripts/` holds the repository validator, the changelog tool and the release verification.

The root solution exposes the documentation files under a `docs` solution folder in Visual Studio, preserving their subfolder structure. When adding documentation files, also add them as solution items; solution folders do not automatically include new files.

The root `global.json` selects stable .NET SDK 10.0 (minimum `10.0.103`, `rollForward: latestFeature`). CI and publishing install the SDK from this file. Additional SDK installations may provide older test runtimes. See the [SDK selection policy](policies/build.md#sdk-selection).

## Solution items

The root solution exposes repository-level documents and configuration under `solutionItems`, GitHub files and maintenance scripts in matching subfolders, and documentation under `docs/`. The list is explicit, not a filesystem glob; keep links up to date when files change. See the [solution items policy](policies/repository-maintenance.md#solution-items).

## Repository validation

`scripts/Validate-Repository.ps1` enforces the required repository documents,
their navigation links, the complete and valid Solution Items, and the workflow action pins. The
`.github/workflows/repository-policy.yml` workflow runs it, the pending fragment check
(`Update-Changelog.ps1 -Verify`) and the release structure check (`Verify-Release.ps1`) for pushes
and pull requests. See the [repository maintenance policy](policies/repository-maintenance.md#repository-validation).

## Formatting

The root `.editorconfig` defines the portable formatting baseline. Existing repositories may add stricter C# or analyzer-specific settings. See the [repository maintenance policy](policies/repository-maintenance.md#formatting-baseline).

## Continuous integration

`build-and-test` builds the solution in `Release` with warnings as errors and documentation warnings
promoted to errors, and runs the headless test executable on a Windows runner. `repository-policy`
runs the validator, the pending fragment check and the release structure check. `publish-nuget` is
manual and refuses a commit without a single stable tag and a matching changelog section.

## Testing

The automated test suite is in `tests/Toolkit.Localization.Tests` and covers the localizer selection,
the attributes and the generic and inherited localizer resolution. `src/Toolkit.Localization.Testing`
remains a console sample and is not used as a test project.

## Versioning and release tags

The library uses MinVer 8 with stable tags in the `vMAJOR.MINOR.PATCH` format. The tag without its
`v` prefix is the NuGet package version. The migration from the former four-part `6.0.0.9` version
starts with `v6.0.10` (or a higher stable version). The publish workflow accepts exactly one matching
tag at `HEAD` and verifies the package ID and version before pushing to NuGet.

The tagged commit must already contain the release section in `CHANGELOG.md` with an empty
`Unreleased` section above it and no pending fragment; the publish workflow stops otherwise. See the
[release policy](policies/releases.md) and the [changelog policy](policies/changelog.md#release).
