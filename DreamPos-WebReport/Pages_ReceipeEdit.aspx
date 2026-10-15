<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ReceipeEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_ReceipeEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/inventory-mobile.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap4.min.css"/>
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.bootstrap4.min.css"/>
    <style type="text/css">
        .card-custom {
            border-radius: 6px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
            border: 1px solid #dee2e6;
            margin-bottom: 1rem;
        }
        .card-custom .card-header {
            background-color: #1e3a5f;
            color: #ffffff;
            font-weight: 600;
            padding: 10px 16px;
        }
        .table-custom thead th {
            background-color: #1e3a5f !important;
            color: #ffffff !important;
            font-weight: 700;
            text-align: center;
            vertical-align: middle;
            border-color: #142841 !important;
            padding: 8px 8px;
            font-size: 0.84rem;
        }
        .table-custom tbody td {
            vertical-align: middle;
            font-size: 0.84rem;
            padding: 6px 10px;
        }
        .table-custom tbody tr:hover {
            background-color: #f1f7fd;
        }
        .filter-card {
            background: linear-gradient(135deg, #1e3a5f 0%, #2c3e50 100%);
            border-radius: 6px;
            padding: 10px 16px;
            margin-bottom: 12px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.08);
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
    <script src="https://cdn.datatables.net/1.13.6/js/dataTables.bootstrap4.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.bootstrap4.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.btn-spinner').click(function () {
                if (typeof Page_IsValid === 'undefined' || Page_IsValid) {
                    $('#spinner').show();
                }
            });
        });
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <!-- Loading Spinner -->
    <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="Loading..." src="/images/loader.gif" />
            <div class="font-weight-bold text-primary mt-2">Processing...</div>
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-2">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-utensils text-primary mr-2"></i>Receipe Edit
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_Receipe.aspx">Receipe</a></li>
                        <li class="breadcrumb-item active">Receipe Edit</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid mb-4">
        <div id="recipeLoadError" class="alert alert-danger" role="alert" style="display:none;">
            Unable to load recipe data. Please reload the page or contact support.
        </div>
        <div id="recipeExportError" class="alert alert-warning" role="status" hidden>
            Excel export is unavailable. You can still view and edit the recipe. Reload the page to try again.
        </div>

        <!-- Upper Section: Recipe Form & Cost Centre Table -->
        <div class="row">
            <!-- Left: Recipe Main Form -->
            <div class="col-lg-6 col-md-12 mb-3">
                <div class="card card-custom h-100" id="formload">
                    <div class="card-header">
                        <i class="fas fa-info-circle mr-2"></i>Recipe Information
                    </div>
                    <div class="card-body p-3">
                        <div class="form-group row align-items-center mb-2">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label font-weight-bold" ID="lblcode" Text="Code:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNo" AutoPostBack="true" OnTextChanged="txtNo_TextChanged" CssClass="form-control form-control-sm" MaxLength="10" placeholder="Recipe code..." />
                                <asp:Label runat="server" ID="errNo" Visible="false" Style="color:red; font-size:0.8rem;"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group row align-items-center mb-2">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label font-weight-bold" ID="Label2" Text="Name:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtName" CssClass="form-control form-control-sm" MaxLength="15" placeholder="Recipe name..." />
                            </div>
                        </div>

                        <div class="form-group row align-items-center mb-2" runat="server" id="cost1" style="display:none;">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label font-weight-bold" ID="Label1" Text="Cost:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtCost" CssClass="form-control form-control-sm text-right" Enabled="false"/>
                            </div>
                        </div>

                        <div class="form-group row align-items-center mb-2">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label font-weight-bold" ID="Label3" Text="Remarks:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2" MaxLength="50" placeholder="Remarks..." />
                            </div>
                        </div>

                        <!-- Action Buttons -->
                        <div class="form-group row mt-3 pt-2 border-top mb-0">
                            <div class="col-sm-12 d-flex flex-wrap gap-2">
                                <button type="button" runat="server" class="btn btn-outline-primary btn-sm font-weight-bold mr-1" data-toggle="modal" id="btnaddcc" data-target="#ModalItem">
                                    <i class="fas fa-plus mr-1"></i>Add CC
                                </button>
                                <button type="button" runat="server" id="btnInsert" onserverclick="btnInsert_Click" class="btn btn-primary btn-sm font-weight-bold mr-1 btn-spinner">
                                    <i class="fas fa-save mr-1"></i>Add
                                </button>
                                <button type="button" runat="server" id="btnUpdate" onserverclick="btnUpdate_Click" class="btn btn-info btn-sm font-weight-bold mr-1 btn-spinner" Visible="false">
                                    <i class="fas fa-edit mr-1"></i>Update
                                </button>
                                <button type="button" runat="server" id="btnCancel" onserverclick="btnCancel_Click" class="btn btn-secondary btn-sm font-weight-bold" CausesValidation="false">
                                    <i class="fas fa-times mr-1"></i>Cancel
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Right: Cost Centre Table -->
            <div class="col-lg-6 col-md-12 mb-3">
                <div class="card card-custom h-100">
                    <div class="card-header">
                        <i class="fas fa-store-alt mr-2"></i>Cost Centre List
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <asp:ListView ID="lstViewCost" runat="server" OnItemCommand="lstViewCost_ItemCommand"
                                OnPagePropertiesChanging="OnPagecostChanging">
                                <LayoutTemplate>
                                    <table id="tblCostDetails" class="table table-bordered table-hover table-custom mb-0">
                                        <thead>
                                            <tr>
                                                <th>Cost Centre</th>
                                                <th style="width: 30%;">Cost</th>
                                                <th style="width: 70px;">Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("ccname")%>'></asp:Label>
                                        </td>
                                        <td class="text-right">
                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("Cost2", "{0:0,0.00}")%>'></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:ImageButton ToolTip="Delete record" OnClientClick="Confirm()"
                                                ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("ccID").ToString + ";" + Eval("ccname").ToString %>' runat="server"
                                                ImageUrl="~/images/delete.png" Style="vertical-align: middle;"></asp:ImageButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                    <div class="card-footer py-2 bg-light d-flex justify-content-between align-items-center">
                        <small class="text-muted font-weight-bold">Page:</small>
                        <asp:DataPager ID="lstViewCostPager1" runat="server" PagedControlID="lstViewCost" PageSize="5">
                            <Fields>
                                <asp:NumericPagerField ButtonType="Link" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </div>
            </div>
        </div>

        <!-- Lower Section: Recipe Detail (Items) -->
        <div class="card card-custom">
            <div class="card-header d-flex justify-content-between align-items-center">
                <span><i class="fas fa-list-ul mr-2"></i>Receipe Detail</span>
            </div>
            <div class="card-body p-3">
                <!-- Filter / Search Toolbar -->
                <div class="filter-card mb-3" id="Div1" runat="server">
                    <div class="form-row align-items-center">
                        <div class="col-auto">
                            <asp:Label for="txtsearch" runat="server" CssClass="text-white font-weight-bold mb-0">
                                <i class="fas fa-search mr-1"></i>Search Detail:
                            </asp:Label>
                        </div>
                        <div class="col-md-4 col-sm-6 my-1">
                            <asp:TextBox id="txtsearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search recipe detail..." type="search" aria-label="Search"></asp:TextBox>
                        </div>
                        <div class="col-auto my-1">
                            <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" class="btn btn-sm btn-light text-primary font-weight-bold shadow-sm" title="Search">
                                <i class="fas fa-search mr-1"></i>Search
                            </button>
                            <button type="button" runat="server" id="btnnew" onserverclick="btnnew_ServerClick" class="btn btn-sm btn-success font-weight-bold shadow-sm ml-1" title="New Detail">
                                <i class="fas fa-plus mr-1"></i><asp:Label runat="server" Text="New"></asp:Label>
                            </button>
                            <div id="dtExportContainer" class="d-inline-block ml-1"></div>
                        </div>
                    </div>
                </div>

                <!-- Table Detail -->
                <div class="table-responsive">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                        OnPagePropertiesChanging="OnPagePropertiesChanging">
                        <LayoutTemplate>
                            <table id="tblEmpDetails" class="table table-bordered table-hover table-custom mb-0">
                                <thead>
                                    <tr>
                                        <th>Tipe</th>
                                        <th>Name</th>
                                        <th style="width: 10%;">Qty</th>
                                        <th style="width: 10%;">Unit</th>
                                        <th style="width: 10%;">Loss factor</th>
                                        <th runat="server" id="thCostUnit3" visible="false">Cost/Unit3</th>
                                        <th runat="server" id="thTotalCost" visible="false">Total Cost</th>
                                        <th style="width: 80px;">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("TypeName")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("dtlName")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Qty")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("unit")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("Lossf")%>'></asp:Label>
                                </td>
                                <td runat="server" visible='<%# modulepriv() = 0 %>' class="text-right">
                                    <asp:Label ID="lblCostUnit3" runat="server" Text='<%# Eval("cost_per_unit3", "{0:0,0.00}") %>'></asp:Label>
                                </td>
                                <td runat="server" visible='<%# modulepriv() = 0 %>' class="text-right">
                                    <asp:Label ID="lblTotalCost" runat="server" Text='<%# Eval("total_cost", "{0:0,0.00}") %>'></asp:Label>
                                </td>
                                <td class="text-center" style="white-space: nowrap;">
                                    <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit record"
                                        CommandArgument='<%#Eval("id").ToString + ";" + Eval("tipe").ToString + ";" + Eval("unit").ToString%>' runat="server" ImageUrl="/images/edit.png" Style="vertical-align: middle; margin-right: 6px;" />
                                    <asp:ImageButton ToolTip="Delete record" OnClientClick="Confirm()"
                                        ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("id") %>' runat="server"
                                        ImageUrl="~/images/delete.png" Style="vertical-align: middle;"></asp:ImageButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <!-- Pagination Footer -->
            <div class="card-footer py-2 bg-light d-flex justify-content-between align-items-center">
                <small class="text-muted font-weight-bold">Page Navigation:</small>
                <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="15">
                    <Fields>
                        <asp:NumericPagerField ButtonType="Link" />
                    </Fields>
                </asp:DataPager>
            </div>
        </div>

        <!-- Modal CC Selection -->
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalStoreLabel" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                    <div class="modal-dialog modal-dialog-scrollable" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: #1e3a5f; color: white;">
                                <h5 class="modal-title font-weight-bold" id="ModalStoreLabel"><i class="fas fa-store mr-2"></i>Select Cost Centre</h5>
                                <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body p-3">
                                <div class="form-horizontal" id="Div2" runat="server">
                                    <div class="form-group row align-items-center mb-3">
                                        <asp:Label runat="server" AssociatedControlID="txtSearchcc" CssClass="col-sm-3 col-form-label font-weight-bold" Text="Cost Centre:"></asp:Label>
                                        <div class="col-sm-9">
                                            <div class="input-group input-group-sm">
                                                <asp:TextBox id="txtSearchcc" runat="server" CssClass="form-control" placeholder="Search cost centre..." type="search" aria-label="Search"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <button runat="server" id="btnSearchcc" onserverclick="Searchcc_Click" class="btn btn-primary" title="Search">
                                                        <i class="fas fa-search"></i>
                                                    </button>
                                                    <button runat="server" id="btnClearSearchcc" onserverclick="btnClearSearch_Click" class="btn btn-secondary" title="Clear Search">
                                                        <i class="fas fa-times"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <table class="table table-bordered table-sm table-hover">
                                        <thead style="background-color: #f1f4f8;">
                                            <tr>
                                                <th style="width: 40px; text-align: center;">Pick</th>
                                                <th scope="col" hidden="hidden">ID</th>
                                                <th scope="col">Cost Centre</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:ListView ID="LsvStk" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="text-center">
                                                            <asp:CheckBox ID="CheckBox" runat="server"></asp:CheckBox>
                                                        </td>
                                                        <td hidden="hidden">
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("ccname")%>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer py-2">
                                <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                                <button type="button" class="btn btn-primary btn-sm font-weight-bold" runat="server" onserverclick="RadButton1_ServerClick" id="RadButton1">
                                    <i class="fas fa-check mr-1"></i>Add Selected
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <script type="text/javascript">
        function OnKeyPress(e) {
            if (e.keyCode == "27") {
                $find("ModalItem").hide();
            }
        }

        function openModalcc() {
            $('#ModalItem').modal({ show: true });
        }

        // =============================================
        // DataTables + Export Excel untuk tblEmpDetails
        // =============================================
        $(document).ready(function () {
            if ($('#tblEmpDetails').length > 0) {
                var notice = document.getElementById('recipeExportError');
                if (!$.fn.DataTable || !$.fn.dataTable.Buttons || !window.JSZip ||
                    !$.fn.dataTable.ext.buttons.excelHtml5) {
                    notice.hidden = false;
                    return;
                }
                if ($.fn.dataTable.isDataTable('#tblEmpDetails')) return;
                try {
                var totalCols = $('#tblEmpDetails thead tr th').length;
                var actionColIdx = totalCols - 1;

                var table = $('#tblEmpDetails').DataTable({
                    paging: false,
                    searching: false,
                    ordering: false,
                    info: false,
                    bAutoWidth: false,
                    dom: 'Bt',
                    buttons: [
                        {
                            extend: 'excelHtml5',
                            text: '<i class="fas fa-file-excel mr-1"></i> Export Excel',
                            className: 'btn btn-success btn-sm font-weight-bold shadow-sm',
                            title: 'Receipe Detail',
                            filename: 'ReceipeDetail_' + new Date().toISOString().slice(0,10),
                            exportOptions: {
                                columns: function (idx, data, node) {
                                    return idx !== actionColIdx;
                                }
                            },
                            customize: function (xlsx) {
                                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                $('row:first c', sheet).attr('s', '2');
                            }
                        }
                    ]
                });

                table.buttons().container().appendTo('#dtExportContainer');
                notice.hidden = false;
                // The Excel plugin can decline availability even when its script loaded.
                if ($('#dtExportContainer button').length > 0) notice.hidden = true;
                } catch (error) {
                    notice.hidden = false;
                }
            }
        });
    </script>
</asp:Content>
