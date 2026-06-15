# Navigation & Views

MySAR Assist uses the .NET MAUI **Shell** with a **flyout** (hamburger) menu as the top-level navigation. Modal and detail pages are pushed onto the navigation stack with `Shell.Current.GoToAsync`.

## Shell Flyout Menu

Defined in `AppShell.xaml`:

| Menu Item | Icon | Root View |
|---|---|---|
| Home | home icon | `MainPage` |
| Calculators | calculator icon | `CalculatorsView` |
| Check In / Out | user-check icon | `CheckInOutView` |
| RADeMS Risk Assessment | radems icon | `RADeMSView` |
| About My SAR Assist | info icon | `AboutView` |
| ~~Incident Information~~ | *(commented out)* | `IncidentItemsListPage` |

## Screen Inventory

### Home

**`MainPage`** — Landing page. Entry point when the app opens.

---

### Calculators (`Views/CalculatorViews/`)

**`CalculatorsView`** — Menu listing the available field calculators.

| Screen | ViewModel | Purpose |
|---|---|---|
| `GridSearchView` | `GridWorkEstimationViewModel` | Estimates time to search a grid area given team size, spacing, and speed |
| `LinearSearchView` | `LinearWorkEstimationViewModel` | Estimates effort for a linear (route/corridor) search |
| `SweepWidthCalculatorView` | `SweepWidthCalculatorViewModel` | Calculates effective sweep width from detection parameters |
| `VisualSearchResourceEstimationView` | `VisualSearchResourceEstimationViewModel` | Estimates resources needed for visual search coverage |
| `CoordinateConverterView` | `CoordinateConverterViewModel` | Converts between DD, DDM, DMS, UTM, MGRS formats using CoordinateSharp |
| `DistanceToPacingPage` | `PacingCalculatorViewModel` | Converts a distance to paces given a calibrated paces-per-100m rate |
| `PacingToDistancePage` | `PacingCalculatorViewModel` | Converts paces to distance |
| `HowToRangeOfDetectionPage` | *(informational)* | Explains how to determine Range of Detection |

---

### Check In / Out (`Views/CheckInOutViews/`)

**`CheckInOutView`** — Hub for personnel tracking at the incident.

| Screen | ViewModel | Purpose |
|---|---|---|
| `CheckInView` | `BarcodeCheckInViewModel` | Scan or manually enter a barcode to check a person in |
| `CheckOutView` | `BarcodeChecKOutViewModel` | Scan or manually enter a barcode to check a person out |
| `PersonnelListView` | `PersonnelListViewModel` | List all personnel in the local DB; tap to edit |
| `PersonnelEditView` | `PersonnelEditViewModel` | Create or edit a personnel record (name, callsign, phone, NOK, org, barcode) |
| `EditQualificationsPage` | `EditQualificationsViewModel` | Toggle qualification checkboxes for a person |
| `CheckInManagementViewModel` | *(shared)* | Manages the overall check-in/out state |

---

### RADeMS Risk Assessment (`Views/RADeMSViews/`)

**`RADeMSView`** — Entry point for risk assessment. Lists assessment categories.

| Screen | ViewModel | Purpose |
|---|---|---|
| `RADeMSDetailsPage` | `RADeMSDetailsViewModel` | Walks through the questions in a category; accumulates scores |
| `RADeMSCardPage` | `RADeMSCardViewModel` | Displays the completed score plotted on the RADeMS risk matrix image using a custom `IDrawable` overlay |

**RADeMS Score Dimensions:**

- **Operational Risk** (score 0–10): Sum of 5 sub-scores. Higher = more dangerous operation.
- **Response Capacity** (score 0–10): Sum of 5 sub-scores. Higher = better prepared team.

The result is plotted on a 2D matrix (x = Response Capacity, y = Operational Risk). Both scores can also be set manually to override the calculated values.

Supporting ViewModels:
- `RADeMSArchiveViewModel` — View past assessments.
- `RADeMSCategoryViewModel` — Category list item.
- `RADeMSQuestionViewModel` — Individual question with answer selection.
- `RADeMSTypesViewModel` — Assessment type selector.

---

### Incident Information *(in development)*

**`IncidentItemsListPage`** / `IncidentInfoListViewModel` — List of incident items (clues, debriefs). Currently commented out of the Shell navigation.

**`IncidentInfoItemViewModel`** — Wraps a `Clue` or similar item for display.

---

### About

**`AboutView`** / `AboutViewModel` — App information and links.

## ViewModel Base

All ViewModels extend `ObservableObject` from `CommunityToolkit.Mvvm`, which provides `INotifyPropertyChanged` support. Commands are implemented as `ICommand` / `Command` instances wired up in the ViewModel constructor.

## Value Converters (`Converters/`)

| Converter | Purpose |
|---|---|
| `InverseBoolConverter` | Negates a bool binding |
| `DoubleConverter` | String ↔ double for entry fields |
| `IntConverter` | String ↔ int for entry fields |
| `WebServiceConverters` | Converters for transforming web service response data for display |

## Validation Behaviors (`Models/ValidationTools/`)

XAML behaviors attached to `Entry` controls:

| Behavior | Rule |
|---|---|
| `BaseValidationBehaviour` | Abstract base |
| `NonBlankValidationBehavior` | Entry must not be empty |
| `EmailValidatorBehavior` | Entry must be a valid email format |
| `PasswordValidationBehavior` | Entry must meet password complexity requirements |
