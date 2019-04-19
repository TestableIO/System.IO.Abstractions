# Contributor guide

## Versioning

This library uses [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) for generating stable and reproducible version numbers.

The base version is manually maintained in [the version config](version.json). Every build calculates its final version number based on the base version and the number of changes that occured since the last change to the version config.

The base version represents the MAJOR and MINOR parts of [SemVer](https://semver.org). If a PR contains breaking changes or new features the base version has to be changed accordingly. If a PR solely contains minor changes (bug fixes, code improvements) nothing needs to be done as the PATCH number will automatically increment with each commit.

## Branches / tags

* `master` contains the latest sources. Each merge there triggers a deploy to `nuget.org`.
* All versions on `nuget.org` have a matching GitHub release/tag.
