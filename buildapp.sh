#!/bin/bash
# macos_build.sh — bundle, sign, create DMG, and notarize a .NET app

APP_NAME="Controller_v2"
PUBLISH_DIR="./bin/Release/net8.0/publish"
ICON_SRC="./media/frozen-game-controller.ico"
UI_SRC="./Controller/UI"
INFO_PLIST="./Info.plist"
APP_DIR="./${APP_NAME}.app"
CONTENTS_DIR="${APP_DIR}/Contents"
MACOS_DIR="${CONTENTS_DIR}/MacOS"
RESOURCES_DIR="${CONTENTS_DIR}/Resources"
DMG_PATH="./${APP_NAME}.dmg"

# Notarization info
APPLE_ID="web@newagesoftware.net"
BUNDLE_ID="com.nas.controller_v2"   # Must match Info.plist CFBundleIdentifier
TEAM_ID="9ELVK5QPKF"

echo "🧩 Building ${APP_NAME}.app..."

# Clean old bundle
rm -rf "$APP_DIR" "$DMG_PATH"

# Create bundle structure
mkdir -p "$MACOS_DIR" "$RESOURCES_DIR"

# Copy everything except runtimes, pdbs, and UI folder to MacOS
rsync -av --exclude='runtimes' --exclude='*.pdb' --exclude='UI' "$PUBLISH_DIR/" "$MACOS_DIR/"

# Copy only macOS runtimes into MacOS/runtimes
rm -rf "$MACOS_DIR/runtimes"
mkdir -p "$MACOS_DIR/runtimes"
for plat in osx osx-x64 osx-arm64; do
    if [ -d "$PUBLISH_DIR/runtimes/$plat" ]; then
        cp -R "$PUBLISH_DIR/runtimes/$plat" "$MACOS_DIR/runtimes/"
    fi
done

# Copy Info.plist
cp "$INFO_PLIST" "$CONTENTS_DIR/"

# Copy UI folder into Resources, excluding Overlay
mkdir -p "$RESOURCES_DIR/UI"
rsync -av --exclude='Overlay' "$UI_SRC/" "$RESOURCES_DIR/UI/"

# Convert .ico to .icns if possible
ICON_ICNS="${RESOURCES_DIR}/${APP_NAME}.icns"
if command -v iconutil &>/dev/null; then
    TMP_ICONSET="/tmp/${APP_NAME}.iconset"
    mkdir -p "$TMP_ICONSET"
    sips -s format png "$ICON_SRC" --out "$TMP_ICONSET/icon_512x512.png" >/dev/null
    iconutil -c icns "$TMP_ICONSET" -o "$ICON_ICNS"
    rm -rf "$TMP_ICONSET"
else
    echo "⚠️ iconutil not found — skipping icon conversion"
fi

# Code sign the .app
codesign --deep --force --verify --verbose \
    --sign "Developer ID Application: New Age Software LLC (${TEAM_ID})" \
    "$APP_DIR"

echo "✅ Signed ${APP_NAME}.app"

# Create DMG
echo "💿 Creating DMG..."
rm -f "$DMG_PATH"
hdiutil create -volname "$APP_NAME" -srcfolder "$APP_DIR" -ov -format UDZO "$DMG_PATH"

# Code sign the DMG
codesign --force --verify --verbose \
    --sign "Developer ID Application: New Age Software LLC (${TEAM_ID})" \
    "$DMG_PATH"
echo "✅ Signed DMG"

# Notarize DMG
read -sp "Enter your Apple app-specific password: " APP_PASSWORD
echo
echo "📥 Submitting DMG for notarization..."
xcrun notarytool submit "$DMG_PATH" \
    --apple-id "$APPLE_ID" \
    --team-id "$TEAM_ID" \
    --password "$APP_PASSWORD" \
    --wait

# Staple notarization ticket
echo "📎 Stapling notarization ticket..."
xcrun stapler staple "$DMG_PATH"

echo "🎉 Done — ${APP_NAME}.app and ${APP_NAME}.dmg created, signed, and notarized in $(pwd)"