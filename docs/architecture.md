# Architecture Overview

MySAR Assist is a .NET MAUI application targeting Android, iOS, Windows, and macOS. It follows the **MVVM** pattern using `CommunityToolkit.Mvvm` and operates **offline-first** with a local SQLite database, syncing reference data from the SCA SOAP web service when connectivity is available.

## Solution Layout

```
MySARAssistMaui/
├── MySARAssist/                    # MAUI host project
│   ├── App.xaml / App.xaml.cs     # Application root
│   ├── AppShell.xaml              # Shell navigation definition
│   ├── MauiProgram.cs             # DI container configuration
│   ├── Constants.cs               # DB path, API endpoint
│   ├── Services/                  # Data access layer
│   ├── ViewModels/                # Presentation logic (MVVM)
│   │   ├── Calculators/           # Grid, linear, sweep width, coordinate, pacing
│   │   ├── CheckInOut/            # Barcode check-in/out, personnel management
│   │   ├── RADeMS/                # Risk assessment scoring and display
│   │   ├── Urgency/               # Urgency calculator decision tree
│   │   ├── IncidentInfoViewModels/# Incident item display (in development)
│   │   └── General/               # About page
│   ├── Views/                     # XAML pages and controls
│   │   ├── CalculatorViews/       # Search planning calculators
│   │   ├── CheckInOutViews/       # Personnel check-in/out screens
│   │   ├── RADeMSViews/           # Risk assessment screens
│   │   ├── UrgencyViews/          # Urgency assessment screen
│   │   ├── IncidentInfoViews/     # Incident item screens (in development)
│   │   └── GeneralViews/          # About page
│   ├── Models/                    # App-level models (validation, events, handlers)
│   ├── Converters/                # XAML value converters
│   ├── Interfaces/                # Platform abstractions (e.g. device orientation)
│   └── Platforms/                 # Per-platform entry points
│       ├── Android/
│       ├── iOS/
│       ├── MacCatalyst/
│       ├── Windows/
│       └── Tizen/
│
├── MySarAssistModels/              # Shared domain model library (no MAUI dependency)
│   ├── People/                    # Personnel, Organization, Qualification
│   ├── Assignments/               # Assignment, AssignmentDebrief, AssignmentType
│   ├── Clues/                     # Clue, ClueValueOption, ClueAgeOption
│   ├── RADeMS/                    # RADeMSScore, RADeMSCategory, RADeMSQuestion
│   ├── IncidentItems/             # IncidentItemType (reference data)
│   ├── Interfaces/                # IDataStore<T>, IPersonnel
│   ├── GISTools.cs                # Coordinate math, polygon area, sun times
│   ├── StatisticalTools.cs        # POA/POD/POS statistics
│   ├── StringExt.cs               # String extension utilities
│   ├── GuidExtension.cs           # GUID helpers
│   ├── SyncableItem.cs            # Base class for syncable entities
│   └── IncidentResource.cs        # Base class for incident resources
│
└── MySarAssistUnitTests/           # xUnit / NUnit test project
    ├── GISTests.cs
    ├── RademsUnitTests.cs
    └── TestTools.cs
```

## Layers

```
┌─────────────────────────────────────────────────┐
│                    Views (XAML)                  │
│  Shell navigation, pages, controls               │
├─────────────────────────────────────────────────┤
│                  ViewModels                      │
│  ObservableObject (CommunityToolkit.Mvvm)        │
│  Commands, property change notification          │
├─────────────────────────────────────────────────┤
│                   Services                       │
│  IDataStore<T> implementations                   │
│  SQLite (local) + SOAP (remote sync)             │
├─────────────────────────────────────────────────┤
│              MySarAssistModels                   │
│  Pure C# domain models, no MAUI dependency       │
│  SQLite attributes for persistence               │
└─────────────────────────────────────────────────┘
```

## Dependency Injection

Configured in `MauiProgram.cs`:

| Registration | Type | Notes |
|---|---|---|
| `PersonnelService` | Singleton | SQLite connection reused across app lifetime |
| All Views | Transient | New instance per navigation |
| `UrgencyCalculatorView` | Transient | Urgency assessment questionnaire |

Services not registered in DI (`OrganizationService`, `ClueService`, `IncidentInfoService`) are instantiated directly with `new` in calling code. `RademsService` is a stub (not yet implemented).

## Data Flow

### Local Persistence (SQLite)

```
View → ViewModel → Service → SQLiteAsyncConnection → mysarassist.db3
```

- Database file lives in `FileSystem.AppDataDirectory/mysarassist.db3`.
- Tables are created lazily via `conn.CreateTableAsync<T>()` before each operation.
- Models decorated with SQLite attributes (`[PrimaryKey]`, `[Ignore]`).
- Arrays (qualifications, RADeMS scores) are flattened to individual bool/int columns for SQLite compatibility.

### Remote Sync (SCA Web Service)

```
RestService → CAUpdatesWebserviceSoapClient (SOAP) → sarassist.ca
```

- `RestService` fetches parent organizations, then iterates to fetch child organizations.
- Organization data is stored locally via `OrganizationService`.
- Endpoint: `https://www.sarassist.ca/ICAUpdatesWebservice.asmx`
- The SOAP proxy is in `Connected Services/ServiceReference1/Reference.cs`.

## Key Third-Party Dependencies

| Package | Purpose |
|---|---|
| `sqlite-net-pcl` | Local SQLite ORM |
| `CommunityToolkit.Maui` | MAUI UI helpers |
| `CommunityToolkit.Mvvm` | `ObservableObject`, `[ObservableProperty]` |
| `ZXing.Net.Maui` | Barcode scanner (check-in/out) |
| `CoordinateSharp` | Coordinate format conversion (UTM, MGRS, DMS, DDM) and celestial calculations |
| `MetroLog.MicrosoftExtensions` | File and in-memory logging |

## Platform-Specific Code

| Feature | Mechanism |
|---|---|
| Device orientation lock | `IDeviceOrientationService` interface with per-platform implementations in `Platforms/Android/` and `Platforms/iOS/` |
| Entry keyboard "Done" button | `EntryHandler.AddDone()` in `MauiProgram.cs` |

## Logging

Dual-sink logging via `MetroLog`:
- **In-memory** ring buffer (1024 lines, Debug–Critical).
- **Rolling file** in `FileSystem.CacheDirectory/MetroLogs`, retained for 2 days.
- Debug sink added in `#if DEBUG` builds.

## Urgency Assessment

A sequential decision-tree calculator (EMCR-based) that walks through 8 risk-factor questions to determine urgency level. Unlike other features, it is **stateless** — no service or persistence layer. The entire logic lives in `UrgencyCalculatorViewModel`.

```
UrgencyCalculatorView (XAML) → UrgencyCalculatorViewModel (decision tree) → Result (in-memory only)
```

**Decision flow:**

| Q | Question | "Yes" → | "No" → |
|---|---|---|---|
| 1 | Risk to searchers? | SAR will not respond | Continue |
| 2 | Medical/injury/mental issues? | High Urgency | Continue |
| 3 | High-hazard areas? | High Urgency | Continue |
| 4 | Age a factor? | High Urgency | Continue |
| 5 | Weather hazards? | High Urgency | Continue |
| 6 | Diminishing daylight? | High Urgency | Continue |
| 7 | Equipment sufficient? | Continue (→ Q8) | High Urgency |
| 8 | Other survival factors? | Intermediate Urgency | Low Urgency |

Some questions treat "Unknown" conservatively (same as "Yes"): medical issues (Q2), weather (Q5), equipment (Q7). Others treat "Unknown" as "No": hazard areas (Q3), age (Q4).

Outcomes are displayed with color-coded result cards:
- **High Urgency** — red (`#C0392B`)
- **Intermediate Urgency** — amber (`#B07D0A`)
- **Low Urgency** — green (`#2E6B20`)
- **SAR will not respond** — dark grey (`#1A1A1A`)
