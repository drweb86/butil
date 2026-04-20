# Prerequisites

- Windows 11 x64 Professional;
- Microsoft Visual Studio 2026;

# Automated Release (GitHub Actions)

The `.github/workflows/publish-deb.yml` workflow runs when a GitHub Release is published. It automatically:

1. **Builds Ubuntu .deb packages** (x64 + arm64), uploads them to the Release, and updates the APT repository on `gh-pages`
2. **Builds Windows installer** (NSIS .exe with x64 + arm64) and **7z archive**, uploads them to the Release
3. **Creates a branch** in the `drweb86/winget-pkgs` fork with the WinGet manifest for the new version

## One-time GitHub setup

### Repository Secrets

Go to repository **Settings > Secrets and variables > Actions > Repository secrets** and create:

| Secret | Value |
|--------|-------|
| `DEB_GPG_PRIVATE_KEY` | Full armored GPG private key (output of `gpg --armor --export-secret-keys YOUR_KEY_ID`) |
| `DEB_GPG_PASSPHRASE` | Passphrase for the GPG key (empty string if none) |
| `WINGET_FORK_PAT` | GitHub Personal Access Token with `repo` scope, for pushing to `drweb86/winget-pkgs` fork |

#### Generating the GPG key

```
gpg --full-generate-key
```

Choose RSA 4096, no expiration (or set one), and enter a name/email.

Export private key:

```
gpg --armor --export-secret-keys YOUR_KEY_ID
```

#### Generating the WinGet fork PAT

Go to https://github.com/settings/tokens and create a **Fine-grained token** (or classic token) with `repo` scope for the `drweb86/winget-pkgs` repository.

### GitHub Pages

1. Go to repository **Settings > Pages**
2. Set source to **Deploy from a branch**
3. Set branch to `gh-pages`, folder to `/ (root)`

### Initial gh-pages branch

The workflow creates it automatically if it doesn't exist. To create it manually:

```
git checkout --orphan gh-pages
git rm -rf .
git commit --allow-empty -m "Initialize gh-pages"
git push origin gh-pages
git checkout master
```

## Release process

1. Update version in `CHANGELOG.md`
2. Commit and push to `master`
3. Create a GitHub Release (tag matching the version, e.g. `2026.04.14`, title matching the version)

Take changes from changelog.

Prepend Ubuntu references from previous release.

4. The workflow automatically builds and uploads all artifacts
5. For WinGet: create a PR from the auto-generated `app-VERSION` branch in `drweb86/winget-pkgs` to `microsoft/winget-pkgs`
