# Contributor guide

## Versioning

This library uses [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) for generating stable and reproducible version numbers.

The base version is manually maintained in [the version config](version.json). Every build calculates its final version number based on the base version and the number of changes that occured since the last change to the version config.

The base version represents the MAJOR and MINOR parts of [SemVer](https://semver.org). If a PR contains breaking changes or new features the base version is changed accordingly. If a PR solely contains minor changes (bug fixes, code improvements) nothing needs to be done.

## Branches / tags

* `master` contains the latest sources - this is where all PRs will go to.
* `release` contains the most recently released sources - this is where we deploy from
  * Source for older releases are tagged.

Every successful commit on `master` deploys packages to `nuget.org` and a creates GitHub release. As long as we have the prelease suffix both will be marked as such.

### Release workflow

1. Create a PR from `master` (or a specific commit on `master`) to `release`
1. Wait for CI to finish. 
1. Inspect CI run (test results, version number)
1. Merge PR 
1. Wait for deployment 
1. Inspect newly created package versions on NuGet.org and newly created GitHub release
