# Contributor guide

## Versioning

This library uses [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) for generating stable and reproducible version numbers.

The base version is manually maintained in [the version config](version.json). Every build calculates its final version number based on the base version and the number of changes that occured since the last change to the version config.

The base version represents the MAJOR and MINOR parts of [SemVer](https://semver.org). If a PR contains breaking changes or new features the base version has to be changed accordingly. If a PR solely contains minor changes (bug fixes, code improvements) nothing needs to be done as the PATCH number will automatically increment with each commit.

## Branches / tags

* `master` contains the latest sources - this is where we develop.
* `release` contains the sources for the latest version on `nuget.org` - this is where we deploy from.
* All versions on `nuget.org` have a matching GitHub release/tag

### Release workflow

1. Create a [PR from `master` to `release`](https://github.com/System-IO-Abstractions/System.IO.Abstractions/compare/release...master?expand=1) and wait for CI to finish. 
1. Inspect CI run (test results, version number)
1. Merge PR and wait for deployment
1. Inspect newly created package versions on NuGet.org and newly created GitHub release
