<%@ Page Title="Inventory Transfer Edit" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvTrfEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvTrfEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .loading {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 10pt;
            border: 3px solid #007bff;
            border-radius: 8px;
            width: 220px;
            height: 110px;
            display: none;
            position: fixed;
            background-color: #ffffff;
            z-index: 9999;
            box-shadow: 0 4px 15px rgba(0,0,0,0.2);
            text-align: center;
            padding-top: 20px;
        }
        .trf-table td {
            vertical-align: middle !important;
            padding: 0.45rem 0.6rem !important;
        }
        .trf-table th {
            vertical-align: middle !important;
            padding: 0.6rem !important;
            font-size: 0.85rem;
            letter-spacing: 0.3px;
        }
        .trf-u1, .trf-u2 {
            min-width: 70px;
            text-align: right;
            font-weight: 600;
        }
        .trf-total {
            font-size: 1rem;
            font-weight: 700;
            color: #007bff;
        }
        .trf-stock {
            font-size: 0.85rem;
        }
        .is-invalid {
            border-color: #dc3545 !important;
            background-color: #fff8f8 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="spinner" class="loading">
        <div style="font-size: 1.8rem; color: #007bff; margin-bottom: 5px;">
            <i class="fas fa-circle-notch fa-spin"></i>
        </div>
        <div style="font-size: 0.95rem; color: #333; font-weight: 600;">Processing Transfer...</div>
        <div style="font-size: 0.8rem; color: #6c757d;">Please wait a moment.</div>
    </div>

    <!-- Content Header -->
    <div class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 align-items-center">
                <div class="col-sm-6">
                    <h1 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-exchange-alt mr-2 text-primary"></i>Inventory Transfer
                    </h1>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right bg-transparent p-0 mb-0">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_InvTransfer.aspx">Transfer List</a></li>
                        <li class="breadcrumb-item active">Edit Transfer</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid pb-4">
        <!-- Transfer Header Card -->
        <div class="card card-outline card-primary shadow-sm mb-3">
            <div class="card-header bg-white py-3">
                <div class="d-flex flex-wrap align-items-center justify-content-between">
                    <div>
                        <h5 class="card-title font-weight-bold mb-0 text-dark">
                            <i class="fas fa-file-invoice mr-1 text-primary"></i>Transfer Information
                        </h5>
                    </div>
                    <div class="btn-group mt-2 mt-md-0">
                        <button type="button" runat="server" class="btn btn-primary shadow-sm font-weight-bold"
                            data-toggle="modal" id="btnshow" data-target="#ModalItem">
                            <i class="fas fa-plus mr-1"></i> Add Items
                        </button>
                        <button type="button" runat="server" class="btn btn-success shadow-sm font-weight-bold"
                            id="btnSave" onserverclick="btnSave_Click" onclientclick="return validateBeforeSave();">
                            <i class="fas fa-save mr-1"></i> Save Transfer
                        </button>
                        <button type="button" runat="server" class="btn btn-secondary shadow-sm font-weight-bold"
                            id="btnBack" onserverclick="btnBack_Click" causesvalidation="false">
                            <i class="fas fa-arrow-left mr-1"></i> Back
                        </button>
                        <!-- Keep btnedit hidden for designer compatibility -->
                        <button type="button" runat="server" id="btnedit" style="display:none;" onserverclick="btnedit_ServerClick"></button>
                    </div>
                </div>
            </div>
            <div class="card-body py-3">
                <div class="row" id="formload">
                    <div class="col-md-4 mb-2">
                        <asp:Label runat="server" ID="lblorderno" CssClass="font-weight-bold text-secondary text-uppercase small d-block" Text="Transfer ID"></asp:Label>
                        <asp:TextBox runat="server" ID="txtLinkid" CssClass="form-control font-weight-bold bg-light" MaxLength="30" ReadOnly="true" placeholder="(Auto-generated on Save)"></asp:TextBox>
                    </div>
                    <div class="col-md-5 mb-2">
                        <asp:Label runat="server" ID="Label2" CssClass="font-weight-bold text-secondary text-uppercase small d-block" Text="From (Source Cost Centre)"></asp:Label>
                        <asp:DropDownList ID="ddlFrom" OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged" runat="server"
                            CssClass="form-control custom-select" AppendDataBoundItems="True" AutoPostBack="true"
                            EnableViewState="True" ViewStateMode="Enabled">
                            <asp:ListItem Value="0">-- Select Source Cost Centre --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="ddlFrom" InitialValue="0" ErrorMessage="* Source Cost Centre is required"
                            ForeColor="Red" Display="Dynamic" CssClass="small font-weight-bold"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3 mb-2">
                        <label class="font-weight-bold text-secondary text-uppercase small d-block">Transfer Date</label>
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                            </div>
                            <input type="text" class="form-control bg-light font-weight-bold" readonly value="<%= DateTime.Now.ToString("dd-MMM-yyyy") %>" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Transfer Detail Items Card -->
        <div class="card shadow-sm mb-4">
            <div class="card-header bg-light d-flex justify-content-between align-items-center py-2">
                <h6 class="mb-0 font-weight-bold text-dark">
                    <i class="fas fa-boxes mr-1 text-info"></i>Items to Transfer
                </h6>
                <span class="badge badge-info px-2 py-1" id="itemCountBadge">
                    <i class="fas fa-bolt mr-1"></i>Direct Editing & Live Calculation Active
                </span>
            </div>
            <div class="card-body p-0 table-responsive">
                <asp:GridView ID="DgvTransfer" runat="server" Width="100%"
                    AutoGenerateColumns="false" CssClass="table table-bordered table-hover align-middle mb-0 trf-table"
                    HeaderStyle-CssClass="thead-dark" CellPadding="5">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Stk ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStkID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Name" HeaderStyle-Width="220px">
                            <ItemTemplate>
                                <div class="font-weight-bold text-dark">
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>
                                </div>
                                <small class="text-muted"><%# Eval("stk_no") %></small>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="cc" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCc" runat="server" Text='<%# Eval("Cc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Destination CC" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:Label ID="lblTo" runat="server" Text='<%# Eval("Cc_Name") %>' style="display:none;"></asp:Label>
                                <asp:DropDownList ID="DdlTo" runat="server" CssClass="form-control form-control-sm trf-ddl-to font-weight-bold">
                                    <asp:ListItem Value="0">-- Select Destination --</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit 1" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("U1") %>' style="display:none;"></asp:Label>
                                <asp:TextBox ID="txtEditUnit1" runat="server" Text='<%# Eval("U1") %>'
                                    CssClass="form-control form-control-sm text-right trf-u1" type="number" step="any" min="0"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="U1 Name" HeaderStyle-Width="55px">
                            <ItemTemplate>
                                <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U1_nm") %>' CssClass="small font-weight-bold text-muted"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit 2" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:Label ID="lblUnit2" runat="server" Text='<%# Eval("U2") %>' style="display:none;"></asp:Label>
                                <asp:TextBox ID="txtEditUnit2" runat="server" Text='<%# Eval("U2") %>'
                                    CssClass="form-control form-control-sm text-right trf-u2" type="number" step="any" min="0"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="U2 Name" HeaderStyle-Width="55px">
                            <ItemTemplate>
                                <asp:Label ID="lblU2Nm" runat="server" Text='<%# Eval("U2_nm") %>' CssClass="small font-weight-bold text-muted"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Factor" HeaderStyle-Width="60px" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblfactor" runat="server" Text='<%# Eval("factor1") %>' CssClass="badge badge-light border trf-factor"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Cost" HeaderStyle-Width="80px" ItemStyle-CssClass="text-right">
                            <ItemTemplate>
                                <asp:Label ID="lblCost" runat="server" Text='<%# Eval("Cost", "{0:#,0}") %>' CssClass="small"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total Trf" HeaderStyle-Width="100px" ItemStyle-CssClass="text-right">
                            <ItemTemplate>
                                <asp:Label ID="lblttlStock" runat="server" Text='<%# Eval("ttlStock") %>' CssClass="trf-total"></asp:Label>
                                <span class="trf-initial-ttl" style="display:none;"><%# Eval("ttlStock") %></span>
                                <div class="trf-warning text-danger font-weight-bold" style="display:none; font-size: 11px;">
                                    <i class="fas fa-exclamation-triangle"></i> Exceeds!
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current Stock" HeaderStyle-Width="100px" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' CssClass="badge badge-info trf-stock font-weight-bold px-2 py-1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="50px" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton ID="imgbtnDelete" runat="server" CssClass="btn btn-outline-danger btn-sm"
                                    ToolTip="Remove Item" OnClick="imgbtnDelete_Click" OnClientClick="Confirm();">
                                    <i class="fas fa-trash-alt"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5 text-muted">
                            <i class="fas fa-inbox fa-3x mb-2 text-secondary"></i>
                            <p class="mb-0 font-weight-bold">No items added to this transfer yet.</p>
                            <p class="small text-muted">Click the <strong>Add Items</strong> button above to select items.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>

        <!-- Modal Item Selection -->
        <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem"
                    aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                    <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
                        <div class="modal-content shadow-lg border-0">
                            <div class="modal-header bg-primary text-white py-3">
                                <h5 class="modal-title font-weight-bold" id="ModalStoreLabel">
                                    <i class="fas fa-boxes mr-2"></i>Select Items to Transfer
                                </h5>
                                <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>

                            <div class="p-3 bg-light border-bottom">
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text bg-white"><i class="fas fa-search text-muted"></i></span>
                                    </div>
                                    <input type="text" id="txtModalQuickSearch" class="form-control"
                                        placeholder="Type to quickly filter items below (sub-second live search)..." autocomplete="off" />
                                    <div class="input-group-append">
                                        <button type="button" class="btn btn-outline-secondary" id="btnClearModalSearch" title="Clear filter">
                                            <i class="fas fa-times"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="d-none">
                                    <!-- Hidden legacy search controls preserved for designer compatibility -->
                                    <div id="Div2" runat="server">
                                        <asp:TextBox id="txtSearch" runat="server"></asp:TextBox>
                                        <button runat="server" id="btnSearch" onserverclick="btnSearch_Click"></button>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-body p-0" style="max-height: 400px; overflow-y: auto;">
                                <div class="table-responsive">
                                    <table class="table table-hover table-striped mb-0" id="tblModalItems">
                                        <thead class="thead-light">
                                            <tr>
                                                <th style="width: 50px;" class="text-center">
                                                    <input type="checkbox" id="chkSelectAllModal" title="Select / Deselect All Visible" />
                                                </th>
                                                <th>Item Name</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:ListView ID="LsvStk" runat="server">
                                                <ItemTemplate>
                                                    <tr class="modal-item-row">
                                                        <td class="text-center">
                                                            <asp:CheckBox ID="CheckBox" runat="server" CssClass="chk-modal-item"></asp:CheckBox>
                                                        </td>
                                                        <td hidden="hidden">
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("name")%>'
                                                                CssClass="modal-item-name font-weight-bold text-dark"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <tr>
                                                        <td colspan="2" class="text-center py-4 text-muted">
                                                            <i class="fas fa-box-open fa-2x mb-2 text-secondary d-block"></i>
                                                            No items available in the selected Source Cost Centre.
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="modal-footer bg-light justify-content-between py-2">
                                <div>
                                    <span class="text-muted small font-weight-bold" id="selectedModalCount">
                                        Check items and click Add
                                    </span>
                                </div>
                                <div>
                                    <button type="button" class="btn btn-secondary mr-1" data-dismiss="modal">Close</button>
                                    <button type="button" class="btn btn-primary font-weight-bold" runat="server"
                                        onserverclick="RadButton1_ServerClick" id="RadButton1">
                                        <i class="fas fa-plus mr-1"></i> Add to Transfer
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Hidden labels for error and designer compatibility -->
        <div style="display:none;">
            <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red"></asp:Label>
            <asp:Label ID="lblnoitem" runat="server" Text="0"></asp:Label>
        </div>
    </div>

    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var loading = $("#spinner");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading.outerHeight() / 2, 0);
                var left = Math.max($(window).width() / 2 - loading.outerWidth() / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }

        $(document).on("submit", "form", function () {
            ShowProgress();
        });

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure you want to remove this item?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function openModalItem() {
            $('#ModalItem').modal('show');
            setTimeout(function () {
                $('#txtModalQuickSearch').focus();
            }, 300);
        }

        // Live calculation per row: total = (u1 * factor) + u2
        function recalculateRow($tr) {
            var u1 = parseFloat($tr.find('.trf-u1').val()) || 0;
            var u2 = parseFloat($tr.find('.trf-u2').val()) || 0;
            var factor = parseFloat($tr.find('.trf-factor').text()) || 1;
            var stock = parseFloat($tr.find('.trf-stock').text()) || 0;
            var initialTtl = parseFloat($tr.find('.trf-initial-ttl').text()) || 0;

            var total = (u1 * factor) + u2;
            $tr.find('.trf-total').text(total % 1 === 0 ? total.toFixed(0) : total.toFixed(2));

            // Exceed stock check: (total - initialTtl) > stock
            if ((total - initialTtl) > stock) {
                $tr.addClass('table-danger');
                $tr.find('.trf-u1, .trf-u2').addClass('is-invalid');
                $tr.find('.trf-warning').show();
            } else {
                $tr.removeClass('table-danger');
                $tr.find('.trf-u1, .trf-u2').removeClass('is-invalid');
                $tr.find('.trf-warning').hide();
            }
        }

        // Client-side validation before save
        function validateBeforeSave() {
            var hasError = false;
            var errorMsg = "";

            var ddlFromVal = $('#<%= ddlFrom.ClientID %>').val();
            if (!ddlFromVal || ddlFromVal === "0") {
                alert("Please select a Source Cost Centre.");
                $('#<%= ddlFrom.ClientID %>').focus();
                return false;
            }

            var rowCount = 0;
            $('#<%= DgvTransfer.ClientID %> tbody tr').each(function () {
                var $tr = $(this);
                if ($tr.find('.trf-u1').length > 0) {
                    rowCount++;
                    var itemName = $tr.find('span[id*="lblName"]').text().trim();
                    var ddlTo = $tr.find('.trf-ddl-to').val();
                    var total = parseFloat($tr.find('.trf-total').text()) || 0;
                    var stock = parseFloat($tr.find('.trf-stock').text()) || 0;
                    var initialTtl = parseFloat($tr.find('.trf-initial-ttl').text()) || 0;

                    if (!ddlTo || ddlTo === "0") {
                        hasError = true;
                        errorMsg = "Please select Destination Cost Centre for: " + itemName;
                        $tr.find('.trf-ddl-to').addClass('is-invalid').focus();
                        return false;
                    }

                    if (ddlTo === ddlFromVal) {
                        hasError = true;
                        errorMsg = "Destination Cost Centre cannot be the same as Source for: " + itemName;
                        $tr.find('.trf-ddl-to').addClass('is-invalid').focus();
                        return false;
                    }

                    if (total <= 0) {
                        hasError = true;
                        errorMsg = "Transfer quantity must be greater than 0 for: " + itemName;
                        $tr.find('.trf-u1').addClass('is-invalid').focus();
                        return false;
                    }

                    if ((total - initialTtl) > stock) {
                        hasError = true;
                        errorMsg = "Transfer quantity (" + total + ") exceeds available stock (" + stock + ") for: " + itemName;
                        $tr.find('.trf-u1').addClass('is-invalid').focus();
                        return false;
                    }
                }
            });

            if (rowCount === 0) {
                alert("Please add at least one item to transfer.");
                return false;
            }

            if (hasError) {
                alert(errorMsg);
                return false;
            }

            return true;
        }

        // Initialize events
        $(document).ready(function () {
            // Initial recalculation
            $('#<%= DgvTransfer.ClientID %> tbody tr').each(function () {
                if ($(this).find('.trf-u1').length > 0) {
                    recalculateRow($(this));
                }
            });

            // Recalculate on input change
            $(document).on('input change', '.trf-u1, .trf-u2', function () {
                recalculateRow($(this).closest('tr'));
            });

            // Instant modal filter
            $(document).on('keyup input', '#txtModalQuickSearch', function () {
                var val = $(this).val().toLowerCase().trim();
                $('#tblModalItems tbody tr.modal-item-row').each(function () {
                    var name = $(this).find('.modal-item-name').text().toLowerCase();
                    if (val === '' || name.indexOf(val) !== -1) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                });
            });

            $(document).on('click', '#btnClearModalSearch', function () {
                $('#txtModalQuickSearch').val('').trigger('input');
            });

            // Select All visible items in modal
            $(document).on('change', '#chkSelectAllModal', function () {
                var checked = $(this).is(':checked');
                $('#tblModalItems tbody tr.modal-item-row:visible input[type="checkbox"]').prop('checked', checked);
                updateModalCheckedCount();
            });

            $(document).on('change', '#tblModalItems tbody input[type="checkbox"]', function () {
                updateModalCheckedCount();
            });

            function updateModalCheckedCount() {
                var count = $('#tblModalItems tbody input[type="checkbox"]:checked').length;
                $('#selectedModalCount').text(count > 0 ? count + " item(s) selected" : "Check items and click Add");
            }
        });
    </script>
</asp:Content>