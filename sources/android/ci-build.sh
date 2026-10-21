#!/usr/bin/env bash
# Release APK and AAB for the Android app, plus the China PIPL-policy AAB.
# The China bundle requires privacy and license agreement before use.
# Required env: VERSION (date from CHANGELOG.md, for example 2026.10.08)
# Optional signing env:
#   ANDROID_KEYSTORE_FILE, ANDROID_KEYSTORE_PASSWORD, ANDROID_KEY_ALIAS, ANDROID_KEY_PASSWORD
set -euo pipefail

ROOT=$(CDPATH= cd -- "$(dirname -- "$0")/../.." && pwd)
ANDROID_ROOT="$ROOT/sources/android"
VERSION="${VERSION:?VERSION is required}"
VERSION="$(printf '%s' "$VERSION" | grep -oE '[0-9]{4}\.[0-9]{2}\.[0-9]{2}' | head -1)"
if [ -z "$VERSION" ]; then
  echo "VERSION must contain a date like 2026.10.08 (from CHANGELOG.md)."
  exit 1
fi
VERSION_CODE="${VERSION_CODE:-$(printf '%s' "$VERSION" | tr -d '.')}"
OUT_DIR="${OUT_DIR:-$ROOT/Output}"

if [ -z "${ANDROID_KEYSTORE_FILE:-}" ] || [ ! -f "$ANDROID_KEYSTORE_FILE" ]; then
  unset ANDROID_KEYSTORE_FILE ANDROID_KEYSTORE_PASSWORD ANDROID_KEY_ALIAS ANDROID_KEY_PASSWORD || true
  echo "No release keystore. Building unsigned."
else
  echo "Release keystore found. Building signed."
fi

chmod +x "$ANDROID_ROOT/gradlew"
sed -i 's/\r$//' "$ANDROID_ROOT/gradlew"

unset GRADLE_OPTS || true

(
  cd "$ANDROID_ROOT"
  sh ./gradlew --no-daemon \
    assembleGeneralRelease \
    bundleGeneralRelease \
    bundleChinaPiplPolicyRelease \
    -PappVersionName="$VERSION" \
    -PappVersionCode="$VERSION_CODE"
)

apk=""
for candidate in \
  "$ANDROID_ROOT/app/build/outputs/apk/general/release/app-general-release.apk" \
  "$ANDROID_ROOT/app/build/outputs/apk/general/release/app-general-release-unsigned.apk"
do
  if [ -f "$candidate" ]; then
    apk="$candidate"
    break
  fi
done
if [ -z "$apk" ]; then
  echo "Release APK not found."
  find "$ANDROID_ROOT/app/build/outputs" -name '*.apk' -type f || true
  exit 1
fi

aab=""
for candidate in \
  "$ANDROID_ROOT/app/build/outputs/bundle/generalRelease/app-general-release.aab" \
  "$ANDROID_ROOT/app/build/outputs/bundle/generalRelease/app-general-release-unsigned.aab"
do
  if [ -f "$candidate" ]; then
    aab="$candidate"
    break
  fi
done
if [ -z "$aab" ]; then
  echo "Release AAB not found."
  find "$ANDROID_ROOT/app/build/outputs" -name '*.aab' -type f || true
  exit 1
fi

china_aab=""
for candidate in \
  "$ANDROID_ROOT/app/build/outputs/bundle/chinaPiplPolicyRelease/app-chinaPiplPolicy-release.aab" \
  "$ANDROID_ROOT/app/build/outputs/bundle/chinaPiplPolicyRelease/app-chinaPiplPolicy-release-unsigned.aab"
do
  if [ -f "$candidate" ]; then
    china_aab="$candidate"
    break
  fi
done
if [ -z "$china_aab" ]; then
  echo "China PIPL policy AAB not found."
  find "$ANDROID_ROOT/app/build/outputs" -name '*.aab' -type f || true
  exit 1
fi

mkdir -p "$OUT_DIR"
dest_apk="$OUT_DIR/butil_${VERSION}_android.apk"
dest_aab="$OUT_DIR/butil_${VERSION}_android.aab"
dest_china_aab="$OUT_DIR/butil_${VERSION}_android_china.aab"
cp "$apk" "$dest_apk"
cp "$aab" "$dest_aab"
cp "$china_aab" "$dest_china_aab"
echo "APK: $dest_apk"
ls -lh "$dest_apk"
echo "AAB: $dest_aab"
ls -lh "$dest_aab"
echo "China PIPL AAB: $dest_china_aab"
ls -lh "$dest_china_aab"
