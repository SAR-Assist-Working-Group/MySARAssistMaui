# Build and Run

## Target Frameworks

The project conditionally includes platforms based on the host OS:

| Platform | Builds on |
|---|---|
| `net9.0-android` | Linux, macOS, Windows |
| `net9.0-ios` | macOS only |
| `net9.0-windows10.0.26100.0` | Windows only |

This means Android is the only target you can build from a WSL2 (Linux) environment.

---

## Option A — Build from Windows (Recommended)

This is the fastest path if you have Android Studio installed on Windows, because the Android SDK and emulator are already set up there.

### Prerequisites (Windows)

- **[.NET 9 SDK for Windows](https://dotnet.microsoft.com/download/dotnet/9.0)** — must be installed on the Windows side, not just in WSL2. Download the *Windows x64 Installer* and run it. Verify with `dotnet --version` in a new PowerShell window.
- Android Studio installed, with at least one AVD (emulator) configured
- MAUI Android workload (run once after installing the SDK):
  ```powershell
  dotnet workload install maui-android
  ```

### Steps

**1. Start your emulator** in Android Studio (AVD Manager → select device → ▶).

**2. Open PowerShell** and navigate to the project using the WSL2 network path:

```powershell
cd \\wsl.localhost\Ubuntu\home\<your-username>\GIT\gfnord\MySARAssistMaui
```

> Windows exposes your WSL2 filesystem at `\\wsl.localhost\<distro-name>\`. No need to copy files.

**3. Build and run:**

```powershell
dotnet run --project MySARAssist\MySARAssist.csproj -f net9.0-android
```

`dotnet run` will build, find the connected emulator via ADB, install the APK, and launch the app automatically.

**Or build and install separately:**

```powershell
# Build
dotnet build MySARAssist\MySARAssist.csproj -f net9.0-android

# Verify the emulator is visible
adb devices

# Install
adb install MySARAssist\bin\Debug\net9.0-android\ca.greathat.mysarassist-Signed.apk
```

---

## Option B — Build from WSL2 (Linux)

Use this if you prefer keeping the entire toolchain inside WSL2.

### Prerequisites (WSL2)

- Java JDK (OpenJDK 17 or 21):
  ```bash
  sudo apt install openjdk-21-jdk
  ```
- MAUI Android workload:
  ```bash
  dotnet workload install maui-android
  ```
- Android SDK (command-line tools for Linux):

  ```bash
  mkdir -p ~/android-sdk/cmdline-tools
  cd /tmp
  wget "https://dl.google.com/android/repository/commandlinetools-linux-11076708_latest.zip" -O cmdline-tools.zip
  unzip cmdline-tools.zip -d ~/android-sdk/cmdline-tools
  mv ~/android-sdk/cmdline-tools/cmdline-tools ~/android-sdk/cmdline-tools/latest
  ```

  Add to `~/.zshrc` (or `~/.bashrc`):
  ```bash
  export ANDROID_HOME=$HOME/android-sdk
  export PATH=$PATH:$ANDROID_HOME/cmdline-tools/latest/bin:$ANDROID_HOME/platform-tools
  ```

  Then install the required SDK components:
  ```bash
  source ~/.zshrc
  sdkmanager --licenses
  sdkmanager "platform-tools" "platforms;android-35" "build-tools;35.0.0"
  ```

### Steps

**1. Start your emulator** on the Windows side (via Android Studio).

**2. Connect ADB from WSL2:**

```bash
WINDOWS_IP=$(cat /etc/resolv.conf | grep nameserver | awk '{print $2}')
adb connect $WINDOWS_IP:5555
adb devices   # should list the emulator
```

**3. Build and install:**

```bash
dotnet build MySARAssist/MySARAssist.csproj -f net9.0-android
adb install MySARAssist/bin/Debug/net9.0-android/ca.greathat.mysarassist-Signed.apk
```

---

## Troubleshooting

| Error | Cause | Fix |
|---|---|---|
| `No .NET SDKs were found` | .NET SDK not installed on Windows (WSL2 install is separate) | Install [.NET 9 SDK for Windows](https://dotnet.microsoft.com/download/dotnet/9.0), reopen PowerShell |
| `NETSDK1147: maui-android workload not installed` | Workload missing | `dotnet workload install maui-android` |
| `NETSDK1178: Microsoft.iOS.Sdk not found` | iOS TFM evaluated on non-macOS | Already fixed in csproj — iOS only builds on macOS |
| `XA5300: Android SDK directory could not be found` | `ANDROID_HOME` not set | Set `ANDROID_HOME` or use Option A (Windows build) |
| `adb devices` shows nothing | Emulator not started or ADB not connected | Start emulator first; on WSL2 run `adb connect <WINDOWS_IP>:5555` |
| ADB connection refused from WSL2 | Windows Firewall blocking port 5555 | Allow port 5555 in Windows Defender Firewall for the WSL2 network adapter |
| APK installs but app crashes immediately | Debug/signing mismatch | Use the `-Signed.apk` file from the `Debug` output folder |
