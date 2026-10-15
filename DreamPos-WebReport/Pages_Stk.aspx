<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_Stk.aspx.vb" Inherits="DreamPos_WebReport.Pages_Stk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/inventory-mobile.css" />
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/buttons/1.7.0/css/buttons.dataTables.min.css" />
    <style type="text/css">
        .table-custom thead th {
            background-color: #1e3a5f !important;
            color: #ffffff !important;
            font-weight: 700;
            text-align: center;
            vertical-align: middle;
            border-color: #142841 !important;
            padding: 9px 8px;
            font-size: 0.88rem;
        }
        .table-custom tbody td {
            vertical-align: middle;
            font-size: 0.86rem;
            padding: 7px 10px;
        }
        .table-custom tbody tr:hover {
            background-color: #f1f7fd;
            cursor: pointer;
        }
        .table-custom tbody tr.clickable-row:focus {
            outline: 2px solid #007bff;
            outline-offset: -2px;
        }
        .filter-card {
            background: linear-gradient(135deg, #1e3a5f 0%, #2c3e50 100%);
            border-radius: 6px;
            padding: 10px 16px;
            margin-bottom: 12px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.08);
        }
        #dtExcelBtnContainer .dt-button {
            background: #28a745 !important;
            color: #ffffff !important;
            border: none !important;
            font-weight: 600 !important;
            font-size: 0.875rem !important;
            border-radius: 4px !important;
            padding: 4px 10px !important;
            margin-left: 4px !important;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1) !important;
        }
        #dtExcelBtnContainer .dt-button:hover {
            background: #218838 !important;
        }
        #spinner.modal {
            position: fixed; top: 0; left: 0;
            width: 100%; height: 100%;
            background-color: rgba(255,255,255,0.75);
            z-index: 9999; text-align: center;
        }
        #spinner .center {
            position: absolute; top: 50%; left: 50%;
            transform: translate(-50%,-50%);
        }
        #spinner img { width: 64px; height: 64px; }
    </style>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.0/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.0/js/buttons.html5.min.js"></script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Delete data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        (function () {
            function initializeStockTable() {
                var notice = document.getElementById('stockTableError');
                if (!document.getElementById('tblEmpDetails')) return;
                if (!window.jQuery || !jQuery.fn.DataTable) {
                    notice.textContent = 'Some table features are unavailable. You can still use Search or reload the page.';
                    notice.hidden = false;
                    return;
                }
                var $ = window.jQuery;
                var search = $("[id$='txtsearch']");
                var button = $("[id$='btnsrc']");
                search.off('.stockSearch');
                button.off('.stockSearch');
                var table;
                try {
                    table = $.fn.dataTable.isDataTable('#tblEmpDetails')
                        ? $('#tblEmpDetails').DataTable()
                        : $('#tblEmpDetails').DataTable({
                            dom: 'rtip',
                            columnDefs: [{ targets: -1, orderable: false, searchable: false }]
                        });
                    table.search(search.val() || '').draw();
                    search.on('keyup.stockSearch change.stockSearch', function () {
                        table.search(this.value).draw();
                    }).on('keydown.stockSearch', function (event) {
                        if (event.key === 'Enter') {
                            event.preventDefault();
                            table.search(this.value).draw();
                        }
                    });
                    button.on('click.stockSearch', function (event) {
                        event.preventDefault();
                        table.search(search.val()).draw();
                    });
                } catch (error) {
                    search.off('.stockSearch');
                    button.off('.stockSearch');
                    notice.textContent = 'Some table features are unavailable. You can still use Search or reload the page.';
                    notice.hidden = false;
                    return;
                }
                try {
                    if (!$.fn.dataTable.Buttons || !window.JSZip ||
                        !$.fn.dataTable.ext.buttons.excelHtml5) throw new Error('Export unavailable');
                    if (!document.querySelector('#dtExcelBtnContainer .dt-button')) {
                        new $.fn.dataTable.Buttons(table, {
                            buttons: [{
                                extend: 'excelHtml5',
                                text: '<i class="fas fa-file-excel mr-1"></i>Export Excel',
                                className: 'btn btn-success btn-sm',
                                exportOptions: { columns: [0, 1, 2] }
                            }]
                        });
                        $('#dtExcelBtnContainer').append(table.buttons().container().find('button'));
                    }
                    notice.hidden = true;
                } catch (error) {
                    notice.textContent = 'Excel export is unavailable. You can still browse and search stock. Reload the page to try again.';
                    notice.hidden = false;
                }
            }
            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', initializeStockTable);
            } else {
                initializeStockTable();
            }
        }());
        if (window.jQuery) {
            $(document).off('.stockRowEdit').on('click.stockRowEdit keydown.stockRowEdit', '#tblEmpDetails tbody tr.clickable-row', function (event) {
                if ($(event.target).closest('a, button, input, select, textarea, label, [role="button"]').length) return;
                if (event.type === 'keydown') {
                    if (event.target !== this || (event.key !== 'Enter' && event.key !== ' ')) return;
                    event.preventDefault();
                } else if (window.getSelection && window.getSelection().toString()) {
                    return;
                }
                var editButton = $(this).find('.stock-edit-trigger')[0];
                if (editButton && !editButton.disabled) {
                    $('#spinner').show();
                    editButton.click();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">

    <!-- Loading Spinner -->
    <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="Loading..." src="/images/loader.gif" />
            <div class="font-weight-bold text-primary mt-2">Loading data...</div>
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-2">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-boxes text-primary mr-2"></i>Stock Master
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item active">Stock Master</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid mb-4">
        <!-- Filter / Toolbar Card -->
        <div class="filter-card" id="formload" runat="server">
            <div class="form-row align-items-center">
                <div class="col-auto">
                    <asp:Label for="txtsearch" runat="server" CssClass="text-white font-weight-bold mb-0">
                        <i class="fas fa-search mr-1"></i>Search:
                    </asp:Label>
                </div>
                <div class="col-md-3 col-sm-5 my-1">
                    <asp:TextBox id="txtsearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search stock..." type="search" aria-label="Search"></asp:TextBox>
                </div>
                <div class="col-auto my-1 d-flex flex-wrap align-items-center">
                    <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-sm btn-light text-primary font-weight-bold shadow-sm mr-1" title="Search">
                        <i class="fas fa-search mr-1"></i>Search
                    </button>
                    <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click" class="btn btn-sm btn-primary font-weight-bold shadow-sm mr-1" title="New Stock">
                        <i class="fas fa-plus mr-1"></i><asp:Label runat="server" Text="New"></asp:Label>
                    </button>
                    <button type="button" runat="server" id="btnExportExcel" onserverclick="btnExportExcel_Click" class="btn btn-sm btn-info font-weight-bold shadow-sm mr-1" title="Import Excel">
                        <i class="fas fa-file-import mr-1"></i><asp:Label runat="server" Text="Import Excel"></asp:Label>
                    </button>
                    <div id="dtExcelBtnContainer" class="d-inline-block"></div>
                </div>
            </div>
        </div>

        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
        </span>
        <div id="stockTableError" class="alert alert-warning" role="status" hidden></div>

        <!-- Table Card -->
        <div class="card card-outline card-primary shadow-sm">
            <div class="card-body p-0">
                <div class="form-horizontal w-100">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server">
                        <EmptyDataTemplate>
                            <p class="text-muted p-3 mb-0" role="status">No stock items found.</p>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div class="table-responsive">
                                <table id="tblEmpDetails" class="table table-bordered table-hover table-custom mb-0">
                                    <thead>
                                        <tr>
                                            <th style="width: 15%;">Code</th>
                                            <th>Name</th>
                                            <th>Group Name</th>
                                            <th style="width: 90px;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr class="clickable-row" tabindex="0" title="Edit stock" aria-label="Edit stock">
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Number")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("grpname")%>'></asp:Label>
                                </td>
                                <td class="text-center" style="white-space: nowrap;">
                                    <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit record"
                                        CssClass="stock-edit-trigger"
                                        CommandArgument='<%#Eval("ID").ToString %>' runat="server" ImageUrl="/images/edit.png" Style="vertical-align: middle; margin-right: 6px;" />
                                    <asp:ImageButton ToolTip="Delete record" OnClientClick="Confirm()"
                                        ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("ID").ToString %>' runat="server"
                                        ImageUrl="~/images/delete.png" Style="vertical-align: middle;"></asp:ImageButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
