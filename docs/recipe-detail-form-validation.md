# Recipe Detail Edit validation

## Changes
- Responsive opt-in form layout; quantity and unit share one input group.
- Removed malformed numeric keydown handlers. Paste, delete and keyboard navigation are not blocked.
- Removed page-local legacy jQuery and unused jQuery UI imports/progress handlers.
- Server parses Decimal with the current request culture, without thousands separators.
- Qty must remain non-zero; Loss Factor must be numeric. Existing sign/range rules are unchanged.
- Blank, malformed and overflow numbers are rejected without replacing the user's input.
- Type and Item placeholder value 0 is rejected on server validation.
- Errors are HTML-encoded via InnerText; save exceptions go to the existing error log.
- Buttons retain their server IDs and markup event handlers; duplicate Handles registration removed.
- Item selection no longer attempts to select the parent recipe ID before loading the detail.
- No SQL, schema, cost formula or transaction changes.

## Manual IIS checks (rebuild by user)
1. Open an existing detail whose parent recipe ID differs from its stock/item ID.
2. Confirm Type, Item, Qty, unit and Loss Factor show the saved values.
3. Update decimal Qty and Loss Factor using the server culture separator.
4. Try blank, alphabetic, zero Qty, grouping separators and a value exceeding Decimal range.
5. Confirm invalid input remains visible and no write occurs.
6. Select the Type or Item placeholder and confirm saving is blocked.
7. Cancel while invalid: navigation back must not be blocked by validation.
8. Confirm one click invokes one save; successful save returns to Recipe Edit.
9. Check desktop and 390px-wide mobile, including a long item name and unit.

No build or live database writes were performed by the agent. Static previews do not
verify WebForms postbacks or successful database writes.
