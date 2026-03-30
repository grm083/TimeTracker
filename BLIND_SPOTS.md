# TimeTracker - Blind Spots and Performance Issues

A roster of architectural, performance, reliability, and security concerns identified through code review.

---

## Performance

### 1. Monthly View N+1 Query Problem (Critical)

**Location:** `MonthlyViewController.ReadEmployeeDetailsGrid()` (lines 82-143)

The monthly view fetches all employees, then loops through each one and makes a separate API call (`getTimeSheetData`) per employee. For 100 employees, this fires 100+ sequential HTTP requests from the web server to the API, each of which opens a new DB connection and executes a stored procedure.

```
foreach (var emp in details)        // N employees
{
    var empData = getTimeSheetData(emp.EmployeeId, startDate);  // 1 API call each
    ...
}
```

This should be a single bulk query that retrieves weekly totals for all employees in one round-trip.

### 2. No Caching of Reference Data

**Location:** Every controller constructor, every page load

Work types, teams, locations, time zones, employment types, user types, and project lists are fetched from the API on nearly every page load. These are small, rarely-changing lookup tables. `MemoryDataCacheManager` exists in the codebase but is never used by any controller. All controllers only use `SessionCacheManager`, meaning each user session re-fetches all reference data independently.

### 3. New EF Context Per Repository Call

**Location:** `EFContextFactory.TimeTrackerDBContext` (getter), every method in `EFTimeTrackerDbRepository`

Every single repository method creates a new `EFContextFactory` and a new `TimeTrackerEntities` context. There is no context pooling or per-request lifetime. A single page load that triggers multiple repository calls (e.g., the Employee edit form calls 6+ lookup endpoints) opens and tears down 6+ separate DB connections.

### 4. New RestSharp/HttpWebRequest Client Per Call

**Location:** `APIExtension.InvokeGet<T>()` and `APIExtension.InvokePost<T>()`

Every API call from the web frontend creates a new `HttpWebRequest` from scratch. There is no `HttpClient` reuse, no connection pooling, and no keep-alive optimization. Under load, this exhausts available sockets and causes port exhaustion (`TIME_WAIT` buildup). The `InvokeServiceWithBasicAuth` method also creates a new `RestClient` per call.

### 5. New Object Instantiation in Every Controller Constructor

**Location:** All web frontend controllers (TimeSheetController, EmployeeController, etc.)

Every controller `new`s up its own `APIExtension`, `APIConfiguration`, `SessionCacheManager`, and `Log4NetLogger` on every HTTP request. The web frontend has no dependency injection container, so these objects are never shared or pooled. `APIConfiguration` reads `ConfigurationManager.AppSettings` on every instantiation.

### 6. Employee Edit Form Chatty API Pattern

**Location:** `EmployeeController.EditEmployee()` and `AddEmployee()` (lines 116-126, 202-211)

Loading the add/edit employee form makes 7 sequential API calls: employee data + teams + managers + timezones + locations + user types + employment types. These could be a single composite endpoint or cached.

### 7. Validation Re-fetches All Reference Data

**Location:** `TimeSheetController.ValidateTimeEntries()` (line 681+)

Before saving time entries, the validation method re-fetches the full work type list, project list, and project item list from the API just to cross-check entries. This data was already available on the page that submitted the form.

---

## Reliability

### 8. Silent Exception Swallowing (Critical)

**Location:** `EFTimeTrackerDbRepository.TimeEntryAdd()` (line 260-262), `TimeEntryUpdate()` (line 301-304)

Both `TimeEntryAdd` and `TimeEntryUpdate` catch all exceptions inside a `TransactionScope` and do nothing with them - empty `catch (Exception ex) { }` blocks. If a database error occurs mid-batch, the transaction silently rolls back, the method returns 0, and the user gets no error message and no indication their data was lost.

### 9. API Errors Silently Return Default Values

**Location:** `APIExtension.InvokeGet<T>()` (lines 73-87)

When an API call returns an HTTP error (4xx/5xx), the `WebException` is caught, the error response body is read into a local variable (`string error`), and then discarded. The method then attempts to deserialize an empty string, which returns `default(T)` (null for reference types, 0 for value types). Callers have no way to distinguish "no data" from "server error."

### 10. Authentication Ticket Re-validates on Every Request

**Location:** `AuthenticateExtension` -> `Authentication.GetAuthenticatedUserFromTicket()` (lines 112-128)

When the session cache is empty but a Forms Auth cookie exists, the system deserializes the stored credentials and calls `ValidateUser()` again, which makes a full API round-trip including Active Directory authentication. This means session expiry triggers a full AD re-authentication on the next request, which is slow and can fail if AD is temporarily unreachable.

### 11. Password Stored in Forms Auth Cookie

**Location:** `AccountController.Login()` (line 53)

The full `LoginModel` (including `Password`) is serialized as the Forms Authentication ticket's `UserData`. Even though the cookie is encrypted, this means the plaintext password lives in server memory during every request that rehydrates the ticket, and the encrypted password persists in the browser cookie for the session duration.

### 12. ChangePassword Is Non-Functional

**Location:** `EFTimeTrackerDbRepository.EmployeeUpdatePassword()` (lines 436-445)

The entire stored procedure call is commented out. The method always returns `null`, which the controller interprets as failure. The feature appears in the UI but doesn't work.

---

## Architecture

### 13. No Dependency Injection in Web Frontend

**Location:** All web controllers

The API project uses Unity for DI, but the web frontend manually instantiates all dependencies with `new`. This prevents testing, forces tight coupling, and means every controller creates duplicate service instances.

### 14. Duplicated Model Definitions

**Location:** `Web/Source/.../Models/` vs `Services/Source/.../Model/TrackerModel.cs`

Nearly every model (`TimeEntrySearchModel`, `EmployeeModel`, `ProjectModel`, etc.) is defined twice: once in the API's data access layer and again in the web frontend's Models folder. These definitions drift independently and have subtle differences (e.g., the web `TimeEntry.WorkHour` is `double`, the API's is `decimal`).

### 15. Mixed HTTP Client Implementations

**Location:** `TimeSheetController.GetAllApplication()`, `GetAllProjectItem()`, `GetAllWorkType()`

Some controller methods use the `APIExtension` wrapper, while others create raw `HttpWebRequest` objects directly, bypassing logging, error handling, and any future centralized concerns. Three different HTTP patterns coexist: `HttpWebRequest` (direct), `APIExtension` (wrapper around `HttpWebRequest`), and `RestClient` (in `InvokeServiceWithBasicAuth`).

### 16. URL Construction via String Concatenation

**Location:** Throughout all web controllers

API URLs are built by concatenating strings with query parameters: `APIResources.EmployeeSearch + "?searchBy=" + searchText + "&managerId=" + managerId + ...`. Values are not URL-encoded, so special characters in search text, comments, or names can break requests or cause incorrect results.

### 17. Static AutoMapper Configuration

**Location:** `AutoMapperExtension.Configure()`, uses deprecated `Mapper.Initialize()` static API

AutoMapper is configured using the legacy static `Mapper.Initialize()` API, which is not thread-safe during configuration and was removed in AutoMapper 9+. This pins the application to old AutoMapper versions.

---

## Security

### 18. No API Authentication

**Location:** All API controllers in `TimeTrackerWebAPI`

The Web API has no authentication or authorization middleware. Any client that can reach the API endpoint can read, create, update, or delete any data for any employee. The API relies entirely on network isolation for security.

### 19. Non-Production Authentication Bypass

**Location:** `EFTimeTrackerDbRepository.ADAuthentication()` (lines 359-375)

When `IsProduction` is `false`, the system accepts any valid AD username without checking the password (`isValid = true`). If the `IsProduction` config flag is accidentally left as `false` in a deployed environment, all accounts are effectively passwordless.

### 20. Delete Operations Use HTTP GET

**Location:** `EmployeeController.EmployeeDelete()`, `ProjectController.ProjectDelete()` (API-side)

Delete operations are exposed as `[HttpGet]` endpoints with IDs in query strings. This means browser pre-fetching, crawlers, or cached links can trigger deletions. Deletes should be POST/DELETE operations with anti-forgery tokens.

### 21. No Input Validation on API Layer

**Location:** All API controllers

None of the API controller actions validate input. Null employee IDs, negative hours, future dates, and empty required fields are all passed directly to stored procedures. Validation exists only in FluentValidation on the web side, but the API is independently accessible.

---

## Observability

### 22. Inconsistent Error Logging

Error handling varies across the codebase: some methods log and rethrow, some log and swallow, some don't log at all (the silent catch blocks in `TimeEntryAdd`/`TimeEntryUpdate`). There is no correlation ID or request tracing across the web-to-API boundary, making it difficult to diagnose failures end-to-end.

### 23. 5-Minute Command Timeout with No Visibility

**Location:** `EFContextFactory` (line 22: `CommandTimeout = 300`)

The EF context is configured with a 300-second (5-minute) command timeout. Long-running stored procedures will block a thread for up to 5 minutes with no visibility into what is running or why it is slow.

---

## Data Integrity

### 24. No Concurrency Control

There is no optimistic concurrency checking on any entity. Two admins editing the same employee, project, or time entry simultaneously will silently overwrite each other's changes (last-write-wins).

### 25. Soft Delete Without Filtering

Delete operations pass a `deleteUserId` to stored procedures, suggesting soft deletes (marking records inactive rather than removing them). However, many GET endpoints accept an `isActive` parameter that defaults to `null`, meaning queries may return deleted records unless callers explicitly filter.
