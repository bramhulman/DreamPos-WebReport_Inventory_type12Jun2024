<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_StkGroup.aspx.vb" Inherits="DreamPos_WebReport.Pages_StkGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/inventory-mobile.css" />
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
        }
        .filter-card {
            background: linear-gradient(135deg, #1e3a5f 0%, #2c3e50 100%);
            border-radius: 6px;
            padding: 10px 16px;
            margin-bottom: 12px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.08);
        }
        .pagination-container {
            padding: 10px 15px;
            background-color: #fafbfc;
            border-top: 1px solid #e9ecef;
        }
        .pagination-container a, .pagination-container span {
            padding: 4px 10px;
            margin: 0 2px;
            border-radius: 4px;
            font-size: 0.85rem;
        }
        .pagination-container span {
            background-color: #1e3a5f;
            color: #fff;
            font-weight: bold;
        }
        .pagination-container a {
            background-color: #fff;
            color: #1e3a5f;
            border: 1px solid #dee2e6;
            text-decoration: none;
        }
        .pagination-container a:hover {
            background-color: #e2e6ea;
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('.btnstyle').click(function () {
                $('#spinner').show();
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
                        <i class="fas fa-layer-group text-primary mr-2"></i>Stock Group
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item active">Stock Group</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <!-- Filter / Toolbar -->
        <div class="filter-card" id="Div1" runat="server">
            <div class="form-row align-items-center">
                <div class="col-auto">
                    <asp:Label for="txtsearch" runat="server" CssClass="text-white font-weight-bold mb-0">
                        <i class="fas fa-search mr-1"></i>Search Name:
                    </asp:Label>
                </div>
                <div class="col-md-4 col-sm-6 my-1">
                    <asp:TextBox id="txtsearch" runat="server" CssClass="form-control form-control-sm"
                        placeholder="Search stock group..." type="search" aria-label="Search"></asp:TextBox>
                </div>
                <div class="col-auto my-1">
                    <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-sm btn-light text-primary font-weight-bold shadow-sm"
                        title="Search">
                        <i class="fas fa-search mr-1"></i>Search
                    </button>
                    <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click"
                        class="btn btn-sm btn-success font-weight-bold shadow-sm ml-1" title="New Stock Group">
                        <i class="fas fa-plus mr-1"></i><asp:Label runat="server" Text="New"></asp:Label>
                    </button>
                </div>
            </div>
        </div>

        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
        </span>

        <!-- Table Card -->
        <div class="card card-outline card-primary shadow-sm">
            <div class="card-body p-0">
                <div class="form-horizontal w-100">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                        OnPagePropertiesChanging="OnPagePropertiesChanging">
                        <LayoutTemplate>
                            <div class="table-responsive">
                                <table id="tblEmpDetails" class="table table-bordered table-hover table-custom mb-0">
                                    <thead>
                                        <tr>
                                            <th style="width: 15%;">Code</th>
                                            <th>Name</th>
                                            <th>Cust Name</th>
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
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Code")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Custname")%>'></asp:Label>
                                </td>
                                <td class="text-center" style="white-space: nowrap;">
                                    <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit record"
                                        CommandArgument='<%#Eval("id") %>' runat="server"
                                        ImageUrl="/images/edit.png" Style="vertical-align: middle; margin-right: 6px;" />
                                    <asp:ImageButton ToolTip="Delete record" OnClientClick="Confirm()"
                                        ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("id") %>'
                                        runat="server" ImageUrl="~/images/delete.png" Style="vertical-align: middle;"></asp:ImageButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <!-- Pagination Footer -->
            <div class="pagination-container d-flex justify-content-between align-items-center">
                <div class="font-weight-bold text-muted small">
                    <i class="fas fa-layer-group mr-1"></i>Halaman Navigasi
                </div>
                <div>
                    <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="15">
                        <Fields>
                            <asp:NumericPagerField ButtonType="Link" NumericButtonCssClass="btn-page" CurrentPageLabelCssClass="active-page" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </div>
        </div>
    </div>
</asp:Content>