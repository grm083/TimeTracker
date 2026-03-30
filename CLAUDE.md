# TimeTracker Application

## Overview

TimeTracker is an internal billable-hours tracking application used to record employee time entries against projects. It is built as a two-tier .NET application: an ASP.NET Web API backend and an ASP.NET MVC web frontend. The backend uses Entity Framework with SQL Server stored procedures for all data access.

## Architecture

The solution is split into two Visual Studio solutions under `Main/`:

- **`SBS.IT.Utilities.TimeTrackerAPI.sln`** - The Web API backend
- **`SBS.IT.Utilities.TimeTrackerWeb.sln`** - The MVC web frontend

### Projects

| Project | Path | Purpose |
|---------|------|---------|
| **TimeTrackerWebAPI** | `Services/Source/SBS.IT.Utilities.API.TimeTrackerWebAPI/` | ASP.NET Web API 2 (.NET 4.6.1) - REST API layer |
| **TimeTrackerDb** | `Services/Source/SBS.IT.Utilities.DataAccess/SBS.IT.Utilities.DataAccess.TimeTrackerDb/` | Entity Framework data access layer using stored procedures |
| **TimeTrackerWeb** | `Web/Source/SBS.IT.Utilities.Web.TimeTrackerWeb/` | ASP.NET MVC frontend with Kendo UI grids |
| **Shared.APIClient** | `Web/Source/SBS.IT.Utilities.Shared.APIClient/` | HTTP client library the web frontend uses to call the API |
| **Shared** | `Shared/Source/SBS.IT.Utilities.Shared/` | Shared utilities: AutoMapper config, FluentValidation, caching, email, base models |
| **Logger** | `Shared/Source/SBS.IT.Utilities.Logger/` | Log4Net logging wrapper |

### Request Flow

```
Browser -> MVC Web (TimeTrackerWeb) -> REST API Client (APIExtension/RestSharp)
       -> Web API (TimeTrackerWebAPI) -> EF Repository (stored procedures) -> SQL Server
```

The web frontend never accesses the database directly. All data flows through the API via `APIExtension.InvokeGet<T>()` and `APIExtension.InvokePost<T>()` calls using RestSharp. API endpoint constants are defined in `Shared.APIClient/Message/CommonTypeMessages.cs` (`APIResources` class).

## Core Domain Entities

### Time Entry
The central entity. Each time entry records:
- **EmployeeId** - who logged the time
- **ProjectId** / **ProjectItemId** - which project and sub-item the work is for
- **WorkTypeId** - the type of work performed (Development, Testing, BA, etc.)
- **Date** - the date of work
- **WorkHour** - decimal hours worked
- **Comments** - description of work done
- **WorkItem** - ALM/INC reference number

Time entries are submitted in batches via `TimeEntryRequestModel` (a list of `TimeEntryModel` objects).

### Project
Top-level billing entity with a name, description, type (`ProjectTypeId`), and active status.

### Project Item
A sub-unit of a project. Tracks:
- Budget, Benefit amounts
- Level of Effort estimates: DevLOE, QaLOE, BaLOE
- Status (`ProjectItemStatusId`)

### Employee
Represents a user of the system with:
- Personal info (name, email, phone, DOB, DOJ, designation)
- **LocationId** - office location
- **EmploymentTypeId** - employment classification
- **UserTypeId** - role/permissions (see User Types below)
- **TeamId** / **TeamCode** - team assignment (DEV, QA, BA, PS, RPT, MGT)
- **ManagerId** - reporting manager
- **IsTimeEntryEnable** - flag to allow/deny time entry access
- **TimeZoneId** - employee's time zone

### Work Type
Categorizes the kind of work being tracked. Each work type has a code (DEV, TES, PST, RPT, PMT, ADN) and belongs to a `WorkTypeCategory`. The `IsCapitalizable` flag indicates whether the work qualifies as capitalizable.

### Supporting Entities
- **Team** - organizational teams (DEV, QA, BA, PS, RPT, MGT)
- **Location** - office locations
- **TimeZone** - time zone reference data
- **EmploymentType** - employment classifications
- **Application** - legacy entity (largely commented out in time entries)
- **ReportRegistryDetail** - SSRS report definitions with server URLs and paths

## Authentication and Authorization

- **Authentication**: Forms Authentication. Users log in with username/password via `AccountController.Login()`. Credentials are validated by calling the API endpoint `api/Employee/GetAuthentication`, which authenticates against Active Directory (via `System.DirectoryServices.AccountManagement`) in production or against the database in non-production.
- **Session Management**: Authenticated user data is stored in session via `SessionCacheManager`. A `SessionTimeoutAttribute` action filter redirects to login if the session has expired.
- **FormsAuthentication Ticket**: Login credentials are serialized into the Forms Auth cookie so sessions can be re-validated on ticket rehydration.

### User Types

| Code | Role | Behavior |
|------|------|----------|
| **SAN** | Super Admin | Redirected to admin dashboard; can view all employees' time entries |
| **ADN** | Admin | Redirected to admin dashboard; can manage employees under them |
| *(other)* | Regular User | Redirected directly to the timesheet entry page |

Admins (ADN/SAN) see a dashboard with counts of users, projects, project items, and reports. Regular users see their last time entry date and go straight to time entry.

## Key Features

### Time Entry (Timesheet)
- **Weekly Entry**: Time is entered on a week-by-week basis. The week starts on Sunday. The UI defaults to the current week.
- **Team-Based Default Work Type**: When adding time, the system pre-selects a work type based on the employee's team code:
  - DEV -> Development (DEV)
  - QA -> Testing (TES)
  - PS -> Production Support (PST)
  - RPT -> Reporting (RPT)
  - BA -> Project Management (PMT)
  - MGT -> Administration (ADN)
- **Pending Entry Detection**: The system tracks the last time entry date and alerts users if they have pending (unfilled) weeks.
- **Distinct Records**: Employees can retrieve their previous distinct time entry records to quickly re-enter similar work.
- **Admin Timesheet**: Admin users get a separate timesheet view that allows them to manage entries with additional capabilities.

### Weekly Status View
Displays a weekly summary per employee showing:
- Week start/end dates
- Total hours per week
- Weekly status indicator

### Monthly View (`MonthlyViewController`)
A manager-level view that shows all employees' weekly hours for a selected month. Managers can:
- Filter by manager
- Select month and year
- View weekly hour breakdowns per employee across Sunday-based weeks
- Export to Excel

### Project View (`ProjectViewController`)
An admin view that shows hours by project, broken down by:
- Location (employee location code)
- Work type
- Filterable by project, month, and year
- Includes an export view with per-employee detail

### Reports (`ReportController`)
Integrates with SQL Server Reporting Services (SSRS). Reports are registered in the database via `ReportRegistryDetail` and rendered using `Microsoft.Reporting.WebForms.ReportViewer`. Reports are filtered by user type so different roles see different reports.

## API Endpoints

All API routes are prefixed with `api/` and use attribute routing. Key endpoint groups:

| Prefix | Controller | Operations |
|--------|-----------|------------|
| `api/TimeEntry/` | TimeEntryController | GetTimeEntry, TimeEntryAdd, TimeEntryUpdate, TimeEntryDelete, TimeEntrySearch, GetTimeEntryWeeklyStatus, GetLastTimeEntry, GetTimeEntryDistinctRecords, GetAdminProjectView, GetAdminProjectViewExport |
| `api/Project/` | ProjectController, ProjectItemController | GetProject, ProjectAdd/Update/Delete, ProjectSearch, GetProjectItem, ProjectItemAdd/Update/Delete, ProjectItemSearch, GetProjectType, GetProjectItemStatus |
| `api/Employee/` | EmployeeController | GetAuthentication, EmployeeAdd/Update/Delete, EmployeeSearch, GetManager, GetEmployeeList, CheckLogonName, ChangePassword |
| `api/Application/` | ApplicationController | GetApplication, ApplicationAdd/Update/Delete, ApplicationSearch |
| `api/WorkTpe/` | WorkTypeController | GetWorkType, WorkTypeAdd/Update/Delete, WorkTypeSearch, GetWorkTypeCategory |
| `api/Team/` | TeamController | GetTeam |
| `api/TimeZone/` | TimeZoneController | GetTimeZone |
| `api/Report/` | ReportController | GetReportDetail |
| `api/Location/` | LocationController | GetLocation |
| `api/UserType/` | UserTypeController | GetUserType |
| `api/EmploymentType/` | EmploymentTypeController | GetEmploymentType |

## Data Access Pattern

All database operations go through stored procedures via Entity Framework's EDMX model:
- Stored procedure results are mapped to `usp_*_Result` classes (auto-generated by EF)
- AutoMapper maps these result classes to domain model classes (e.g., `usp_TimeEntry_Get_Result` -> `TimeEntryModel`)
- The repository (`EFTimeTrackerDbRepository`) implements `ITrackerDbRepository`
- Dependency injection is handled via Unity (`UnityConfig.cs`)

## Technology Stack

- **Backend API**: ASP.NET Web API 2 on .NET Framework 4.6.1
- **Frontend**: ASP.NET MVC with Razor views
- **UI Components**: Telerik Kendo UI for ASP.NET MVC (grids, data sources)
- **Database**: SQL Server (all logic in stored procedures)
- **ORM**: Entity Framework 6 (Database-First with EDMX)
- **Object Mapping**: AutoMapper
- **Validation**: FluentValidation
- **HTTP Client**: RestSharp (Signed)
- **Logging**: Log4Net
- **DI Container**: Unity
- **Reports**: SQL Server Reporting Services (SSRS) via ReportViewer
- **Authentication**: ASP.NET Forms Authentication + Active Directory
