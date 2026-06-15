# MySAR Assist

A cross-platform mobile companion app for Search and Rescue (SAR) personnel, built with .NET MAUI. Designed to work alongside the [SAR Command Assist (SCA)](https://www.sarassist.ca) program.

## Features

- **Personnel Check In / Out** — Track SAR responders at an incident using manual entry or barcode scanning. Manage qualifications (GSAR, GSTL, First Aid, Rope Rescue, Swiftwater, etc.) and next-of-kin information.
- **RADeMS Risk Assessment** — Rapid Assessment Decision Making System. Guides users through structured risk questions and plots the result (Operational Risk vs. Response Capacity) on a risk matrix.
- **Urgency Assessment** — EMCR-based decision-tree calculator. Walks through 8 risk-factor questions (searcher risk, medical, hazards, age, weather, daylight, equipment, other factors) to determine urgency level: High, Intermediate, Low, or SAR will not respond.
- **Search Calculators** — Field calculators for grid search time estimation, linear search work estimation, sweep width, visual search resource estimation, coordinate conversion (DD, DDM, DMS, UTM, MGRS), and pacing distance conversion.
- **Incident Information** *(in development)* — Clue logging and assignment debrief tracking.
- **Organization Directory** — SAR organizations fetched from the SCA web service and cached locally.

## Platform Support

| Platform | Status |
|----------|--------|
| Android  | Supported |
| iOS      | Supported |
| Windows  | Supported |
| macOS (Catalyst) | Supported |
| Tizen    | Entry point present |

## Solution Structure

```
MySARAssistMaui.sln
├── MySARAssist/          # Main MAUI application
├── MySarAssistModels/    # Shared domain models (class library)
└── MySarAssistUnitTests/ # Unit tests (GIS, RADeMS)
```

See [`docs/`](docs/) for architecture documentation.

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 17.8+ with the MAUI workload, or JetBrains Rider with MAUI support

### Build

```bash
dotnet restore
dotnet build
```

### Run on Android (from Windows)

This is the recommended approach, since Android Studio already provides the Android SDK and emulator.

**1. Install the [.NET 9 SDK for Windows](https://dotnet.microsoft.com/download/dotnet/9.0)**. Reopen PowerShell and confirm with `dotnet --version`.

**2. Install the MAUI Android workload** (once, in PowerShell):

```powershell
dotnet workload install maui-android
```

**3. Start an emulator** in Android Studio (AVD Manager → pick a device → ▶).

**4. Build and install the APK:**

```powershell
dotnet build MySARAssist\MySARAssist.csproj -f net9.0-android

# Install the APK to the running emulator
adb install MySARAssist\bin\Debug\net9.0-android\ca.greathat.mysarassist-Signed.apk
```

See [`docs/build-and-run.md`](docs/build-and-run.md) for troubleshooting.

### Rebuild and Redeploy to an Android Emulator

After making code changes, rebuild and redeploy with:

```powershell
# Clean previous build artifacts to force a fresh APK
Remove-Item -Recurse -Force MySARAssist\obj, MySARAssist\bin

# Build in Release mode (Debug uses Fast Deployment which can crash on some emulators)
dotnet build MySARAssist\MySARAssist.csproj -f net9.0-android -c Release

# Uninstall the old version
adb uninstall ca.greathat.mysarassist

# Install the new APK
adb install MySARAssist\bin\Release\net9.0-android\ca.greathat.mysarassist-Signed.apk
```

> **Note:** If the .NET 10 SDK is installed alongside .NET 9, pin the SDK version with a `global.json` file to ensure the correct tooling is used:
> ```powershell
> dotnet new globaljson --sdk-version 9.0.315 --force
> ```

## Documentation

| Document | Description |
|----------|-------------|
| [Architecture Overview](docs/architecture.md) | Solution structure, layers, data flow |
| [Build and Run](docs/build-and-run.md) | Building for Android from Windows |
| [Data Models](docs/data-models.md) | Domain model reference |
| [Services](docs/services.md) | Service layer and persistence |
| [Navigation & Views](docs/navigation-views.md) | Shell navigation and screen inventory |

## License

See [LICENSE.txt](LICENSE.txt).
