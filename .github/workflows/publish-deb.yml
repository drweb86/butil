#!/bin/bash

# Builds BUtil .deb packages for amd64 and arm64.
# Run from the sources/ directory on Ubuntu with .NET 10 SDK installed.

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
OUTPUT_DIR="$REPO_ROOT/Output"

cd "$SCRIPT_DIR"

version=$(head -1 "$REPO_ROOT/help/Version History (Changelog).md" | sed 's/^\xEF\xBB\xBF//' | sed 's/^# //')
echo "Building BUtil v$version deb packages"
echo ""

rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

declare -A ARCH_MAP
ARCH_MAP["linux-x64"]="amd64"
ARCH_MAP["linux-arm64"]="arm64"

for rid in "${!ARCH_MAP[@]}"; do
    deb_arch="${ARCH_MAP[$rid]}"
    publish_dir="$OUTPUT_DIR/staging/$deb_arch/publish"
    pkg_root="$OUTPUT_DIR/staging/$deb_arch/pkg"
    deb_file="$OUTPUT_DIR/butil_${version}_${deb_arch}.deb"

    echo "========================================="
    echo "  Building $rid ($deb_arch)"
    echo "========================================="
    echo ""

    rm -rf "$OUTPUT_DIR/staging/$deb_arch"

    echo "Publishing..."
    dotnet publish \
        "/p:InformationalVersion=$version" \
        "/p:VersionPrefix=$version" \
        "/p:Version=$version" \
        "/p:AssemblyVersion=$version" \
        "--runtime=$rid" \
        -c Release \
        "/p:PublishDir=$publish_dir" \
        /p:PublishReadyToRun=false \
        /p:RunAnalyzersDuringBuild=False \
        --self-contained true \
        --property WarningLevel=0

    echo "Creating package structure..."
    mkdir -p "$pkg_root/DEBIAN"
    mkdir -p "$pkg_root/usr/lib/butil"
    mkdir -p "$pkg_root/usr/bin"
    mkdir -p "$pkg_root/usr/share/applications"
    mkdir -p "$pkg_root/usr/share/pixmaps"

    cp -a "$publish_dir/"* "$pkg_root/usr/lib/butil/"

    ln -sf ../lib/butil/butil-ui.Desktop "$pkg_root/usr/bin/butil-ui"
    ln -sf ../lib/butil/butilc "$pkg_root/usr/bin/butilc"

    cp "$REPO_ROOT/help/Assets/Icon 120x120.png" "$pkg_root/usr/share/pixmaps/butil.png"

    cat > "$pkg_root/usr/share/applications/butil.desktop" << 'DESKTOP'
[Desktop Entry]
Version=1.0
Name=BUtil
GenericName=Backup and Synchronization
Comment=Incremental backup, synchronization, and media import with deduplication
Categories=Utility;Archiving;FileTools;
Type=Application
Terminal=false
Exec=butil-ui
Icon=butil
StartupWMClass=butil-ui.Desktop
DESKTOP

    installed_size=$(du -sk "$pkg_root" | cut -f1)

    cat > "$pkg_root/DEBIAN/control" << CONTROL
Package: butil
Version: $version
Section: utils
Priority: optional
Architecture: $deb_arch
Installed-Size: $installed_size
Depends: libc6, libgcc-s1, libstdc++6, libx11-6, libfontconfig1, dbus-x11, cron
Maintainer: Siarhei Kuchuk <https://github.com/drweb86>
Homepage: https://github.com/drweb86/butil
Description: Incremental backup, synchronization, and media import tool
 BUtil creates incremental backups, synchronization, and imports
 multimedia with deduplication and FTPS, SFTP, SMB/CIFS support.
 .
 Features: AES-256 encryption, scheduling via console tool, graphical UI.
CONTROL

    cat > "$pkg_root/DEBIAN/postinst" << 'POSTINST'
#!/bin/bash
set -e
chmod +x /usr/lib/butil/butil-ui.Desktop
chmod +x /usr/lib/butil/butilc
if command -v update-desktop-database > /dev/null 2>&1; then
    update-desktop-database -q /usr/share/applications || true
fi
POSTINST
    chmod 755 "$pkg_root/DEBIAN/postinst"

    cat > "$pkg_root/DEBIAN/postrm" << 'POSTRM'
#!/bin/bash
set -e
if command -v update-desktop-database > /dev/null 2>&1; then
    update-desktop-database -q /usr/share/applications || true
fi
POSTRM
    chmod 755 "$pkg_root/DEBIAN/postrm"

    echo "Setting permissions..."
    find "$pkg_root/usr" -type d -exec chmod 755 {} \;
    find "$pkg_root/usr/lib/butil" -type f -exec chmod 644 {} \;
    chmod 755 "$pkg_root/usr/lib/butil/butil-ui.Desktop"
    chmod 755 "$pkg_root/usr/lib/butil/butilc"
    find "$pkg_root/usr/lib/butil" \( -name "*.so" -o -name "*.so.*" \) -exec chmod 755 {} \;
    chmod 644 "$pkg_root/usr/share/applications/butil.desktop"
    chmod 644 "$pkg_root/usr/share/pixmaps/butil.png"

    echo "Building .deb..."
    dpkg-deb --build --root-owner-group "$pkg_root" "$deb_file"

    echo "Created: $deb_file"
    echo ""
done

rm -rf "$OUTPUT_DIR/staging"

echo "========================================="
echo "  Build complete"
echo "========================================="
ls -lh "$OUTPUT_DIR"/*.deb
