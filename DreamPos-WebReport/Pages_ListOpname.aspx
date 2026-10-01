<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ListOpname.aspx.vb" Inherits="DreamPos_WebReport.Pages_ListOpname" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* ── Loading Spinner ── */
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
        #spinner img { width: 70px; height: 70px; }

        /* ── Filter Bar ── */
        .filter-card {
            background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
            color: white; border-radius: 6px;
            padding: 8px 14px; margin-bottom: 10px;
            box-shadow: 0 3px 6px rgba(0,0,0,0.08);
        }
        .filter-card label { font-size: 0.72rem; margin-bottom: 2px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.3px; }

        /* ── DataTables Toolbar Alignment ── */
        .custom-dt-length label, .custom-dt-filter label {
            margin-bottom: 0 !important;
            display: flex;
            align-items: center;
        }
        .custom-dt-length select, .custom-dt-filter input {
            margin: 0 0.5rem;
        }

        /* ── Card ── */
        .card-opname { border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.07); }

        /* ── Table Styling ── */
        .table-opname { font-size: 0.83rem; width: 100% !important; }
        .table-opname thead th {
            background-color: #1e3a5f !important;
            color: #ffffff !important;
            font-weight: 700 !important;
            font-size: 0.8rem !important;
            text-align: center;
            vertical-align: middle !important;
            white-space: nowrap;
            padding: 8px 8px !important;
            border: 1px solid #142841 !important;
        }
        .table-opname tbody td {
            vertical-align: middle !important;
            padding: 5px 8px !important;
            font-size: 0.83rem;
        }
        .table-opname tbody tr:hover { background-color: #eaf4ff !important; cursor: pointer; }

        /* variance */
        .var-positive { color: #c0392b; font-weight: 700; }
        .var-negative { color: #e67e22; font-weight: 700; }
        .var-zero     { color: #27ae60; font-weight: 700; }

        /* ── Modal X button ── */
        .btn-close {
            background: transparent url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23000'%3e%3cpath d='M.293.293a1 1 0 011.414 0L8 6.586 14.293.293a1 1 0 111.414 1.414L9.414 8l6.293 6.293a1 1 0 01-1.414 1.414L8 9.414l-6.293 6.293a1 1 0 01-1.414-1.414L6.586 8 .293 1.707a1 1 0 010-1.414z'/%3e%3c/svg%3e") center/1em auto no-repeat;
            opacity: .5; height: 15px;
        }
        .btn-close:hover { opacity: 1; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

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
    </script>

    <%-- Loading Spinner --%>
    <div id="spinner" class="modal" style="display:none">
        <div class="center">
            <img alt="Loading..." src="/images/loader.gif" />
            <div class="font-weight-bold text-primary mt-2">Memuat data&hellip;</div>
        </div>
    </div>

    <%-- Content Header --%>
    <div class="content-header py-1">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-list-alt text-primary mr-2"></i>
                        <asp:Label ID="lblTitle" runat="server" Text="List Opname"></asp:Label>
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item active">List Opname</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">

        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
        </span>

        <%-- Data Table Card --%>
        <div class="card card-opname">
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="upCrudGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:GridView ID="gvOpname" runat="server" Width="100%"
                                AutoGenerateColumns="false"
                                DataKeyNames="invdate,ccid,stkid,custid,Stkno,stkname,ccname,opencost,openqty,cost,ttlqty,bclose,closeqty,closecost,varianceqty,factor1,unit1,unit2, createddate,trtype, type"
                                ShowHeaderWhenEmpty="True"
                                OnPreRender="gvOpname_PreRender"
                                OnRowCommand="gvOpname_RowCommand"
                                CssClass="table table-bordered table-hover table-opname mb-0">
                                <Columns>
                                    <asp:ButtonField CommandName="editRecord"
                                        ButtonType="Button" Text="Edit"
                                        HeaderText="" ItemStyle-CssClass="text-center" ItemStyle-Width="4%">
                                        <ControlStyle CssClass="btn btn-sm btn-info"></ControlStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="invdate" SortExpression="invdate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Tanggal" />
                                    <asp:BoundField DataField="ccname" SortExpression="ccname" HeaderText="Cost Centre" />
                                    <asp:BoundField DataField="stkno" SortExpression="stkno" HeaderText="Kode Barang" />
                                    <asp:BoundField DataField="stkname" SortExpression="stkname" HeaderText="Nama Barang" />
                                    <asp:BoundField DataField="type" SortExpression="type" HeaderText="Tipe" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="ttlqty" DataFormatString="{0:#,0}" SortExpression="ttlqty" HeaderText="Total Qty" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="unit2" DataFormatString="{0:#,0}" SortExpression="unit2" HeaderText="Satuan" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="cost" DataFormatString="{0:#,0}" SortExpression="cost" HeaderText="HPP" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="closeqty" DataFormatString="{0:#,0}" SortExpression="closeqty" HeaderText="Opname Qty" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="closecost" DataFormatString="{0:#,0}" SortExpression="closecost" HeaderText="Opname Cost" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="varianceqty" DataFormatString="{0:#,0}" SortExpression="varianceqty" HeaderText="Variance Qty" ItemStyle-CssClass="text-right font-weight-bold" HeaderStyle-CssClass="text-center" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%-- Modal Edit --%>
        <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal fade" id="editModal" tabindex="-1" role="dialog"
                    aria-labelledby="editModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header py-2 px-3" style="background:#1e3a5f;">
                                <h5 class="modal-title text-white font-weight-bold" id="editModalLabel">
                                    <i class="fas fa-edit mr-2"></i>Edit Record Opname
                                </h5>
                                <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close" style="opacity:1; text-shadow:none;">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <asp:HiddenField ID="HfUpdatecc" runat="server" />
                                <asp:HiddenField ID="HfUpdatestock" runat="server" />
                                <asp:HiddenField ID="Hffactor1" runat="server" />
                                <asp:HiddenField ID="Hfstkno" runat="server" />
                                <asp:HiddenField ID="HfUpdatedate" runat="server" />

                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">Cost Centre</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtcostcentre" Enabled="false" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">Nama Barang</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtStockNameUpdate" runat="server" Enabled="false" CssClass="form-control form-control-sm" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">HPP (Cost)</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtcost" Enabled="false" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">Opening Cost</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtclosecost" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">Total Qty</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtttlqty" runat="server" Enabled="false" CssClass="form-control form-control-sm" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="lblttlu2" runat="server" CssClass="form-control-plaintext font-weight-bold" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">Qty Unit 1</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtcloseqtyu1" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="lblunit1" runat="server" CssClass="form-control-plaintext font-weight-bold" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label font-weight-bold">Qty Unit 2</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtcloseqtyu2" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="lblunit2" runat="server" CssClass="form-control-plaintext font-weight-bold" />
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer py-2">
                                <asp:Label ID="lblResult" Visible="false" runat="server" CssClass="mr-auto text-danger"></asp:Label>
                                <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">
                                    <i class="fas fa-times mr-1"></i>Tutup
                                </button>
                                <asp:Button ID="btnSave" runat="server" Text="Simpan Perubahan"
                                    CssClass="btn btn-primary btn-sm"
                                    ValidationGroup="ValidationGroupUpdate"
                                    OnClick="btnUpdate_Click" />
                            </div>
                        </div><!-- /.modal-content -->
                    </div><!-- /.modal-dialog -->
                </div><!-- /.modal Edit -->
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvOpname" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </div><%-- /.container-fluid --%>

    <%-- DataTables Buttons --%>
    <script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>

    <script type="text/javascript">
        var dataTable;

        function pageLoad() {
            if (dataTable) {
                dataTable.destroy();
                dataTable = null;
            }
            setTimeout(function () {
                dataTable = $('#<%= gvOpname.ClientID %>').DataTable({
                    "pageLength": 15,
                    "lengthMenu": [10, 15, 25, 50, 100],
                    "order": [[1, "asc"]],
                    destroy: true,
                    paging: true,
                    searching: true,
                    stateSave: false,
                    responsive: false,
                    autoWidth: false,
                    deferRender: true,
                    dom: '<"d-flex justify-content-between align-items-center py-3 px-4"<"d-flex align-items-center custom-dt-length"l><"d-flex align-items-center custom-dt-filter"<"mr-2"f>B>>rtip',
                    buttons: [
                        { extend: 'excel', text: '<i class="fas fa-file-excel mr-1"></i>Excel', className: 'btn btn-sm btn-success', title: 'List Opname' },
                        { extend: 'csv',   text: '<i class="fas fa-file-csv mr-1"></i>CSV',   className: 'btn btn-sm btn-info',    title: 'List Opname' }
                    ],
                    language: {
                        search: '',
                        searchPlaceholder: 'Cari...',
                        lengthMenu: 'Tampilkan _MENU_ baris',
                        info: 'Menampilkan _START_ - _END_ dari _TOTAL_ data',
                        paginate: { previous: '&laquo;', next: '&raquo;' }
                    }
                });
            }, 100);

            // Bootstrap modal init
            var modalEl = $('#editModal');
            if (modalEl.length) {
                modalEl.removeClass('fade');
                modalEl.modal({ backdrop: 'static', keyboard: true, show: false });
                modalEl.addClass('fade');
            }
        }

        // Modal dismiss
        $(document).on('click', '[data-bs-dismiss="modal"]', function () {
            $(this).closest('.modal').modal('hide');
        });
        $(document).on('click', '.modal-backdrop', function () {
            $('.modal.show').modal('hide');
        });

        // Row click to edit
        $(document).ready(function () {
            var attachRowClick = function () {
                $('#<%= gvOpname.ClientID %> tbody tr').css('cursor', 'pointer');
                $('#<%= gvOpname.ClientID %> tbody tr').off('click').on('click', function (e) {
                    if (!$(e.target).closest('a, button, input, img').length) {
                        $(this).find("input[value='Edit']").click();
                    }
                });
            };
            attachRowClick();

            if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(function () { $('#spinner').show(); });
                prm.add_endRequest(function () {
                    $('#spinner').hide();
                    attachRowClick();
                });
            }
        });
    </script>
</asp:Content>
