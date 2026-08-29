# Services

Services live in `MySARAssist/Services/` and are responsible for all data access — both local SQLite persistence and remote API calls.

## `PersonnelService : IDataStore<Personnel>`

**Registration:** Singleton (DI container).

Manages the local roster of SAR personnel.

| Method | Description |
|---|---|
| `AddItemAsync` | Insert a new person into SQLite |
| `GetItemAsync(Guid)` | Fetch by ID; also resolves `MemberOrganization` via `OrganizationService` |
| `GetItems` | Return all personnel, sorted by name |
| `UpdateItemAsync` | Update an existing record |
| `UpsertItemAsync` | Insert-or-update by `PersonID` |
| `DeleteItemAsync` | Delete by `PersonID` |
| `GetMostFrequentOrganizationAsync` | Returns the organization with the most members in the local DB; falls back to a hardcoded "Unassigned" org GUID |
| `GetCurrentPersonAsync` | Reads `SelectedPersonID` from `Preferences` and returns that person |
| `setCurrentPerson(Guid)` | Persists `SelectedPersonID` to `Preferences` |

## `OrganizationService : IDataStore<Organization>`

**Registration:** Instantiated with `new` where needed.

Caches SAR organizations fetched from the SCA web service.

Additional method:
- `GetItems(Guid parentID)` — Filters organizations by parent for hierarchical display.
- `AreOrgsEqual` — Reflection-based string-property comparison (used for update detection).

## `RestService`

**Registration:** Instantiated with `new` where needed.

Fetches organization data from the SCA SOAP web service.

| Method | Description |
|---|---|
| `RefreshDataAsync` | Fetches all parent organizations, then recursively fetches child organizations for each. Returns the combined flat list. |

Internally uses the generated `CAUpdatesWebserviceSoapClient` SOAP proxy (`Connected Services/ServiceReference1`).

## `ClueService`

**Registration:** Instantiated with `new` where needed.

Manages clue records in the local SQLite database.

| Method | Description |
|---|---|
| `AddItemAsync` | Insert a clue |
| `GetItemsAsync` | Return all clues |
| `GetItemAsync(Guid)` | Fetch a single clue by `ClueID` |
| `UpdateItemAsync` | Update an existing clue |
| `DeleteItemAsync` | Delete a clue by `ClueID` |

## `IncidentInfoService`

**Registration:** Instantiated with `new` where needed.

Aggregates incident-related data into view model objects for display.

| Method | Description |
|---|---|
| `GetAllIncidentInfoVMs` | Returns a list of `IncidentInfoItemViewModel` built from all stored clues. Assignment and debrief integration is scaffolded but not yet implemented. |

## `RademsService : IDataStore<RADeMSScore>`

**Registration:** Not registered (stub only).

All methods throw `NotImplementedException`. RADeMS scoring is currently handled entirely within the ViewModels without persistence.

## Database

All SQLite-backed services share the same database file:

```
FileSystem.AppDataDirectory / mysarassist.db3
```

Tables are created on first access via `conn.CreateTableAsync<T>()`. There is no migration system; schema changes require manual handling or clearing the database.
