# Input form audit

## Implemented in this stage

- Pages_SupplierEdit.aspx and Pages_OwnEdit.aspx: opt-in shared responsive form layout.
- Removed fixed 800px input widths and the script forcing 80% width on mobile.
- Labels use a readable desktop column and stack above inputs on small screens.
- Associated labels with inputs; validation messages appear below each input.
- Replaced action layout tables with wrapping flex containers; Cancel is secondary.
- Preserved server IDs, handlers, maximum lengths, validation targets and visibility.
- No changes to VB, database writes, or existing JavaScript library loading.

## Further inspection needed

The active root edit pages below still contain fixed-width input rules or scripts
forcing form width to 80%. These are source-level findings, not confirmed browser
defects. Do not apply simple-form styles blindly to transaction detail grids.

- Pages_limitStkEdit.aspx, Pages_replaceStkEdit.aspx
- Pages_Inv_CnvrEdit.aspx, Pages_InvBatchEdit.aspx, Pages_InvInvoiceEdit.aspx
- Pages_InvoicePaymentEdit.aspx, Pages_InvReturnEdit.aspx, Pages_InvRwasteEdit.aspx
- Pages_ItemMasterEdit.aspx, Pages_StkEdit.aspx, Pages_RtnWHEdit.aspx
- Pages_CPOEdit.aspx, Pages_InvRcvEdit.aspx, User_Edit.aspx, wastelist_edit.aspx

## Verification

Static checks compare protected control attributes before/after transformation.
No rebuild was run, as required by .antigravityrules. Live IIS verification remains:
check desktop/mobile layout, required-field validation, Add/Update and Cancel.
Legacy jQuery imports and loading behavior were not changed in this visual stage.

## Second stage

- Applied the same opt-in layout to Pages_ICCEdit.aspx and Pages_ExpOtherEdit.aspx.
- Retained checkbox and dropdown AutoPostBack, event handlers, calendar target and date format.
- Calendar input stays beside its icon; checkbox has a larger visible control.
- Recipe Detail was subsequently updated; see recipe-detail-form-validation.md
  for numeric validation changes and the manual IIS regression checklist.
- Static desktop/mobile fixtures do not test IIS, calendar behavior, or database saves.
