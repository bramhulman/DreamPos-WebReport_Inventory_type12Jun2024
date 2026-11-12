# Receive posting regression tests

Run `powershell -NoProfile -ExecutionPolicy Bypass -File tests\ReceivePosting\run.ps1` from the repository. The optional `MySqlBaseDir` and `VSToolsPath` parameters point at local MySQL 5.7 and Visual Studio WebApplication targets.

The runner creates its own MySQL datadir under `runtime/mysql-data`, binds only 127.0.0.1:33179, and refuses to reuse an occupied port. The test executable verifies `@@datadir` before creating/resetting its dedicated `codex_receive_posting_test` database. It never reads the application Web.config connection string. The instance stops in `finally`; runtime data/output are git-ignored.

Tests execute the compiled application classes, not a reimplementation. Six table fixtures come from `backup_fast/schema.sql`. Settings are overridden in memory in the test process and never saved. The build disables the version-writing prebuild event and the unrelated web packaging target. Output is `runtime/app-build`.

Coverage:

1. Cancel once and reject a repeated cancellation; carry through today.
2. Overlapping cancellation and receive for the same document rejected.
3. Injected balance SQL failure rolls back header, details and quantity; error context verified.
4. MySQL lock timeout 1205 aborts without posting and records diagnostic information.
5. Zero/negative stored receive bucket rebuilt without another subtraction; other active receipts preserved.
6. Active zero-quantity receipt rejected.
7. Cancel November-9-equivalent posting, receive on November-4-equivalent date, reject duplicate finish, and cancel using the persisted destination date.
8. Carry-forward respects an intervening physical opname.
9. Missing quiet daily balance recreated with explicit correct column names.
10. Failure in cost synchronization after carry-forward rolls back all affected dates.
11. Nullable movement buckets produce a verified numeric closing quantity.
12. Connection loss at commit is reported as uncertain and is never retried automatically.

`ui.test.cjs` tests double-click rejection, submit-before-disable ordering, blocking other buttons, AJAX completion/failure reset and restoration of previously disabled controls. It is a DOM-level unit test; it does not replace staging verification of the full WebForms page.

Captured diagnostics: `runtime/app-errLog.txt` (application logger) and `runtime/captured-operation-log.txt` (direct helper fault tests).
