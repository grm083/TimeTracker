# TimeTracker - Actionable Fix Roster

Items from the blind spots analysis that are fully within this application's control, requiring no stored procedure changes, database schema modifications, or infrastructure team involvement.

Each item is tagged with an effort estimate (S/M/L) and severity (Critical/High/Medium).

---

## Critical

### 1. Silent Exception Swallowing in TimeEntry Save/Update
**Effort:** S | **File:** `EFTimeTrackerDbRepository.cs` (lines 260, 301)

`TimeEntryAdd()` and `TimeEntryUpdate()` have empty `catch (Exception ex) { }` blocks inside `TransactionScope`. Database errors are silently discarded and the user's data is lost without any feedback. Fix: log the exception and rethrow (or return an error result the controller can surface).

### 2. API Errors Silently Return Default Values
**Effort:** M | **File:** `APIExtension.cs` (lines 73-87)

`InvokeGet<T>()` and `InvokePost<T>()` catch `WebException`, discard the error, then deserialize an empty string. Callers cannot distinguish "no data found" from "server error." Fix: log the error and either throw or return a result wrapper that carries error state.

### 3. Password Stored in Forms Auth Cookie
**Effort:** S | **File:** `AccountController.cs` (line 53)

The full `LoginModel` including plaintext password is serialized into the Forms Auth ticket `UserData`. Fix: store only the username (or a session token) in the ticket, not the password. The `GetAuthenticatedUserFromTicket` method will need to be updated to re-authenticate by session rather than replaying credentials.

### 4. Delete Operations Use HTTP GET
**Effort:** S | **File:** API `EmployeeController.cs`, `ProjectController.cs`

`EmployeeDelete` and `ProjectDelete` on the API side are `[HttpGet]` actions. Browser pre-fetch, link previews, or crawlers can trigger deletions. Fix: change to `[HttpPost]` or `[HttpDelete]` and update the web-side callers to use POST.

---

## High

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

### 10. Mixed HTTP Client Implementations
**Effort:** S | **File:** `TimeSheetController.cs` (lines 550-643)

`GetAllApplication()`, `GetAllProjectItem()`, and `GetAllWorkType()` bypass `APIExtension` and create raw `HttpWebRequest` objects directly. Fix: replace with calls through `APIExtension` (or its successor) so logging and error handling are consistent.

### 11. No Input Validation on API Layer
**Effort:** M | **Files:** All API controllers

FluentValidation is registered in `UnityConfig` but no controller actions use `[Validate]` attributes or check `ModelState`. Any direct API caller can submit invalid data. Fix: add `ModelState.IsValid` checks to POST actions, or add a global validation action filter.

### 12. No API Authentication
**Effort:** L | **Files:** API `Web.config`, all API controllers

The API has zero authentication. Fix: add token-based or API key authentication middleware. Even a shared secret validated via a message handler would be a significant improvement over the current open-access state.

---

## Medium

### 13. URL Construction via String Concatenation (No Encoding)
**Effort:** S | **Files:** All web controllers

Query parameters are concatenated without `Uri.EscapeDataString()`. Special characters in search text or comments break requests. Fix: encode all parameter values, or switch to a URI builder / RestSharp parameter binding.

### 14. Duplicated Model Definitions with Type Mismatches
**Effort:** L | **Files:** `Web/.../Models/` vs `Services/.../Model/TrackerModel.cs`

Models are defined independently in both tiers. The web-side `TimeEntry.WorkHour` is `double` while the API-side is `decimal`, which can cause rounding issues. Fix: extract shared models into a common NuGet package or shared project, or at minimum align the types.

### 15. No Dependency Injection in Web Frontend
**Effort:** M | **Files:** All web controllers, need new `UnityConfig` for web project

All dependencies are `new`-ed in constructors. This blocks unit testing and forces duplicate instances. Fix: add Unity (or another DI container) to the web project and register `APIExtension`, `APIConfiguration`, `SessionCacheManager`, and `ILogger` as shared instances.

### 16. Authentication Ticket Re-validates Against AD on Session Expiry
**Effort:** M | **File:** `AuthenticateExtension.cs` (lines 112-128)

When the session cache is empty but the Forms Auth cookie is still valid, `GetAuthenticatedUserFromTicket` replays the stored credentials against AD. This is slow and fragile. Fix: store only non-sensitive user profile data in the ticket (after removing the password per item 3), and redirect to login on session expiry rather than silently re-authenticating.

### 17. Validation Re-fetches All Reference Data
**Effort:** S | **File:** `TimeSheetController.cs` (line 681+)

`ValidateTimeEntries()` calls `getWorkTypeList()`, `getallProjectList()`, and `getProjectItemList()` on every save. Fix: once reference data is cached (item 5), this becomes a non-issue. Alternatively, validate against IDs only and let the database enforce referential integrity.

### 18. Static AutoMapper Configuration (Legacy API)
**Effort:** M | **File:** `AutoMapperExtension.cs`

Uses the deprecated static `Mapper.Initialize()` API which pins AutoMapper to pre-v9 versions. Fix: migrate to instance-based `MapperConfiguration` and inject `IMapper`. Can be done incrementally.

### 19. Inconsistent Error Logging
**Effort:** M | **Files:** All controllers and repository methods

Some methods log and rethrow, some log and swallow, some don't log at all. No correlation IDs across the web-to-API boundary. Fix: establish a consistent pattern (e.g., log at boundaries, let exceptions propagate, add a global exception filter for unhandled cases). Add a correlation header passed from web to API.

### 20. New EF Context Per Repository Call
**Effort:** M | **File:** `EFTimeTrackerDbRepository.cs`, `EFContextFactory.cs`

Every repository method creates a new context. Fix: manage context lifetime per-request using Unity's `PerResolveLifetimeManager` or `HierarchicalLifetimeManager`, so a single API request shares one DB connection.

### 21. ChangePassword Is Non-Functional
**Effort:** S | **File:** `EFTimeTrackerDbRepository.cs` (lines 436-445)

The stored procedure call is commented out. The UI presents the feature but it always fails. Fix: either uncomment the sproc call (if the procedure exists and works) or remove the ChangePassword UI to avoid user confusion.

### 22. Non-Production Auth Bypass Risk
**Effort:** S | **File:** `EFTimeTrackerDbRepository.cs` (line 372)

When `IsProduction=false`, any valid AD username is accepted without a password. Fix: add a safeguard so this flag cannot be accidentally left off in deployed environments (e.g., fail-closed by requiring an explicit override, or log a warning at startup).
