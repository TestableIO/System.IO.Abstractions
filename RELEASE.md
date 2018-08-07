# Procedure for Release

- [ ] Update versions in appveyor.yml: `version: 2.2.0.{build}`
- [ ] Commit release version: `git add appveyor.yml` `git commit -m "chore(release): update version number for release"`
- [ ] Prepare to deploy by creating and pushing tags: _...initiates deploy an actual release, due to appveyor.yml 'on: APPVEYOR_REPO_TAG: true' condition._
- [ ] Create a tag of version: `git tag v0.0.0` `git push --tags` 
- [ ] Optional, watch build: https://ci.appveyor.com/System-IO-Abstractions/System.IO.Abstractions
- [ ] Update version to vNext in appveyor.yml.
- [ ] Update appveyor version to vNext for subsequent builds: 
- [ ] Commit vNext version: `git add appveyor.yml` `git commit -m "chore(release): update version number for vNext"`