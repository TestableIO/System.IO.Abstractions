# Contributor guide

## Versioning

This library follows [Semantic Versioning](https://semver.org). The version is calculated by [GitVersion](https://gitversion.net) in the build pipeline from the Git history and tags, so there is no version file to update and a PR doesn't need to change any version number.

Releases are made by pushing a tag of the form `vMAJOR.MINOR.PATCH` (for example `v23.0.1`), optionally with a pre-release suffix (for example `v23.1.0-beta.1`). The build for that tag packs the packages with that version and publishes them to `nuget.org`.

Please still mark breaking changes and new features in the PR title (see below), so the next release can be versioned accordingly.

## Branches / tags

- `main` contains the latest sources.
- Each `v*` release tag triggers a deploy to `nuget.org`, so all versions on `nuget.org` have a matching GitHub tag. Tags without a pre-release suffix also get a GitHub release.

## Commits and PR title

- Please use [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) to name your commits and PR title.
  We use [action-semantic-pull-request](https://github.com/amannn/action-semantic-pull-request?tab=readme-ov-file#configuration) to enforce this policy, feel free to have a closer look.
- Available prefixes:
  - `feat:` A new feature
  - `fix:` A bug fix
  - `docs:` Documentation only changes
  - `style:` Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
  - `refactor:` A code change that neither fixes a bug nor adds a feature
  - `perf:` A code change that improves performance
  - `test:` Adding missing tests or correcting existing tests
  - `build:` Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
  - `ci:` Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
  - `chore:` Other changes that don't modify src or test files
  - `revert:` Reverts a previous commit
