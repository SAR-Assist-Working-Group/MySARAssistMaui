# Data Models

All domain models live in the `MySarAssistModels` class library so they can be shared between the MAUI app and future server-side or test projects without pulling in MAUI dependencies.

## Base Classes

### `SyncableItem`
Base for any entity that can be synced between devices or with the SCA server.

| Property | Type | Description |
|---|---|---|
| `ID` | `Guid` | Primary key |
| `LastUpdatedUTC` | `DateTime` | UTC timestamp of last change |
| `Active` | `bool` | Soft-delete flag |
| `OpPeriod` | `int` | Operational period number |
| `CreatedBy` | `string` | Creator identifier |

### `IncidentResource`
Extends `SyncableItem`. Base for any resource deployed to an incident.

## People

### `Personnel : IncidentResource`
Represents a SAR team member. Persisted to SQLite.

Key fields:

| Property | Type | Notes |
|---|---|---|
| `PersonID` | `Guid` | Alias for `ID` |
| `Name` | `string` | Required |
| `Email` | `string` | Required |
| `Callsign` | `string?` | Radio callsign |
| `Phone` | `string?` | Contact number |
| `Barcode` | `string?` | For barcode check-in/out |
| `OrganizationID` | `Guid` | FK to Organization |
| `QualificationList` | `bool[28]` | Qualification flags (SQLite-ignored; flattened to Qualification0…Qualification27) |
| `PacesPer100` | `double` | Calibrated pacing for distance estimation |
| `SignedInToTask` | `bool` | Current check-in status |
| `NOKName/Relation/Phone` | `string?` | Next of kin |

Derived qualification properties (computed from `QualificationList`): `GSAR`, `GSTL`, `SARM`, `FirstAid`, `RopeRescue`, `Swiftwater`, `MountainRescue`, `Tracker`, `BoatOperator`, `K9`, `CDFL`.

### `Organization`
Represents a SAR organization (e.g. a regional group). Fetched from SCA and cached locally.

| Property | Type | Notes |
|---|---|---|
| `OrganizationID` | `Guid` | Primary key |
| `ParentOrganizationID` | `Guid` | `Guid.Empty` for top-level orgs |
| `OrganizationName` | `string?` | Display name |
| `LogoFileName` | `string?` | Asset name for logo image |

### `Qualification`
Reference type describing a named qualification (index + name). Used by `PersonnelTools`.

## Assignments

### `Assignment : SyncableItem`
A field assignment issued to a SAR team during an incident.

Key fields:

| Property | Type | Notes |
|---|---|---|
| `AssignmentID` | `Guid` | Primary key |
| `AssignmentNumber` | `int` | Numeric identifier |
| `TeamName` | `string` | Team callsign/name |
| `Priority` | `int` | Assignment priority |
| `PlannedStart` | `DateTime` | Planned departure time |
| `PlannedDurrationMinutes` | `int` | Planned duration |
| `teamMembers` | `List<Personnel>` | Personnel on this assignment |
| `RADeMSOperationalRisk` | `int` | Recorded RADeMS scores |
| `RADeMSResponseCapability` | `int` | |
| `AreaOfAssignment` | `double` | km² |
| `TeamSpacing` | `double` | Metres between searchers |
| `RangeOfDetection` | `double` | Estimated detection range |
| `EstimatedSpeed` | `double` | km/h |
| `POA / POD / POS` | `double` | Probability of Area/Detection/Success |

These measurements are always stored in metric, whatever units the user has chosen to see;
conversion happens only in the view layer (see *Display Units* in `architecture.md`).

Assignment types (IRT, Tracking, Sound Sweep, Dog, Type 2/3 Grid, Rope Rescue, Swiftwater, etc.) are encoded as a bitstring in `AssignmentTypeCheckboxes`.

### `AssignmentDebrief`
Post-assignment debrief data, including actual spacing, speed, and range of detection.

### `AssignmentDebriefPackage`
Container pairing an `Assignment` with its `AssignmentDebrief`.

## Clues

### `Clue : SyncableItem`
A physical or informational clue found during a search. Persisted locally.

| Property | Type | Notes |
|---|---|---|
| `ClueID` | `Guid` | Primary key |
| `Description` | `string?` | Clue description |
| `FoundBy` | `string?` | Who found it |
| `DateFound` | `DateTime?` | When found |
| `LocationFound` | `string?` | Text description of location |
| `Latitude / Longitude` | `double?` | GPS coordinates |
| `ClueValueId` | `int?` | FK to `ClueValueOption` |
| `AssignmentID` | `Guid` | Associated assignment |

### `ClueValueOption` / `ClueAgeOption`
Reference enumerations for clue significance and age classifications.

## RADeMS (Risk Assessment)

### `RADeMSScore : SyncableItem`
A completed RADeMS risk assessment. Persisted to SQLite.

| Property | Type | Notes |
|---|---|---|
| `Scores` | `int[10]` | 5 Operational Risk + 5 Response Capacity scores (SQLite-ignored; flattened to ScoreValue0…9) |
| `OperationalRisk` | `int` | Sum of scores[0..4] (or manual override) |
| `ResponseCapacity` | `int` | Sum of scores[5..9] (or manual override) |
| `CategoryID` | `int` | Which RADeMS category was assessed |
| `SetByName` | `string` | Name of assessor |
| `Comment` | `string` | Free-text note |

### `RADeMSCategory`
Named grouping of RADeMS questions (e.g. Ground, Water, Technical).

### `RADeMSQuestion`
An individual scored question within a category, with answer options and score values.

## GIS / Coordinates

### `Coordinate` (in `GISTools.cs`)
Wraps a lat/lon pair with conversion methods: Decimal Degrees, DDM, DMS, UTM, MGRS.

### `GISTools` (static)
Spatial utility functions:
- `DistanceBetweenPlaces` — Haversine distance (km)
- `Bearing` — Rhumb-line bearing
- `CalculatePolygonAreaSquareMeters` — Shoelace formula
- `FindCentroid` — Polygon centroid
- `ShapeContainsLocation` — Point-in-polygon test
- `GetSunrise` / `GetSunset` — Via CoordinateSharp celestial calculations

## Interfaces

### `IDataStore<T>`
Generic CRUD contract implemented by all services:
```csharp
Task<bool> AddItemAsync(T item);
Task<bool> DeleteItemAsync(Guid id);
Task<T> GetItemAsync(Guid id);
Task<IEnumerable<T>> GetItems(bool forceRefresh = false);
Task<bool> UpdateItemAsync(T item);
Task<bool> UpsertItemAsync(T item);
```

### `IPersonnel`
Marker interface for personnel types.
