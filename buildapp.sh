#!/bin/bash

# Configuration
APP_NAME="ControllerDesktop"
BUNDLE_ID="com.nas.controller_v2"
PROJECT_PATH="Controller_v2.Desktop/Controller_v2.Desktop.csproj"
PUBLISH_DIR="bin/Release/net8.0/osx-arm64/publish"
APP_BUNDLE="${APP_NAME}.app"
DMG_NAME="${APP_NAME}.dmg"

# Code signing configuration
DEVELOPER_ID="Developer ID Application: New Age Software LLC (9ELVK5QPKF)"
INSTALLER_ID="Developer ID Installer: New Age Software LLC (9ELVK5QPKF)"
APPLE_ID="web@newagesoftware.net"
TEAM_ID="9ELVK5QPKF"
KEYCHAIN_PROFILE="notarytool-profile"  # Created with: notarytool store-credentials

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

echo_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

echo_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

# Clean previous builds
echo_info "Cleaning previous builds..."
rm -rf "${PUBLISH_DIR}"
rm -rf "${APP_BUNDLE}"
rm -f "${DMG_NAME}"
dotnet clean "${PROJECT_PATH}" -c Release

# Publish the application
echo_info "Publishing application for osx-arm64..."
dotnet publish "${PROJECT_PATH}" \
    -c Release \
    -r osx-arm64 \
    --self-contained true \
    -p:PublishSingleFile=false \
    -p:PublishTrimmed=false

if [ $? -ne 0 ]; then
    echo_error "Publish failed"
    exit 1
fi

# Create .app bundle structure
echo_info "Creating .app bundle structure..."
mkdir -p "${APP_BUNDLE}/Contents/MacOS"
mkdir -p "${APP_BUNDLE}/Contents/Resources"

# Copy published files to MacOS folder
echo_info "Copying application files..."
cp -R "${PUBLISH_DIR}"/* "${APP_BUNDLE}/Contents/MacOS/"

# Move UI folder to Resources if it exists
if [ -d "${APP_BUNDLE}/Contents/MacOS/UI" ]; then
    echo_info "Moving UI folder to Resources..."
    mv "${APP_BUNDLE}/Contents/MacOS/UI" "${APP_BUNDLE}/Contents/Resources/"
fi

# Make the main executable
chmod +x "${APP_BUNDLE}/Contents/MacOS/${APP_NAME}"

# Copy existing Info.plist
echo_info "Copying Info.plist..."
INFO_PLIST_PATH="Info.plist"  # Adjust this path if your Info.plist is in a different location

if [ ! -f "${INFO_PLIST_PATH}" ]; then
    echo_error "Info.plist not found at ${INFO_PLIST_PATH}"
    exit 1
fi

cp "${INFO_PLIST_PATH}" "${APP_BUNDLE}/Contents/Info.plist"

# Convert .ico to .icns for app icon
ICO_PATH="media/frozen-game-controller.ico"
ICNS_PATH="${APP_BUNDLE}/Contents/Resources/AppIcon.icns"

if [ -f "${ICO_PATH}" ]; then
    echo_info "Converting icon from .ico to .icns..."
    
    # Create temporary directory for icon conversion
    ICON_TEMP_DIR="icon_temp"
    mkdir -p "${ICON_TEMP_DIR}"
    
    # Extract PNG from ICO (macOS's sips can read .ico files)
    sips -s format png "${ICO_PATH}" --out "${ICON_TEMP_DIR}/icon.png" > /dev/null 2>&1
    
    # Create iconset directory structure
    mkdir -p "${ICON_TEMP_DIR}/AppIcon.iconset"
    
    # Generate different sizes for .icns
    sips -z 16 16     "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_16x16.png" > /dev/null 2>&1
    sips -z 32 32     "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_16x16@2x.png" > /dev/null 2>&1
    sips -z 32 32     "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_32x32.png" > /dev/null 2>&1
    sips -z 64 64     "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_32x32@2x.png" > /dev/null 2>&1
    sips -z 128 128   "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_128x128.png" > /dev/null 2>&1
    sips -z 256 256   "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_128x128@2x.png" > /dev/null 2>&1
    sips -z 256 256   "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_256x256.png" > /dev/null 2>&1
    sips -z 512 512   "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_256x256@2x.png" > /dev/null 2>&1
    sips -z 512 512   "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_512x512.png" > /dev/null 2>&1
    sips -z 1024 1024 "${ICON_TEMP_DIR}/icon.png" --out "${ICON_TEMP_DIR}/AppIcon.iconset/icon_512x512@2x.png" > /dev/null 2>&1
    
    # Convert iconset to icns
    iconutil -c icns "${ICON_TEMP_DIR}/AppIcon.iconset" -o "${ICNS_PATH}"
    
    # Clean up temp files
    rm -rf "${ICON_TEMP_DIR}"
    
    echo_info "Icon converted successfully"
else
    echo_warning "Icon file not found at ${ICO_PATH}, skipping icon conversion"
fi

# Code signing
echo_info "Code signing application..."

# Sign all dylibs and frameworks first (if any)
find "${APP_BUNDLE}" -name "*.dylib" -exec codesign --force --sign "${DEVELOPER_ID}" --timestamp --options runtime {} \;

# Sign all executables and binaries
find "${APP_BUNDLE}/Contents/MacOS" -type f -perm +111 -exec codesign --force --sign "${DEVELOPER_ID}" --timestamp --options runtime {} \;

# Sign the entire app bundle
codesign --force --sign "${DEVELOPER_ID}" \
    --timestamp \
    --options runtime \
    --entitlements entitlements.plist \
    --deep \
    "${APP_BUNDLE}"

if [ $? -ne 0 ]; then
    echo_error "Code signing failed"
    exit 1
fi

# Verify signature
echo_info "Verifying code signature..."
codesign --verify --deep --strict --verbose=2 "${APP_BUNDLE}"

if [ $? -ne 0 ]; then
    echo_error "Code signature verification failed"
    exit 1
fi

# Create DMG
echo_info "Creating DMG..."

# Create temporary directory for DMG contents
DMG_TEMP_DIR="dmg_temp"
rm -rf "${DMG_TEMP_DIR}"
mkdir -p "${DMG_TEMP_DIR}"

# Copy app to temp directory
cp -R "${APP_BUNDLE}" "${DMG_TEMP_DIR}/"

# Create symbolic link to Applications folder
ln -s /Applications "${DMG_TEMP_DIR}/Applications"

# Create DMG
hdiutil create -volname "${APP_NAME}" \
    -srcfolder "${DMG_TEMP_DIR}" \
    -ov -format UDZO \
    "${DMG_NAME}"

if [ $? -ne 0 ]; then
    echo_error "DMG creation failed"
    exit 1
fi

# Clean up temp directory
rm -rf "${DMG_TEMP_DIR}"

# Sign the DMG
echo_info "Signing DMG..."
codesign --force --sign "${DEVELOPER_ID}" \
    --timestamp \
    "${DMG_NAME}"

if [ $? -ne 0 ]; then
    echo_error "DMG signing failed"
    exit 1
fi

# Notarize
echo_info "Submitting for notarization..."
echo_warning "You'll need an app-specific password from https://appleid.apple.com"
echo_info "Notarization can take 5-15 minutes..."

xcrun notarytool submit "${DMG_NAME}" \
    --apple-id "${APPLE_ID}" \
    --team-id "${TEAM_ID}" \
    --wait

if [ $? -ne 0 ]; then
    echo_error "Notarization failed"
    echo_info "You can check the log with: xcrun notarytool history --apple-id ${APPLE_ID} --team-id ${TEAM_ID}"
    exit 1
fi

# Staple the notarization ticket
echo_info "Stapling notarization ticket..."
xcrun stapler staple "${DMG_NAME}"

if [ $? -ne 0 ]; then
    echo_error "Stapling failed"
    exit 1
fi

# Verify stapling
echo_info "Verifying staple..."
xcrun stapler validate "${DMG_NAME}"

if [ $? -ne 0 ]; then
    echo_error "Staple validation failed"
    exit 1
fi

echo_info "✅ Build complete!"
echo_info "DMG created: ${DMG_NAME}"
echo_info "App bundle: ${APP_BUNDLE}"

# Display signature info
echo_info "Signature information:"
codesign -dvv "${APP_BUNDLE}"