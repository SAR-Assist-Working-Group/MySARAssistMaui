# Build and Run

## Target Frameworks

The project conditionally includes platforms based on the host OS:

| Platform | Builds on |
|---|---|
| `net9.0-android` | Linux, macOS, Windows |
| `net9.0-ios` | macOS only |
| `net9.0-windows10.0.26100.0` | Windows only |

---

## Build from Windows

### Prerequisites

- **[.NET 9 SDK for Windows](https://dotnet.microsoft.com/download/dotnet/9.0)** — download the *Windows x64 Installer* and run it. Verify with `dotnet --version`.
- Android Studio installed, with at least one AVD (emulator) configured
- MAUI Android and iOS workloads (run once after installing the SDK):
  ```powershell
  dotnet workload install maui-android
  dotnet workload install ios
  ```
- Pin SDK version to .NET 9 (required if .NET 10 SDK is also installed):
  ```powershell
  dotnet new globaljson --sdk-version 9.0.315 --force
  ```

### Steps

**1. Start your emulator** in Android Studio (AVD Manager → select device → ▶).

**2. Build the APK:**

```powershell
dotnet build MySARAssist\MySARAssist.csproj -f net9.0-android
```

**3. Install to the emulator:**

```powershell
adb install MySARAssist\bin\Debug\net9.0-android\ca.greathat.mysarassist-Signed.apk
```

> If the app crashes on launch, use **Release** mode instead (Debug uses Fast Deployment which can fail on some emulators):
> ```powershell
> dotnet build MySARAssist\MySARAssist.csproj -f net9.0-android -c Release
> adb install MySARAssist\bin\Release\net9.0-android\ca.greathat.mysarassist-Signed.apk
> ```

---

## Clean Rebuild and Redeploy

After making code changes, do a clean rebuild to ensure the APK is regenerated:

```powershell
Remove-Item -Recurse -Force MySARAssist\obj, MySARAssist\bin
dotnet build MySARAssist\MySARAssist.csproj -f net9.0-android -c Release
adb uninstall ca.greathat.mysarassist
adb install MySARAssist\bin\Release\net9.0-android\ca.greathat.mysarassist-Signed.apk
```

---

## SDK Version Pinning

If multiple .NET SDK versions are installed (e.g., .NET 9 and .NET 10), pin the project to .NET 9 with a `global.json`:

```powershell
dotnet new globaljson --sdk-version 9.0.315 --force
```

---

## Troubleshooting

| Error | Cause | Fix |
|---|---|---|
| `No .NET SDKs were found` | .NET SDK not installed | Install [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0), reopen PowerShell |
| `NETSDK1147: maui-android workload not installed` | Workload missing | `dotnet workload install maui-android` |
| `NETSDK1178: Microsoft.iOS.Sdk not found` | iOS TFM evaluated on non-macOS | Already fixed in csproj — iOS only builds on macOS |
| `XA5300: Android SDK directory could not be found` | `ANDROID_HOME` not set | Set `ANDROID_HOME` or use Android Studio's SDK path |
| `adb devices` shows nothing | Emulator not started | Start emulator first |
| APK installs but app crashes immediately | Debug Fast Deployment incompatibility | Use Release mode (`-c Release`) |
| `monodroid: ALL entries in APK named lib/x86_64/ MUST be STORED` | Compressed native libraries | Rebuild in Release mode with clean obj/bin folders |
