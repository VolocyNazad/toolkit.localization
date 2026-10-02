# Repository guide

Paths in this document are relative to the repository root.
Read and follow the [development policy](policies/development.md) alongside this guide.

## About

`VolocyNazad.Toolkit.Localization` extends `Microsoft.Extensions.Localization` with string localizers, localization type selection, attributes, and support for generic types and inheritance.

## Repository structure

- `Toolkit.Localization.sln` - solution.
- `src/Toolkit.Localization/` - library; abstractions and infrastructure containing services, attributes, extensions, and utilities.
- `src/Toolkit.Localization.Testing/` - console application for exercising localization with dependency injection and hosting.
- `.github/workflows/` - CI and publishing workflows.

## Technology stack

- Library: `Microsoft.NET.Sdk`, `netstandard2.0;netstandard2.1`, C# 14, nullable reference types, implicit usings.
- Microsoft.Extensions.Localization 6.0.0, AutoConstructor, and PolySharp.
- The console application targets `net9.0` and uses Microsoft.Extensions.Hosting and System.Memory.
- Dependency versions are specified inline in each project. The library package version is derived by MinVer from stable `vMAJOR.MINOR.PATCH` Git tags; publish only tagged commits.
- No Revit API dependency. The `Testing` project is a console application, not an automated test suite; no test framework is referenced in it.

## Documentation layout

- `AGENTS.md` links to the required repository guidance.
- `docs/policies/development.md` contains the development policy.
- `docs/repository.md` describes the project, repository structure, and technology stack.
- `CHANGELOG.md` records changes under `Unreleased` before release.

The root solution exposes the documentation files under a `docs` solution folder in Visual Studio, preserving their subfolder structure. When adding documentation files, also add them as solution items; solution folders do not automatically include new files.

The root `global.json` selects stable .NET SDK 10.0 (minimum `10.0.103`, `rollForward: latestFeature`). CI and publishing install the SDK from this file. Additional SDK installations may provide older test runtimes. See the [SDK selection policy](policies/development.md#net-sdk-selection).

## Solution items

The root solution exposes repository-level documents and configuration under `solutionItems`, GitHub files and maintenance scripts in matching subfolders, and documentation under `docs/`. The list is explicit, not a filesystem glob; keep links up to date when files change. See the [solution items policy](policies/development.md#solution-items).
## Repository validation

`scripts/Validate-Repository.ps1` enforces the required repository documents,
their navigation links, and complete, valid Solution Items. The
`.github/workflows/repository-policy.yml` workflow runs it for pushes and pull
requests. See the [development policy](policies/development.md#repository-validation).

## Formatting

The root `.editorconfig` defines the portable formatting baseline. Existing repositories may add stricter C# or analyzer-specific settings. See the [development policy](policies/development.md#formatting-baseline).

## Testing

The automated test suite is in tests/Toolkit.Localization.Tests. src/Toolkit.Localization.Testing remains a console sample and is not used as a test project.

## Versioning and release tags

The library uses MinVer 8 with stable tags in the `vMAJOR.MINOR.PATCH` format. The tag without its `v` prefix is the NuGet package version. The migration from the former four-part `6.0.0.9` version starts with `v6.0.10` (or a higher stable version). Manual publishing accepts exactly one matching tag at HEAD and verifies the package ID and version before pushing to NuGet.

The tagged commit must already contain the release section in `CHANGELOG.md` with an empty `Unreleased` section above it; the publish workflow stops otherwise. See the [release changelog policy](policies/development.md#release-changelog).
