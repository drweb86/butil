# Prerequisites

- Windows 11x64 Professional;
- Microsoft Visual Studio 2026;
- AMD architecture

# Packaging

## Windows

### Prerequisites

- NSIS 3.11 (must be installed in default directory);

### Build.cmd

Update version (it should match the version name and tag of release!) in Version History file. 

Update version in the ubuntu-install.sh script

Launch build.cmd.

It will create <root>\output folder with all compiled.

## Ubuntu (.deb packages)

### Automated (GitHub Actions)

The `.github/workflows/publish-deb.yml` workflow automatically builds `.deb` packages and publishes them when a GitHub Release is created. It also maintains an APT repository on the `gh-pages` branch.

#### One-time GitHub setup

1. **Generate a GPG key** (on any machine with GPG installed):

```
gpg --full-generate-key
```

Choose RSA 4096, no expiration (or set one), and enter a name/email.

2. **Export the private key**:

```
gpg --armor --export-secret-keys YOUR_KEY_ID
```

3. **Add the private key as a GitHub secret** named `DEB_GPG_PRIVATE_KEY`:
   - Go to repository Settings > Secrets and variables > Actions
   - Create a new secret with the full armored private key output

4. **Enable GitHub Pages**:
   - Go to repository Settings > Pages
   - Set source to "Deploy from a branch"
   - Set branch to `gh-pages`, folder to `/ (root)`

5. **Create the initial `gh-pages` branch** (the workflow can also create it automatically):

```
git checkout --orphan gh-pages
git rm -rf .
git commit --allow-empty -m "Initialize gh-pages"
git push origin gh-pages
git checkout master
```

After this setup, every published GitHub Release will automatically build and publish `.deb` packages.

### Manual (on Ubuntu)

Prerequisites: .NET 10 SDK, `dpkg-deb` (standard on Ubuntu).

```
cd sources
chmod +x build-deb.sh
./build-deb.sh
```

Output `.deb` files appear in the `Output/` directory at the repository root.
