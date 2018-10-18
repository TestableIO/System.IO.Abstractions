# Contributor guide

## Versioning

This library uses [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) for generating stable and reproducible version numbers.

The so-called base version is manually maintained in [the version config](version.json). Every build calculates its final version number based on the base version and the number of changes that occured since the last change to the version config.

The base version represents the _next_ version that we will released. During development it contains a prerelease suffix, like `-beta` which is appended to the generated NuGet packages.

Every successful commit on `master` deploys packages to `nuget.org` and a creates GitHub release. As long as we have the prelease suffix both will be marked as such.

### Release workflow

1. Remove prelease suffix from `version.json`.
1. Wait for the completion of the deployment.
1. Remove the prerelease flag from the newly created GitHub release.
1. Increment the version number in `version.json` and again add the prelease suffix (usually `beta` is fine).