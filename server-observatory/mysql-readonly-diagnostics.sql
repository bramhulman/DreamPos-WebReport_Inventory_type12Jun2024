-- MANUAL READ-ONLY DIAGNOSTICS. Review grants and test overhead before scheduling.
-- No account creation, schema changes, tracing, query text or application table reads.
SELECT VERSION() AS mysql_version, @@performance_schema AS performance_schema_enabled;
SELECT VARIABLE_NAME, VARIABLE_VALUE
FROM performance_schema.global_status
WHERE VARIABLE_NAME IN ('Uptime','Uptime_since_flush_status','Queries','Threads_connected','Threads_running','Aborted_connects','Aborted_clients');

-- CUMULATIVE per-default-schema statement aggregates. Not interval QPS or p95.
-- Use two valid samples/deltas, detect restart/truncation/digest eviction before interpreting.
-- Cross-schema statements are attributed to the default schema, not necessarily the accessed schema.
-- Confirm instrumentation coverage. No DIGEST_TEXT or SQL_TEXT is selected.
SELECT /*+ MAX_EXECUTION_TIME(2000) */
 SCHEMA_NAME,
 SUM(COUNT_STAR) AS statements_total,
 SUM(SUM_TIMER_WAIT) / 1000000000 AS elapsed_ms_total,
 SUM(SUM_ERRORS) AS errors_total,
 SUM(SUM_ROWS_EXAMINED) AS rows_examined_total,
 SUM(SUM_ROWS_SENT) AS rows_sent_total
FROM performance_schema.events_statements_summary_by_digest
WHERE SCHEMA_NAME IS NOT NULL
 AND SCHEMA_NAME NOT IN ('mysql','sys','performance_schema','information_schema')
GROUP BY SCHEMA_NAME
ORDER BY elapsed_ms_total DESC
LIMIT 100;
