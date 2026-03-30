# TimeTracker - Actionable Fix Roster

Items from the blind spots analysis that are fully within this application's control, requiring no stored procedure changes, database schema modifications, or infrastructure team involvement.

Each item is tagged with an effort estimate (S/M/L) and severity (Critical/High/Medium).

---

## Completed

The following quick wins have been implemented:

| # | Fix | Commit |
|---|-----|--------|
| ~~1~~ | Silent exception swallowing in `TimeEntryAdd`/`TimeEntryUpdate` - now logs and rethrows | `820b689` |
| ~~3~~ | Password stored in Forms Auth cookie - now stores only the username; session expiry forces re-login | `820b689` |
| ~~4~~ | Delete operations used HTTP GET - changed to `[HttpPost]` on API and updated web callers | `820b689` |
| ~~10~~ | Mixed HTTP client implementations - replaced raw `HttpWebRequest` calls with `APIExtension` | `820b689` |
| ~~13~~ | URL construction without encoding - added `Uri.EscapeDataString()` to all user-input parameters | `820b689` |
| ~~17~~ | Validation re-fetched all reference data - lists now passed from caller, removed unused `projectItemList` fetch | `820b689` |
| ~~21~~ | ChangePassword non-functional - uncommented the `usp_Employee_UpdatePassword` stored procedure call | `820b689` |
| ~~22~~ | Non-production auth bypass risk - added WARN-level logging when bypass is triggered | `820b689` |

---

## Remaining - Critical

### 2. API Errors Silently Return Default Values
**Effort:** M | **File:** `APIExtension.cs` (lines 73-87)

`InvokeGet<T>()` and `InvokePost<T>()` catch `WebException`, discard the error, then deserialize an empty string. Callers cannot distinguish "no data found" from "server error." Fix: log the error and either throw or return a result wrapper that carries error state.

---

## Remaining - High

### 5. No Caching of Reference Data
**Effort:** M | **Files:** All web controllers, `MemoryDataCacheManager.cs`

Work types, teams, locations, time zones, employment types, user types, and project lists are re-fetched from the API on nearly every page load. `MemoryDataCacheManager` already exists but is unused. Fix: wrap lookup calls with `MemoryDataCacheManager` using a reasonable TTL (e.g., 5-10 minutes).

### 6. New HttpWebRequest Per API Call (Socket Exhaustion)
**Effort:** M | **File:** `APIExtension.cs`

Every call to `InvokeGet` or `InvokePost` creates a new `HttpWebRequest`. Under load this causes port exhaustion. Fix: replace with a shared static `HttpClient` instance (which is designed to be reused and handles connection pooling internally).

### 7. New Object Instantiation in Every Controller Constructor
**Effort:** M | **Files:** All web controllers, need to add Unity or similar DI

Every web controller `new`s up `APIExtension`, `APIConfiguration`, `SessionCacheManager`, and `Log4NetLogger`. `APIConfiguration` re-reads `AppSettings` each time. Fix: register these as singletons in a DI container (Unity is already used on the API side) and inject via constructor parameters.

### 8. Monthly View N+1 Loop
**Effort:** M | **File:** `MonthlyViewController.cs` (lines 82-143)

The employee loop fires a separate API call per employee. Even without a new bulk stored procedure, the app can: (a) issue the calls in parallel using `Task.WhenAll`, (b) cache the weekly status data per request, or (c) create a composite API endpoint that accepts a list of employee IDs and calls the existing sproc in a single DB connection.

### 9. Employee Edit Form: 7 Sequential API Calls
**Effort:** M | **File:** `EmployeeController.cs` (lines 116-126, 202-211)

`AddEmployee()` and `EditEmployee()` make 6-7 sequential API calls for lookup data. Fix: cache lookup data (see item 5), or issue the calls in parallel, or create a single composite API endpoint that returns all lookups in one response.

### 11. No Input Validation on API Layer
**Effort:** M | **Files:** All API controllers

FluentValidation is registered in `UnityConfig` but no controller actions use `[Validate]` attributes or check `ModelState`. Any direct API caller can submit invalid data. Fix: add `ModelState.IsValid` checks to POST actions, or add a global validation action filter.

### 12. No API Authentication
**Effort:** L | **Files:** API `Web.config`, all API controllers

The API has zero authentication. Fix: add token-based or API key authentication middleware. Even a shared secret validated via a message handler would be a significant improvement over the current open-access state.

---

## Remaining - Medium

### 14. Duplicated Model Definitions with Type Mismatches
**Effort:** L | **Files:** `Web/.../Models/` vs `Services/.../Model/TrackerModel.cs`

Models are defined independently in both tiers. The web-side `TimeEntry.WorkHour` is `double` while the API-side is `decimal`, which can cause rounding issues. Fix: extract shared models into a common NuGet package or shared project, or at minimum align the types.

### 15. No Dependency Injection in Web Frontend
**Effort:** M | **Files:** All web controllers, need new `UnityConfig` for web project

All dependencies are `new`-ed in constructors. This blocks unit testing and forces duplicate instances. Fix: add Unity (or another DI container) to the web project and register `APIExtension`, `APIConfiguration`, `SessionCacheManager`, and `ILogger` as shared instances. Note: this is closely related to item 7 - solving one effectively solves both.

### 18. Static AutoMapper Configuration (Legacy API)
**Effort:** M | **File:** `AutoMapperExtension.cs`

Uses the deprecated static `Mapper.Initialize()` API which pins AutoMapper to pre-v9 versions. Fix: migrate to instance-based `MapperConfiguration` and inject `IMapper`. Can be done incrementally.

### 19. Inconsistent Error Logging
**Effort:** M | **Files:** All controllers and repository methods

Some methods log and rethrow, some log and swallow, some don't log at all. No correlation IDs across the web-to-API boundary. Fix: establish a consistent pattern (e.g., log at boundaries, let exceptions propagate, add a global exception filter for unhandled cases). Add a correlation header passed from web to API.

### 20. New EF Context Per Repository Call
**Effort:** M | **File:** `EFTimeTrackerDbRepository.cs`, `EFContextFactory.cs`

Every repository method creates a new context. Fix: manage context lifetime per-request using Unity's `PerResolveLifetimeManager` or `HierarchicalLifetimeManager`, so a single API request shares one DB connection.
