# TimeTracker Application - Architecture & Developer Guide

## Table of Contents

1. [Overview](#overview)
2. [Solution Structure](#solution-structure)
3. [How to Build and Run](#how-to-build-and-run)
4. [Request Flow](#request-flow)
5. [Authentication and Authorization](#authentication-and-authorization)
6. [Core Domain Entities](#core-domain-entities)
7. [API Reference](#api-reference)
8. [Data Access Layer](#data-access-layer)
9. [Web Frontend Architecture](#web-frontend-architecture)
10. [Dependency Injection](#dependency-injection)
11. [Cross-Cutting Concerns](#cross-cutting-concerns)
12. [Configuration Reference](#configuration-reference)
13. [Key Features](#key-features)
14. [Known Issues and Technical Debt](#known-issues-and-technical-debt)
15. [Development Workflow](#development-workflow)

---

## Overview

TimeTracker is an internal billable-hours tracking application used to record employee time entries against projects. It is built as a two-tier .NET application: an ASP.NET Web API backend and an ASP.NET MVC web frontend. The backend uses Entity Framework with SQL Server stored procedures for all data access.

The web frontend never talks to the database directly. All data flows through the REST API.

---

## Solution Structure

The repository root is organized under `Main/`:

```
Main/
├── SBS.IT.Utilities.TimeTrackerAPI.sln          # API solution
├── SBS.IT.Utilities.TimeTrackerWeb.sln          # Web frontend solution
├── Services/Source/
│   ├── SBS.IT.Utilities.API.TimeTrackerWebAPI/  # Web API 2 project
│   └── SBS.IT.Utilities.DataAccess/
│       └── SBS.IT.Utilities.DataAccess.TimeTrackerDb/  # EF data access
├── Web/Source/
│   ├── SBS.IT.Utilities.Web.TimeTrackerWeb/     # MVC frontend project
│   └── SBS.IT.Utilities.Shared.APIClient/       # REST client library
└── Shared/Source/
    ├── SBS.IT.Utilities.Shared/                 # Shared utilities
    └── SBS.IT.Utilities.Logger/                 # Log4Net wrapper
```

### Projects

| Project | Path | Purpose |
|---------|------|---------|
| **TimeTrackerWebAPI** | `Services/Source/SBS.IT.Utilities.API.TimeTrackerWebAPI/` | ASP.NET Web API 2 (.NET 4.6.1) - REST API layer |
| **TimeTrackerDb** | `Services/Source/SBS.IT.Utilities.DataAccess/SBS.IT.Utilities.DataAccess.TimeTrackerDb/` | Entity Framework data access layer using stored procedures |
| **TimeTrackerWeb** | `Web/Source/SBS.IT.Utilities.Web.TimeTrackerWeb/` | ASP.NET MVC frontend with Kendo UI grids |
| **Shared.APIClient** | `Web/Source/SBS.IT.Utilities.Shared.APIClient/` | HTTP client library (static HttpClient) the web frontend uses to call the API |
| **Shared** | `Shared/Source/SBS.IT.Utilities.Shared/` | Shared utilities: AutoMapper config, FluentValidation, caching, email, base models |
| **Logger** | `Shared/Source/SBS.IT.Utilities.Logger/` | Log4Net logging wrapper |

### Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Backend API | ASP.NET Web API 2 | .NET Framework 4.6.1 |
| Frontend | ASP.NET MVC 5 with Razor views | .NET Framework 4.6.1 |
| UI Components | Telerik Kendo UI for ASP.NET MVC | 2015.3.930 |
| Database | SQL Server (stored procedures) | — |
| ORM | Entity Framework 6 (Database-First, EDMX) | — |
| Object Mapping | AutoMapper | — |
| Validation | FluentValidation | — |
| HTTP Client | Static HttpClient (replaced RestSharp) | — |
| Logging | Log4Net | — |
| DI Container | Unity | 4.0.1 |
| Reports | SQL Server Reporting Services (SSRS) | ReportViewer |
| Authentication | ASP.NET Forms Authentication + Active Directory | — |
| JS Libraries | jQuery 1.12.4, Bootstrap, SweetAlert2 | — |

---

## How to Build and Run

### Prerequisites

- Visual Studio 2017+ (with .NET Framework 4.6.1 targeting pack)
- SQL Server instance with the TimeTracker database and stored procedures deployed
- IIS or IIS Express for hosting

### Build Steps

1. Open `Main/SBS.IT.Utilities.TimeTrackerAPI.sln` in Visual Studio and build (restores NuGet packages automatically).
2. Open `Main/SBS.IT.Utilities.TimeTrackerWeb.sln` in Visual Studio and build.
3. Alternatively, from the command line:
   ```
   msbuild Main/SBS.IT.Utilities.TimeTrackerAPI.sln /p:Configuration=Release
   msbuild Main/SBS.IT.Utilities.TimeTrackerWeb.sln /p:Configuration=Release
   ```

### Running Locally

1. **Start the API** - Set `TimeTrackerWebAPI` as the startup project and run (F5). It will launch on a local port (e.g., `http://localhost:PORT/`).
2. **Configure the Web frontend** - In `Web/Source/SBS.IT.Utilities.Web.TimeTrackerWeb/Web.config`, set the `ServiceAddress` app setting to point to the running API:
   ```xml
   <add key="ServiceAddress" value="http://localhost:PORT/" />
   ```
3. **Start the Web frontend** - Set `TimeTrackerWeb` as the startup project and run. It launches at its own port and redirects to the login page (`/Account/Login`).

### Database

The application requires a SQL Server database with all stored procedures pre-deployed. The Entity Framework EDMX model (`TimeTrackerDb.edmx`) maps to these procedures. The connection string is in the API project's `Web.config`.

---

## Request Flow

```
┌─────────┐    HTTP     ┌──────────────────────┐   HTTP (HttpClient)   ┌─────────────────────┐
│ Browser  │ ──────────> │  MVC Web Frontend    │ ────────────────────> │  Web API Backend    │
│          │ <────────── │  (TimeTrackerWeb)     │ <──────────────────── │  (TimeTrackerWebAPI)│
│          │   HTML/JS   │                      │   JSON                │                     │
└─────────┘              └──────────────────────┘                      └─────────┬───────────┘
                                                                                 │
                                                                    EF + Stored Procs
                                                                                 │
                                                                       ┌─────────▼───────────┐
                                                                       │    SQL Server        │
                                                                       │    (TimeTrackerDb)   │
                                                                       └──────────────────────┘
```

### Detailed Flow (Example: Loading Timesheet)

1. **Browser** requests `GET /TimeSheet/Index`
2. **MVC Controller** (`TimeSheetController.Index()`) checks session via `SessionTimeoutAttribute`
3. Controller calls `APIExtension.InvokeGet<T>()` with the API URL from `APIResources` constants
4. **APIExtension** sends HTTP GET with `X-Correlation-ID` and `X-Api-Key` headers to the API
5. **API Pipeline**: `ApiKeyAuthHandler` validates the key → `ValidateModelAttribute` checks model state → Controller action executes
6. **API Controller** calls `ITrackerDbRepository` methods (e.g., `GetTimeEntry()`)
7. **Repository** (`EFTimeTrackerDbRepository`) calls stored procedure via EF context
8. **EF** returns `usp_*_Result` objects → **AutoMapper** maps to domain models → returned as JSON
9. **MVC Controller** receives deserialized model, populates `ViewBag`/`ViewData`, returns Razor view
10. **Browser** renders the Kendo UI grid from the view

### API Client

The web frontend communicates with the API through `APIExtension` (`Shared.APIClient/Implementation/APIExtension.cs`):

- Uses a **static `HttpClient`** instance (prevents socket exhaustion)
- Adds **`X-Correlation-ID`** header to every request for cross-tier tracing
- Adds **`X-Api-Key`** header for API authentication
- Logs errors with correlation ID and **throws** on failure (no silent swallowing)
- Two core methods: `InvokeGet<T>(Uri)` and `InvokePost<T>(Uri, object)`

API endpoint URLs are assembled from `IAPIConfiguration.ServiceBaseAddress` + constants defined in `APIResources` (`Shared.APIClient/Message/CommonTypeMessages.cs`).

---

## Authentication and Authorization

### Login Flow

1. User navigates to `/Account/Login` (the default route)
2. `AccountController.Login(LoginModel)` calls API: `api/Employee/GetAuthentication`
3. API authenticates against **Active Directory** (production) or **database** (non-production) via `System.DirectoryServices.AccountManagement`
4. On success, the API returns the employee record
5. MVC frontend creates a `FormsAuthentication` ticket (cookie) with serialized credentials
6. Employee data is stored in `HttpContext.Session` via `SessionCacheManager`
7. User is redirected based on role:
   - **SAN/ADN** (admin) → `Home/Index` (admin dashboard)
   - **Other** → `TimeSheet/Index` (timesheet entry)

### Session Management

- `SessionCacheManager` wraps `HttpContext.Current.Session` with get/set/remove helpers
- Session timeout is set to **60 minutes**
- `SessionTimeoutAttribute` (action filter on all controllers) checks if the user's `AuthenticateExtension.UserType` is null and redirects to login if expired

### User Types

| Code | Role | Dashboard | Capabilities |
|------|------|-----------|-------------|
| **SAN** | Super Admin | Admin dashboard with counts | View all employees' entries, full admin access |
| **ADN** | Admin | Admin dashboard with counts | Manage employees under them, admin timesheet |
| *(other)* | Regular User | Timesheet entry page | Enter own time, view own weekly status |

### API Security

- `ApiKeyAuthHandler` (`DelegatingHandler`) validates the `X-Api-Key` header on every API request
- If no key is configured in `Web.config` (`ApiKey` app setting is empty), validation is bypassed
- Invalid keys receive HTTP 401 Unauthorized

---

## Core Domain Entities

### Time Entry (Central Entity)

Each time entry records a block of work performed by an employee:

| Field | Type | Description |
|-------|------|-------------|
| `TimeEntryId` | int | Primary key |
| `EmployeeId` | int | Who logged the time |
| `ProjectId` | int | Which project |
| `ProjectItemId` | int | Which sub-item of the project |
| `WorkTypeId` | int | Type of work (Development, Testing, etc.) |
| `Date` | DateTime | Date of work |
| `WorkHour` | decimal | Hours worked |
| `Comments` | string | Description of work done |
| `WorkItem` | string | ALM/INC reference number |

Time entries are submitted in batches via `TimeEntryRequestModel` (contains a list of `TimeEntryModel` objects).

### Project

Top-level billing entity with name, description, type (`ProjectTypeId`), and active/inactive status.

### Project Item

Sub-unit of a project. Tracks budget, benefit amounts, level-of-effort estimates (`DevLOE`, `QaLOE`, `BaLOE`), and status (`ProjectItemStatusId`).

### Employee

| Field | Description |
|-------|-------------|
| `EmployeeId` | Primary key |
| `FirstName`, `LastName`, `Email`, `Phone` | Personal info |
| `LocationId` | Office location |
| `EmploymentTypeId` | Employment classification |
| `UserTypeId` | Role/permissions (SAN, ADN, or regular) |
| `TeamId` / `TeamCode` | Team assignment (DEV, QA, BA, PS, RPT, MGT) |
| `ManagerId` | Reporting manager |
| `IsTimeEntryEnable` | Flag to allow/deny time entry access |
| `TimeZoneId` | Employee's time zone |

### Work Type

Categorizes work. Each has a code (DEV, TES, PST, RPT, PMT, ADN), belongs to a `WorkTypeCategory`, and has an `IsCapitalizable` flag.

### Supporting/Lookup Entities

- **Team** - organizational teams (DEV, QA, BA, PS, RPT, MGT)
- **Location** - office locations
- **TimeZone** (`EmployeeTimeZone`) - time zone reference data
- **EmploymentType** - employment classifications
- **UserType** - role classifications
- **Application** - legacy entity (largely commented out)
- **ProjectType** - project type classifications
- **ProjectItemStatus** - status values for project items
- **WorkTypeCategory** - groupings for work types
- **Manager** - manager lookup (derived from Employee)
- **ReportRegistryDetail** - SSRS report definitions with server URLs and paths

---

## API Reference

All API routes are prefixed with `api/` and use attribute routing. The API pipeline processes requests through: `ApiKeyAuthHandler` → `ValidateModelAttribute` → `GlobalExceptionFilterAttribute` → Controller action.

### Time Entry Endpoints (`TimeEntryController`)

| Method | Route | Parameters | Returns | Description |
|--------|-------|------------|---------|-------------|
| GET | `api/TimeEntry/GetTimeEntry` | `employeeId`, `startDate`, `endDate` | `List<TimeEntryModel>` | Get time entries for date range |
| POST | `api/TimeEntry/TimeEntryAdd` | `TimeEntryRequestModel` (body) | `TimeEntryModel` | Add batch of time entries |
| POST | `api/TimeEntry/TimeEntryUpdate` | `TimeEntryModel` (body) | `TimeEntryModel` | Update a time entry |
| POST | `api/TimeEntry/TimeEntryDelete` | `TimeEntryModel` (body) | `TimeEntryModel` | Delete a time entry |
| GET | `api/TimeEntry/TimeEntrySearch` | `employeeId`, `startDate`, `endDate` | `List<TimeEntryModel>` | Search time entries |
| GET | `api/TimeEntry/GetTimeEntryWeeklyStatus` | `employeeId`, `startDate`, `endDate` | `List<TimeEntryWeeklyStatusModel>` | Weekly status summary |
| GET | `api/TimeEntry/GetLastTimeEntry` | `employeeId` | `TimeEntryModel` | Last time entry for employee |
| GET | `api/TimeEntry/GetTimeEntryDistinctRecords` | `employeeId` | `List<TimeEntryModel>` | Distinct previous entries for quick re-entry |
| GET | `api/TimeEntry/GetAdminProjectView` | `projectId`, `month`, `year` | `List<AdminProjectViewModel>` | Project hours by location/work type |
| GET | `api/TimeEntry/GetAdminProjectViewExport` | `projectId`, `month`, `year` | `List<AdminProjectViewExportModel>` | Per-employee project detail for export |

### Project Endpoints (`ProjectController`)

| Method | Route | Description |
|--------|-------|-------------|
| GET | `api/Project/GetProject` | Get all projects |
| GET | `api/Project/GetByProjectId?projectId=` | Get single project |
| POST | `api/Project/ProjectAdd` | Add project |
| POST | `api/Project/ProjectUpdate` | Update project |
| POST | `api/Project/ProjectDelete` | Delete project |
| GET | `api/Project/ProjectSearch?search=` | Search projects |
| GET | `api/Project/GetProjectType` | Get project types |

### Project Item Endpoints (`ProjectItemController`)

| Method | Route | Description |
|--------|-------|-------------|
| GET | `api/Project/GetProjectItem` | Get all project items |
| GET | `api/Project/GetByProjectItemId?projectItemId=` | Get single item |
| GET | `api/Project/GetActiveProjectItem` | Get active items only |
| POST | `api/Project/ProjectItemAdd` | Add project item |
| POST | `api/Project/ProjectItemUpdate` | Update project item |
| POST | `api/Project/ProjectItemDelete` | Delete project item |
| GET | `api/Project/ProjectItemSearch?search=` | Search project items |
| GET | `api/Project/ProjectItemListGetBySearch?search=` | Search project item list |
| GET | `api/Project/GetProjectItemStatus` | Get status values |

### Employee Endpoints (`EmployeeController`)

| Method | Route | Description |
|--------|-------|-------------|
| POST | `api/Employee/GetAuthentication` | Authenticate user (AD or DB) |
| POST | `api/Employee/EmployeeAdd` | Add employee |
| POST | `api/Employee/EmployeeUpdate` | Update employee |
| POST | `api/Employee/EmployeeDelete` | Delete employee |
| GET | `api/Employee/EmployeeSearch?search=` | Search employees |
| GET | `api/Employee/GetByEmployeeId?employeeId=` | Get single employee |
| GET | `api/Employee/GetManager` | Get all managers |
| GET | `api/Employee/GetEmployeeListByManagerId?managerId=` | Get employees by manager |
| GET | `api/Employee/EmployeeBirthday` | Get employee birthdays |
| GET | `api/Employee/CheckLogonName?logonName=` | Check if logon name exists |
| POST | `api/Employee/ChangePassword` | Change password |

### Work Type Endpoints (`WorkTypeController`)

| Method | Route | Description |
|--------|-------|-------------|
| GET | `api/WorkTpe/GetAllWorkType` | Get all work types |
| GET | `api/WorkTpe/GetByWorkTypeId?workTypeId=` | Get single work type |
| POST | `api/WorkTpe/WorkTypeAdd` | Add work type |
| POST | `api/WorkTpe/WorkTypeUpdate` | Update work type |
| POST | `api/WorkTpe/WorkTypeDelete` | Delete work type |
| GET | `api/WorkTpe/WorkTypeSearch?search=` | Search work types |
| GET | `api/WorkTpe/GetAllWorkTypeCategory` | Get work type categories |

> **Note:** The route prefix is `api/WorkTpe` (typo in the original code — missing "y"). This is a known issue.

### Other Endpoints

| Method | Route | Controller | Description |
|--------|-------|-----------|-------------|
| GET | `api/Application/GetAllApplication` | ApplicationController | Get all applications |
| GET | `api/Application/GetByApplicationId?applicationId=` | ApplicationController | Get single application |
| POST | `api/Application/ApplicationAdd` | ApplicationController | Add application |
| POST | `api/Application/ApplicationUpdate` | ApplicationController | Update application |
| POST | `api/Application/ApplicationDelete` | ApplicationController | Delete application |
| GET | `api/Application/ApplicationSearch?search=` | ApplicationController | Search applications |
| GET | `api/Application/GetAllActiveProjects` | ApplicationController | Get active projects |
| GET | `api/Application/GetProjectItemByApplicationId?applicationId=` | ApplicationController | Get items by app |
| GET | `api/Team/GetTeam` | TeamController | Get all teams |
| GET | `api/TimeZone/GetTimeZone` | TimeZoneController | Get all time zones |
| GET | `api/Report/GetReportDetail` | ReportController | Get report definitions |
| GET | `api/Location/GetLocation` | LocationController | Get all locations |
| GET | `api/UserType/GetUserType` | UserTypeController | Get all user types |
| GET | `api/EmploymentType/GetEmploymentType` | EmploymentTypeController | Get employment types |

---

## Data Access Layer

### Architecture

All database operations go through stored procedures via Entity Framework's Database-First EDMX model.

```
API Controller
    → ITrackerDbRepository (interface)
        → EFTimeTrackerDbRepository (implementation)
            → EFContextFactory.TimeTrackerDBContext (EF context)
                → Stored Procedures (SQL Server)
                    → usp_*_Result classes (EF auto-generated)
                        → AutoMapper → Domain Model classes
```

### Key Files

| File | Purpose |
|------|---------|
| `TimeTrackerDb/EntityFramework/Core/ITrackerDbRepository.cs` | Repository interface defining all data operations |
| `TimeTrackerDb/EntityFramework/Implementation/EFTimeTrackerDbRepository.cs` | Implementation calling stored procedures through EF |
| `TimeTrackerDb/EntityFramework/EFContextFactory.cs` | Creates/provides the EF `DbContext` |
| `TimeTrackerDb/TimeTrackerDb.edmx` | Entity Framework Database-First model (auto-generated) |
| `Shared/Mapper/AutoMapperExtension.cs` | AutoMapper profile mapping `usp_*_Result` → domain models |

### Stored Procedure Naming Convention

Stored procedures follow the pattern `usp_{Entity}_{Operation}`:
- `usp_TimeEntry_Get` → retrieves time entries
- `usp_TimeEntry_Add` → inserts time entries
- `usp_Employee_GetAuthentication` → authenticates a user
- etc.

EF auto-generates result classes named `usp_{Entity}_{Operation}_Result` in the EDMX model.

### AutoMapper Configuration

`AutoMapperExtension` (`Shared/Mapper/AutoMapperExtension.cs`) defines all mappings between EF result classes and domain models. It exposes:
- `AutoMapperExtension.Mapper` — an `IMapper` instance (registered in Unity)
- A backward-compatible static `Mapper.Initialize()` call

Example mapping: `usp_TimeEntry_Get_Result` → `TimeEntryModel`

### Repository Lifetime

The repository (`EFTimeTrackerDbRepository`) is registered with **`HierarchicalLifetimeManager`** in Unity, meaning a new instance is created per HTTP request. This prevents EF context sharing across concurrent requests.

---

## Web Frontend Architecture

### MVC Controllers

All controllers inherit from `Controller` and receive dependencies via **constructor injection** (Unity).

| Controller | Views | Purpose |
|-----------|-------|---------|
| `AccountController` | Login, ChangePassword, AccessDenied | Authentication |
| `HomeController` | Index, About, Contact | Admin dashboard / landing |
| `TimeSheetController` | Index, Timesheet, AdminTimesheet, Add, WeeklyTimeSheet, _EditTimeSheet | Time entry (main feature) |
| `MonthlyViewController` | Index | Manager monthly summary |
| `ProjectViewController` | Index, GetProjectDetail, ExportProjectDetail | Project hours reporting |
| `EmployeeController` | Index, _AddEditEmployee | Employee CRUD |
| `ProjectController` | Index, _AddEditProject | Project CRUD |
| `ProjectItemController` | Index, _AddEditProjectItem | Project item CRUD |
| `WorkTypeController` | Index, _AddEditWorkType | Work type CRUD |
| `ApplicationController` | Index, _AddEditApplication | Application CRUD |
| `WorkItemController` | Index, _AddEditWorkItem | Work item CRUD |
| `ReportController` | Index | SSRS report viewer |

### View Structure

```
Views/
├── Account/          Login, ChangePassword, AccessDenied
├── Home/             Index (dashboard), About, Contact
├── TimeSheet/        Index, Timesheet, AdminTimesheet, Add, WeeklyTimeSheet, _EditTimeSheet
├── MonthlyView/      Index
├── ProjectView/      Index, GetProjectDetail, ExportProjectDetail
├── Employee/         Index, _AddEditEmployee
├── Project/          Index, _AddEditProject
├── ProjectItem/      Index, _AddEditProjectItem
├── WorkType/         Index, _AddEditWorkType
├── Application/      Index, _AddEditApplication
├── WorkItem/         Index, _AddEditWorkItem
├── ALM/              Index, _AddEditALM
├── Report/           Index
├── Sweet/            Alert (SweetAlert partial)
├── Shared/
│   ├── _Layout.cshtml              Main layout with sidebar navigation
│   ├── _LayoutLogin.cshtml         Login page layout (no sidebar)
│   ├── _AjaxLoading.cshtml         Loading spinner partial
│   ├── _Notifications.cshtml       Notification partial
│   ├── _ValidationScriptsPartial.cshtml
│   ├── Error.cshtml
│   └── EditorTemplates/
│       └── _ProjectItemStatus.cshtml
├── _ViewImports.cshtml
└── _ViewStart.cshtml
```

### Layout and Navigation

**`_Layout.cshtml`** provides the main application shell with:
- **Sidebar navigation** — role-based menu items:
  - All users: Timesheet, Weekly Status
  - Admin users (SAN/ADN): Dashboard, Employee, Project, Project Items, Work Types, Applications, Work Items, Monthly View, Project View, Reports
- **Top bar** with user info and logout
- **Bundle references** for CSS (Bootstrap, Kendo) and JS (jQuery, Kendo, custom scripts)

**`_LayoutLogin.cshtml`** is a minimal layout used only for the login page (no sidebar or navigation).

### JavaScript Files

| File | Purpose |
|------|---------|
| `Scripts/application-base.js` | Core application helpers, common UI initialization (203 lines) |
| `Scripts/jquery.timecard.js` | Time entry grid logic for regular users |
| `Scripts/jquery.admin.timecard.js` | Time entry grid logic for admin users |
| `Scripts/jquery.BA.timecard.js` | Time entry grid logic for BA team |
| `Scripts/jquery.table2excel.min.js` | Client-side Excel export utility |
| `Scripts/sweetalert2.all.min.js` | SweetAlert2 for notification dialogs |

### Client-Side Libraries and Bundles

Bundles are configured in `App_Start/BundleConfig.cs`:

| Bundle | Contents |
|--------|----------|
| `~/bundles/jquery` | jQuery 1.12.4 |
| `~/bundles/jqueryval` | jQuery Validate |
| `~/bundles/bootstrap` | Bootstrap JS, jQuery Validate, jQuery UI |
| `~/bundles/modernizr` | Modernizr |
| `~/bundles/js/kendo` | Kendo UI 2015.3.930 (all, timezones, ASP.NET MVC) |
| `~/bundles/css/kendo` | Kendo Bootstrap theme CSS |
| `~/Content/css` | Bootstrap, MetisMenu, SB Admin 2, Font Awesome |

### Kendo UI Grid Pattern

Most CRUD views follow the same pattern:
1. A Kendo Grid bound to a `DataSource` that reads from a controller action returning JSON
2. Grid toolbar with "Add" button opening a Kendo Window with a partial view (`_AddEdit*.cshtml`)
3. Edit/Delete buttons in each grid row
4. The partial view contains a form that POSTs to the corresponding controller action
5. On success, the grid is refreshed

---

## Dependency Injection

Both the API and web frontend use **Unity** for dependency injection.

### API-Side DI (`Services/.../App_Start/UnityConfig.cs`)

```
ITrackerDbRepository  → EFTimeTrackerDbRepository  (HierarchicalLifetimeManager — per-request)
IMapper               → AutoMapperExtension.Mapper  (singleton instance)
IValidatorFactory     → UnityValidatorFactory       (singleton)
+ All FluentValidation validators auto-discovered from SBS.* assemblies
```

Resolver: `GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container)`

### Web-Side DI (`Web/.../App_Start/UnityConfig.cs`)

```
IAPIExtension         → APIExtension               (singleton — uses static HttpClient internally)
IAPIConfiguration     → APIConfiguration            (singleton)
ISessionCacheManager  → SessionCacheManager         (singleton)
ILogger               → Log4NetLogger               (singleton)
```

Resolver: `DependencyResolver.SetResolver(new UnityDependencyResolver(container))`

### Controller Constructor Injection

All MVC controllers receive their dependencies through constructors:

```csharp
public class TimeSheetController : Controller
{
    private readonly IAPIExtension _apiExtension;
    private readonly IAPIConfiguration _apiConfiguration;
    private readonly ISessionCacheManager _sessionCacheManager;
    private readonly ILogger _logger;

    public TimeSheetController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration,
        ISessionCacheManager sessionCacheManager, ILogger logger)
    {
        _apiExtension = apiExtension;
        // ...
    }
}
```

---

## Cross-Cutting Concerns

### Request Tracing (Correlation IDs)

Every API call from the web frontend includes an `X-Correlation-ID` header (GUID). This ID is:
- Generated per-request in `APIExtension.InvokeGet/InvokePost`
- Logged on errors in both `APIExtension` (client-side) and `GlobalExceptionFilterAttribute` (server-side)
- Enables tracing a single user action across both tiers in log files

### API Pipeline Filters

Configured in `WebApiConfig.Register()`:

1. **`ApiKeyAuthHandler`** (DelegatingHandler) — validates `X-Api-Key` header before the request reaches any controller
2. **`ValidateModelAttribute`** (ActionFilterAttribute) — returns `400 Bad Request` with model state errors if `ModelState.IsValid` is false
3. **`GlobalExceptionFilterAttribute`** (ExceptionFilterAttribute) — catches unhandled exceptions, logs them with correlation ID, returns `500 Internal Server Error`

### Session Timeout

`SessionTimeoutAttribute` is applied to controllers via `[SessionTimeout]`. It checks `AuthenticateExtension.UserType` before each action and redirects to `/Account/Login` if the session has expired.

### Reference Data Caching

`ReferenceDataCache` (`Web/.../Extension/ReferenceDataCache.cs`) is a static cache that stores lookup data in `MemoryCache` with a **5-minute TTL**. This prevents re-fetching reference data (work types, projects, teams, etc.) on every page load.

Cached data types: WorkTypes, Projects, Teams, Managers, TimeZones, Locations, UserTypes, EmploymentTypes, ProjectTypes, WorkTypeCategories, ProjectItemStatuses, Applications.

Usage in controllers:
```csharp
var workTypes = ReferenceDataCache.GetWorkTypes(_apiExtension, _apiConfiguration);
```

### Logging

Log4Net is wrapped in `ILogger` / `Log4NetLogger`. Configured in `Web.config` / `log4net.config`. The logger is injected via Unity into controllers and `APIExtension`.

### Validation

FluentValidation validators are auto-discovered from all `SBS.*` assemblies and registered in the Unity container. The API-side `ValidateModelAttribute` enforces model validation globally.

---

## Configuration Reference

### Web Frontend `Web.config` App Settings

| Key | Purpose |
|-----|---------|
| `ServiceAddress` | Base URL of the API (e.g., `http://localhost:PORT/`) |
| `ApiKey` | API key sent in `X-Api-Key` header (empty = no auth) |

### API `Web.config` App Settings

| Key | Purpose |
|-----|---------|
| `ApiKey` | Expected API key value (empty = auth bypassed) |

### Connection Strings

The API's `Web.config` contains the EF connection string for the SQL Server database, referenced by the EDMX model.

---

## Key Features

### Time Entry (Timesheet)

- **Weekly Entry**: Time is entered on a week-by-week basis. The week starts on **Sunday**. The UI defaults to the current week.
- **Team-Based Default Work Type**: When adding time, the system pre-selects a work type based on the employee's team code:

  | Team Code | Default Work Type |
  |-----------|------------------|
  | DEV | Development (DEV) |
  | QA | Testing (TES) |
  | PS | Production Support (PST) |
  | RPT | Reporting (RPT) |
  | BA | Project Management (PMT) |
  | MGT | Administration (ADN) |

- **Pending Entry Detection**: The system tracks the last time entry date and alerts users if they have pending (unfilled) weeks.
- **Distinct Records**: Employees can retrieve their previous distinct time entry records to quickly re-enter similar work.
- **Admin Timesheet**: Admin users get a separate timesheet view (`AdminTimesheet.cshtml`) with additional management capabilities.
- **Batch Submission**: Time entries are submitted as a list via `TimeEntryRequestModel`, allowing multiple entries per save.

### Weekly Status View

Displays a weekly summary per employee showing week start/end dates, total hours per week, and a weekly status indicator.

### Monthly View (`MonthlyViewController`)

A manager-level view showing all employees' weekly hours for a selected month. Features:
- Filter by manager
- Select month and year
- View weekly hour breakdowns per employee across Sunday-based weeks
- Export to Excel
- Uses `Parallel.ForEach` for concurrent API calls when loading employee data

### Project View (`ProjectViewController`)

An admin view showing hours by project, broken down by location and work type. Filterable by project, month, and year. Includes an export view with per-employee detail.

### Reports (`ReportController`)

Integrates with SQL Server Reporting Services (SSRS). Reports are registered in the database via `ReportRegistryDetail` and rendered using `Microsoft.Reporting.WebForms.ReportViewer`. Reports are filtered by user type so different roles see different reports.

### CRUD Admin Pages

Employee, Project, Project Item, Work Type, Application, and Work Item entities each have admin pages with:
- Kendo UI Grid with search/filter
- Add/Edit dialog (partial view in Kendo Window)
- Delete confirmation
- Server-side validation via FluentValidation

---

## Known Issues and Technical Debt

### Route Typo
The WorkType API controller uses route prefix `api/WorkTpe` (missing "y"). This is baked into both the controller attribute and the `APIResources` constants. Fixing requires coordinated changes in both projects.

### Model Duplication
There are **11+ model classes duplicated** across the data access and web layers. For example, `TimeEntryModel` exists in both `TimeTrackerDb/Model/TrackerModel.cs` and `TimeTrackerWeb/Models/`. These are not shared — they are separate classes with the same properties. Changes must be made in both places.

Duplicated models include: `TimeEntryModel`, `ProjectModel`, `ProjectItemModel`, `EmployeeModel`, `WorkTypeModel`, `ProjectListModel`, `ProjectItemListModel`, `TimeEntryWeeklyStatusModel`, `AdminProjectViewModel`, `AdminProjectViewExportModel`, `WorkTypeCategoryModel`, `ProjectTypeModel`, `ProjectItemStatusModel`.

### Dead File
`TimeTrackerWeb/Models/TimeSheetModeoldl.cs` — a file with a typo in its name. Contains what appears to be an unused or outdated model class.

### God Controller
`TimeSheetController` is ~1,076 lines with many responsibilities. It handles time entry CRUD, weekly status, distinct records, admin timesheet, and various helper methods. A candidate for decomposition.

### Scaffold Artifact
`ValuesController` exists in the API project — this is a default Web API scaffolding artifact that should be removed.

### Static Caching Limitation
`ReferenceDataCache` is a static in-memory cache. In a multi-server deployment, cache invalidation is not coordinated across instances. Each server maintains its own 5-minute TTL cache independently.

### Legacy Entity
The `Application` entity is largely commented out in time entry flows but still has full CRUD pages and API endpoints.

---

## Development Workflow

### Adding a New API Endpoint

1. Add the stored procedure to SQL Server
2. Update the EDMX model: open `TimeTrackerDb.edmx` → right-click → "Update Model from Database" → select the new stored procedure
3. Add the mapping in `AutoMapperExtension.cs` for the new `usp_*_Result` → domain model
4. Add the repository method to `ITrackerDbRepository` and `EFTimeTrackerDbRepository`
5. Add the controller action in the appropriate API controller with `[Route("api/...")]` attribute
6. Add the endpoint constant to `APIResources` in `CommonTypeMessages.cs`

### Adding a New MVC Page

1. Create the controller in `TimeTrackerWeb/Controllers/` with constructor injection
2. Add the `[SessionTimeout]` attribute to the controller or individual actions
3. Create views in `Views/{ControllerName}/`
4. For CRUD pages: create `Index.cshtml` (grid) and `_AddEdit{Entity}.cshtml` (dialog)
5. Add navigation link in `Views/Shared/_Layout.cshtml` (check role-based visibility)
6. Call API via `_apiExtension.InvokeGet<T>()` using URLs from `APIResources`

### Adding a New Lookup/Reference Type

1. Create the stored procedure and add to EDMX
2. Add model class to **both** `TimeTrackerDb/Model/TrackerModel.cs` and `TimeTrackerWeb/Models/`
3. Add AutoMapper mapping in `AutoMapperExtension.cs`
4. Add repository method, API controller action, and `APIResources` constant
5. Optionally add to `ReferenceDataCache` for client-side caching

### Modifying an Existing Stored Procedure

1. Modify the stored procedure in SQL Server
2. If the result shape changed: delete and re-import the function import in EDMX designer
3. Update the `usp_*_Result` → model mapping in `AutoMapperExtension.cs` if fields changed
4. Update model classes in **both** data access and web projects if fields changed

### Common Gotchas

- **Model duplication**: Always update models in both `TimeTrackerDb` and `TimeTrackerWeb` projects
- **APIResources constants**: The web frontend assembles API URLs from `IAPIConfiguration.ServiceBaseAddress` + `APIResources.*` — don't hardcode URLs in controllers
- **EDMX updates**: After updating the EDMX, check that the auto-generated `usp_*_Result` classes match your expectations. EF can sometimes lose mappings
- **Route prefix typo**: WorkType routes use `api/WorkTpe` — maintain this typo for backward compatibility or fix it in both projects simultaneously
- **Session data**: Controllers access user info via `SessionCacheManager` — this returns `null` if the session has expired, which is caught by `SessionTimeoutAttribute`
- **Static HttpClient**: `APIExtension` uses a shared static `HttpClient`. Do not create new `HttpClient` instances elsewhere
- **Cache invalidation**: After CRUD operations on reference data (work types, projects, etc.), the `ReferenceDataCache` will serve stale data for up to 5 minutes
