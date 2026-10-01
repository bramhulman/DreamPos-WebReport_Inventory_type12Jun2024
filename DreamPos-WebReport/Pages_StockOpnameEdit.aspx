<%@ Page Title="Stock Opname Edit" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
  CodeBehind="Pages_StockOpnameEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_StockOpnameEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* ── Modern Loading Overlay ── */
        .modal-loading {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background-color: rgba(255,255,255,0.75);
            z-index: 9999; display: none; justify-content: center; align-items: center;
        }
        .loading-box {
            background: #ffffff; padding: 24px 32px; border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.15); text-align: center;
        }
        .loading-box img { width: 50px; height: 50px; margin-bottom: 12px; }
        .loading-box h6 { font-weight: 600; color: #1e3a5f; margin: 0; }
        .loading-box small { color: #6c757d; }

        /* ── Form Card Styling ── */
        .edit-card { border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.06); border: none; }
        .edit-card-header { background: #1e3a5f; color: #ffffff; border-radius: 8px 8px 0 0; padding: 12px 20px; }
        .edit-card-body { background: #fdfdfd; padding: 20px 24px; }
        .form-label { font-weight: 600; color: #34495e; font-size: 0.88rem; }
        .input-group-text.unit-label { background: #e9ecef; font-weight: bold; font-size: 0.85rem; color: #495057; width: 70px; justify-content: center; }
        
        .form-control:read-only { background-color: #f4f6f9; cursor: not-allowed; }
    </style>
    <script type="text/javascript">
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'flex';
        }
        function DisableButton() {
            var btnInsert = document.getElementById("<%=btnInsert.ClientID %>");
            if(btnInsert) btnInsert.disabled = true;
        }
        window.onbeforeunload = DisableButton;
        
        $(document).ready(function () {
            $('.btn-loading-trigger').click(function () {
                if (typeof Page_IsValid !== 'undefined' && !Page_IsValid) return;
                showLoading();
            });
            $('form').on("submit", function () {
                showLoading();
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server"></asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <!-- Loading Overlay -->
    <div id="loadingOverlay" class="modal-loading">
        <div class="loading-box">
            <img src="/images/loader.gif" alt="Loading" />
            <h6>Processing...</h6>
            <small>Please wait a moment</small>
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-2">
        <div class="container-fluid">
            <div class="row align-items-center mb-2">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-edit text-primary mr-2"></i>
                        <asp:Label runat="server" ID="lblh1">Stock Opname Edit</asp:Label>
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0" style="background:transparent; padding:0;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_StockOpname.aspx">Stock Opname</a></li>
                        <li class="breadcrumb-item active"><asp:Label runat="server" ID="lblli1">Edit</asp:Label></li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <!-- Main Content -->
    <div class="container-fluid">
        <div class="row justify-content-center">
            <div class="col-lg-8 col-xl-7">
                <div class="card edit-card mb-4">
                    <div class="edit-card-header d-flex align-items-center">
                        <i class="fas fa-clipboard-check mr-2"></i>
                        <h6 class="m-0 font-weight-bold">Form Adjustment Stock</h6>
                    </div>
                    
                    <div class="edit-card-body">
                        
                        <%-- Cost Centre & Item Row --%>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Cost Centre</label>
                                <asp:DropDownList ID="DdlTo" runat="server" AppendDataBoundItems="True" CssClass="form-control" AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                                    <asp:ListItem Value="0">---- Select Cost Centre ----</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Item / Barang</label>
                                <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="True" CssClass="form-control" AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                                    <asp:ListItem Value="0">---- Select Item ----</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <%-- Type & Hidden Stkno --%>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Type (For)</label>
                                <asp:DropDownList ID="ddladjustnow" runat="server" AppendDataBoundItems="True" CssClass="form-control" AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                                    <asp:ListItem Value="2">Opening Stock</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 mb-3" style="display:none;">
                                <label class="form-label">Stkno</label>
                                <asp:TextBox runat="server" ID="txtstkNo" CssClass="form-control text-center" />
                            </div>
                        </div>

                        <hr class="mt-1 mb-3" />

                        <%-- Stock Quantities Row 1 --%>
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label text-info">In Stock (System)</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtinstock" CssClass="form-control text-right font-weight-bold" ReadOnly="true" AutoPostBack="True" />
                                    <div class="input-group-append">
                                        <asp:Label runat="server" ID="lblunit22" CssClass="input-group-text unit-label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Opname Unit 1</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtUnit1" CssClass="form-control text-right" OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True" />
                                    <div class="input-group-append">
                                        <asp:Label runat="server" ID="lblUnit1" Text="Unit1" CssClass="input-group-text unit-label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Opname Unit 2</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtUnit2" CssClass="form-control text-right" OnTextChanged="txtUnit2_TextChanged" AutoPostBack="True" />
                                    <div class="input-group-append">
                                        <asp:Label runat="server" ID="lblUnit2" Text="Unit2" CssClass="input-group-text unit-label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Stock Quantities Row 2 --%>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Conversion Factor</label>
                                <asp:TextBox runat="server" ID="txtLosF" CssClass="form-control text-right" Enabled="false" Text="1" AutoPostBack="True" />
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label text-success">Total Actual Qty (U2)</label>
                                <asp:TextBox runat="server" ID="txtTotalStk" CssClass="form-control text-right font-weight-bold" Enabled="false" Text="1" AutoPostBack="True" />
                            </div>
                        </div>

                        <hr class="mt-1 mb-3" />

                        <%-- Costs --%>
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <label class="form-label">Cost Unit 1</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtCostu1" CssClass="form-control text-right" Text="1" AutoPostBack="True" />
                                    <div class="input-group-append">
                                        <asp:Label runat="server" ID="lblcostu1" Text="Unit1" CssClass="input-group-text unit-label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 mb-4">
                                <label class="form-label">Cost Unit 2</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtCost" CssClass="form-control text-right" Text="1" AutoPostBack="True" />
                                    <div class="input-group-append">
                                        <asp:Label runat="server" ID="lblcostu2" Text="Unit2" CssClass="input-group-text unit-label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Action Buttons --%>
                        <div class="d-flex justify-content-end align-items-center mt-2 pt-3 border-top">
                            <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-secondary mr-2" style="width: 110px; font-weight:600;" CausesValidation="false" />
                            <button runat="server" id="btnInsert" onserverclick="btnInsert_Click" class="btn btn-primary btn-loading-trigger" style="width: 110px; font-weight:600;">
                                <i class="fas fa-save mr-1"></i> Add
                            </button>
                            <button runat="server" id="btnUpdate" onserverclick="btnUpdate_Click" class="btn btn-success btn-loading-trigger" style="width: 110px; font-weight:600;" visible="false">
                                <i class="fas fa-check mr-1"></i> Update
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>