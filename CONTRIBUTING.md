# Contributor guide

## Versioning

This library uses [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) for generating stable and reproducible version numbers.

The base version is manually maintained in [the version config](version.json). Every build calculates its final version number based on the base version and the number of changes that occured since the last change to the version config.

The base version represents the MAJOR and MINOR parts of [SemVer](https://semver.org). If a PR contains breaking changes or new features the base version has to be changed accordingly. If a PR solely contains minor changes (bug fixes, code improvements) nothing needs to be done as the PATCH number will automatically increment with each commit.

## Branches / tags

- `main` contains the latest sources. Each merge there triggers a deploy to `nuget.org`.
- All versions on `nuget.org` have a matching GitHub release/tag.

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
