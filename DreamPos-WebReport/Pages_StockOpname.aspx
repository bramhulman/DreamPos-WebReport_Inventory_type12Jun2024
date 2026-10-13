<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_StockOpname.aspx.vb" Inherits="DreamPos_WebReport.Pages_StockOpname" %>
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
            background: #ffffff;
            border-radius: 6px;
            padding: 8px 14px; margin-bottom: 10px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            border: 1px solid #dee2e6;
        }
        .filter-card label { font-size: 0.72rem; margin-bottom: 2px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.3px; color: #1e3a5f; }
        .filter-card .form-control,
        .filter-card .btn { height: 31px; font-size: 0.83rem; }

        /* ── Table Styling ── */
        .card-opname { border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.07); }
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

        /* variance color */
        .var-positive { color: #c0392b; font-weight: 700; }
        .var-negative { color: #e67e22; font-weight: 700; }
        .var-zero     { color: #27ae60; font-weight: 700; }
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
                        <i class="fas fa-clipboard-list text-primary mr-2"></i>
                        <asp:Label ID="lblTitle" runat="server" Text="Stock Opname"></asp:Label>
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item active">Stock Opname</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">

        <%-- Failure Message --%>
        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
        </span>

        <%-- Filter Bar --%>
        <div class="filter-card">
            <div id="formload" runat="server">
                <div class="row align-items-end">

                    <%-- Date --%>
                    <div class="col-12 col-sm-6 col-md-2 mb-2 mb-md-0">
                        <label class="small mb-1">
                            <i class="far fa-calendar-alt mr-1"></i>Tanggal:
                        </label>
                        <div class="input-group input-group-sm">
                            <asp:TextBox style="text-align:center;" runat="server" id="txtDate" CssClass="form-control font-weight-bold" />
                            <div class="input-group-append">
                                <span class="input-group-text bg-light px-2">
                                    <i class="fas fa-calendar text-primary"></i>
                                </span>
                            </div>
                            <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate" Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                        </div>
                    </div>

                    <%-- Search Name --%>
                    <div class="col-12 col-sm-6 col-md-3 mb-2 mb-md-0">
                        <label class="small mb-1">
                            <i class="fas fa-search mr-1"></i>Cari Nama Barang:
                        </label>
                        <div class="input-group input-group-sm">
                            <asp:TextBox id="txtsearch" runat="server" CssClass="form-control" placeholder="Ketik nama barang..." type="search" />
                            <div class="input-group-append">
                                <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-outline-secondary" title="Search">
                                    <i class="fas fa-search"></i>
                                </button>
                            </div>
                        </div>
                    </div>

                    <%-- Cost Centre --%>
                    <div class="col-12 col-sm-6 col-md-3 mb-2 mb-md-0">
                        <label class="small mb-1">
                            <i class="fas fa-store mr-1"></i>Cost Centre:
                        </label>
                        <asp:DropDownList ID="ddlOutlet" runat="server" CssClass="form-control form-control-sm font-weight-bold"
                            AppendDataBoundItems="True" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged"
                            EnableViewState="True" ViewStateMode="Enabled"
                            style="height:31px; font-size:0.83rem; padding:2px 8px;">
                            <asp:ListItem Value="0">---- Pilih Cost Centre ----</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <%-- Action Buttons --%>
                    <div class="col-12 col-sm-6 col-md-auto mb-2 mb-md-0">
                        <label class="small mb-1 d-block">&nbsp;</label>
                        <div class="d-flex flex-wrap" style="gap:6px;">
                            <button type="button" runat="server" id="btnnew" onserverclick="btnnew_ServerClick"
                                class="btn btn-sm btn-outline-primary font-weight-bold" title="Adjustment" visible="False">
                                <i class="fas fa-sliders-h mr-1"></i>Adjustment
                            </button>
                            <button type="button" runat="server" id="btnImport" onserverclick="btnImport_Click"
                                class="btn btn-sm btn-warning font-weight-bold" title="Import Excel">
                                <i class="fas fa-file-import mr-1"></i>Import Excel
                            </button>
                            <button type="button" runat="server" id="btnExport" onserverclick="btnExport_Click"
                                class="btn btn-sm btn-success font-weight-bold" title="Export/Print">
                                <i class="fas fa-file-export mr-1"></i>Export
                            </button>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <%-- Data Table Card --%>
        <div class="card card-opname">
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                        OnPagePropertiesChanging="OnPagePropertiesChanging">
                        <LayoutTemplate>
                            <table id="tblEmpDetails" class="table table-bordered table-hover table-opname mb-0">
                                <thead>
                                    <tr>
                                        <th>Nama Barang</th>
                                        <th>Cost Centre</th>
                                        <th>Tanggal</th>
                                        <th>Tipe</th>
                                        <th class="text-right">Total Qty</th>
                                        <th class="text-right">Stock Opname</th>
                                        <th class="text-right">Variance Qty</th>
                                        <th class="text-center">Action</th>
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
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("stkname")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("ccname")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("createddate")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("type")%>'></asp:Label>
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("ttlqty", "{0:0.000}") + " " + Eval("Unit2").ToString %>'></asp:Label>
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("closeqty", "{0:#,0}")%>'></asp:Label>
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="Label5" runat="server"
                                        Text='<%# Eval("varianceqty", "{0:#,0}")%>'
                                        CssClass='<%# If(CDec(If(Eval("varianceqty"), 0)) > 0, "var-positive", If(CDec(If(Eval("varianceqty"), 0)) < 0, "var-negative", "var-zero")) %>'></asp:Label>
                                </td>
                                <td style="text-align:center; width:8%;">
                                    <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit"
                                        CommandArgument='<%#Eval("stkid").ToString + ";" + Eval("Ccid").ToString%>'
                                        runat="server" ImageUrl="/images/edit.png" Visible="false" />
                                    <asp:LinkButton ID="imgBtnDelete" ToolTip="Batalkan opname" OnClientClick="return confirm('Batalkan proses opname untuk barang ini?');" CausesValidation="false" CssClass="btn btn-sm btn-outline-danger"
                                        CommandName="Del"
                                        CommandArgument='<%# Eval("stkid").ToString() & ";" & Eval("Ccid").ToString() & ";" & CDate(Eval("invdate")).ToString("yyyy-MM-dd") & ";" & If(Eval("type").ToString() = "Current", "11", "12") %>'
                                        runat="server"><i class="fas fa-trash-alt" aria-hidden="true"></i></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div class="card-footer py-2 px-3 d-flex align-items-center" style="background:#f8f9fa;">
                <asp:Label runat="server" Font-Bold="True" CssClass="mr-2 text-muted" style="font-size:0.82rem;">Halaman:</asp:Label>
                <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="15">
                    <Fields>
                        <asp:NumericPagerField ButtonType="Link" />
                    </Fields>
                </asp:DataPager>
            </div>
        </div>

    </div><%-- /.container-fluid --%>

    <script type="text/javascript">
        function showLoading() {
            $('#spinner').show();
        }
        function hideLoading() {
            $('#spinner').hide();
        }
        function waitForDownload(tokenName) {
            showLoading();
            var poller = setInterval(function () {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var c = cookies[i].trim();
                    if (c.indexOf(tokenName + '=') === 0) {
                        document.cookie = tokenName + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
                        clearInterval(poller);
                        hideLoading();
                        return;
                    }
                }
            }, 500);
            setTimeout(function () { clearInterval(poller); hideLoading(); }, 30000); // 30s timeout fallback
        }

        $(document).ready(function () {
            // Show spinner on standard server-side clicks
            $('button[id$="btnsrc"], button[id$="btnnew"], button[id$="btnImport"]').click(function () {
                showLoading();
            });

            // Special handling for Export button (File Download)
            var btnExp = document.querySelector('button[id$="btnExport"]');
            if (btnExp) {
                btnExp.addEventListener('click', function(e) {
                    waitForDownload('fileDownloadToken');
                }, true);
            }

            // Row hover click to edit
            $('#tblEmpDetails tbody tr').css('cursor', 'pointer');
        });
    </script>
</asp:Content>
