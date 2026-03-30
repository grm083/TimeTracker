# TimeTracker - Actionable Fix Roster

Items from the blind spots analysis that are fully within this application's control, requiring no stored procedure changes, database schema modifications, or infrastructure team involvement.

Each item is tagged with an effort estimate (S/M/L) and severity (Critical/High/Medium).

---

## Completed

The following fixes have been implemented:

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
| ~~2~~ | API errors silently returned default values - `APIExtension` now logs and throws on HTTP errors | *batch 2* |
| ~~6~~ | New `HttpWebRequest` per API call (socket exhaustion) - replaced with shared static `HttpClient` | *batch 2* |
| ~~5~~ | No caching of reference data - created `ReferenceDataCache` wrapping `MemoryDataCacheManager` with 5-min TTL | *batch 2* |
| ~~9~~ | Employee edit form 7 sequential API calls - lookup data now served from cache (see #5) | *batch 2* |
| ~~8~~ | Monthly View N+1 loop - parallelized per-employee API calls with `Parallel.ForEach` | *batch 2* |
| ~~11~~ | No input validation on API layer - added global `ValidateModelAttribute` action filter | *batch 2* |
| ~~20~~ | New EF context per repository call - changed repository from singleton to per-request lifetime in Unity | *batch 2* |
| ~~14~~ | `WorkHour` type mismatch (`double` vs `decimal`) - aligned web-side `TimeEntry.WorkHour` to `decimal` | *batch 2* |
| ~~18~~ | Static AutoMapper configuration - migrated to instance-based `MapperConfiguration` with `IMapper`, registered in Unity | *batch 2* |
| ~~19~~ | Inconsistent error logging - added correlation ID header (`X-Correlation-ID`) flowing web-to-API, added `GlobalExceptionFilterAttribute` | *batch 2* |
| ~~7~~ | New object instantiation in every controller constructor - all web controllers now use constructor injection | *batch 2* |
| ~~15~~ | No DI in web frontend - added Unity DI container with `UnityConfig`, registered shared `APIExtension`/`APIConfiguration`/`SessionCacheManager`/`ILogger` | *batch 2* |
| ~~12~~ | No API authentication - added `ApiKeyAuthHandler` delegating handler validating `X-Api-Key` header (backward-compatible: bypassed when no key configured) | *batch 2* |

---

All 22 items from the original roster have been addressed.
