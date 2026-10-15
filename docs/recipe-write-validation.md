# Recipe write validation

This change parameterizes Recipe writes and cost calculation without a schema migration.
Build in Visual Studio, then run these checks on a development database, not production.
Runtime/database validation has not been performed by the agent.

## Manual regression checks

1. Save a new recipe with an apostrophe in code/name/remarks. Reload and verify exact text.
2. Edit an existing recipe, then save a changed code. Confirm the existing copy-by-code behavior remains.
3. Use the same recipe code for two different customers. Update one and confirm the other is unchanged.
4. Add a detail with fractional qty/loss factor; edit it; add the same item again. Confirm quantities are preserved and the add operation increments once.
5. Add the same item when its saved quantity is zero. Confirm it updates the existing detail rather than creating another zero-quantity duplicate.
6. Try an item ID belonging to a different customer. Confirm the operation fails without inserting/updating a detail.
7. Replace the cost-centre list. Cause the second insert to fail in a test database and confirm the previous list remains intact.
8. Import a recipe whose code contains an apostrophe. Check the correct customer's recipe and stock are selected.
9. Delete master/detail/cost-centre records and verify records for another customer are untouched.
10. Compare recalculated cost against the old calculation for normal data, missing stock cost, loss factor, and fractional totals. Integer cost rounding remains unchanged.
11. Force a calculation query failure for one recipe. Confirm none of that recipe's cost-centre values change and the error is logged. Previously completed recipes in the batch remain committed.
12. Simultaneously insert from two sessions. Check for duplicate IDs, rollback, and logged conflicts. Legacy applications that allocate MAX(ID)+1 without participating in locking can still conflict; no schema change is included.

## Transaction scope

- Master save, detail save/import, and each delete are separate transactions.
- Replacing a recipe's cost-centre list is one transaction.
- Cost calculation commits per recipe, not per batch.
- The entire UI workflow (master save, centres, calculation, item-master recalculation) is not one transaction.
- Transaction rollback requires transactional tables (for example InnoDB). Verify the actual development and production table engines before deployment.
- Existing audit-log writes run after business-data commits and are not part of the same transaction.

## Static checks

- Project includes RecipeDb.vb.
- SQL value placeholders match SetParameter bindings in the changed functions.
- SQL statements in the changed write paths contain no concatenated user values.
- No build, schema change, or live database write was performed during implementation.
