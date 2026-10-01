<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteinventoryHome.Master" CodeBehind="Pages_ExprtImprt.aspx.vb" Inherits="DreamPos_WebReport.Pages_ExprtImprt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">

        /* ── Loading overlay ── */
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'flex';
        }
        function hideLoading() {
            document.getElementById('loadingOverlay').style.display = 'none';
        }

        /* ── Poll cookie until download token detected, then hide spinner ── */
        function waitForDownload(tokenName) {
            showLoading();
            var poller = setInterval(function () {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var c = cookies[i].trim();
                    if (c.indexOf(tokenName + '=') === 0) {
                        // clear the cookie
                        document.cookie = tokenName + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
                        clearInterval(poller);
                        hideLoading();
                        return;
                    }
                }
            }, 500);
            // Safety fallback: hide after 30 s
            setTimeout(function () { clearInterval(poller); hideLoading(); }, 30000);
        }

        $(document).ready(function () {

            /* ── Import button: show loading (page will reload) ── */
            document.getElementById('<%= btnImport.ClientID %>').addEventListener('click', function () {
                showLoading();
            });

            /* ── Sample/Export button: show loading, poll for download cookie ── */
            var btnExp = document.getElementById('<%= btnExport.ClientID %>');
            if (btnExp) {
                btnExp.addEventListener('click', function (e) {
                    var txtEx = document.getElementById('<%= txtEx.ClientID %>');
                    if (txtEx && txtEx.value.trim() === '') {
                        e.preventDefault();
                        e.stopImmediatePropagation();
                        var fname = prompt('Masukkan nama file sample (tanpa .xlsx):', '');
                        if (fname !== null && fname.trim() !== '') {
                            txtEx.value = fname.trim();
                            setTimeout(function () { btnExp.click(); }, 50);
                        }
                        return false;
                    }
                    // nama sudah diisi -> mulai polling download token
                    waitForDownload('fileDownloadToken');
                }, true);
            }
        });
    </script>

    <!-- Loading Overlay -->
    <div id="loadingOverlay" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%;
         background:rgba(0,0,0,0.35); z-index:9999; justify-content:center; align-items:center;">
        <div style="background:#fff; border-radius:10px; padding:32px 40px; text-align:center;
                    box-shadow:0 4px 24px rgba(0,0,0,0.18); min-width:200px;">
            <div style="margin-bottom:14px;">
                <img src="/images/loader.gif" alt="loading" style="width:48px; height:48px;" />
            </div>
            <div style="font-family:Arial,sans-serif; font-size:14px; color:#444; font-weight:600;">
                Mohon tunggu&hellip;
            </div>
            <div style="font-family:Arial,sans-serif; font-size:12px; color:#888; margin-top:4px;">
                Sedang memproses data&hellip;
            </div>
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Export / Import StockOpname</h1>
          </div>
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Export / Import StockOpname</li>
            </ol>
          </div>
        </div>
      </div>
    </div>

    <!-- Main Content -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card card-primary card-outline">

                    <div class="card-header">
                        <h3 class="card-title">
                            <i class="fas fa-file-excel mr-1" style="color:#1D6F42;"></i>
                            Import / Export StockOpname
                        </h3>
                    </div>

                    <div class="card-body">

                        <!-- File Upload -->
                        <div class="form-group row">
                            <asp:Label runat="server" ID="lblfile" Text="File Upload:"
                                CssClass="col-auto col-form-label font-weight-bold text-nowrap" style="width: 140px;" />
                            <div class="col" style="max-width: 360px;">
                                <asp:FileUpload ID="FileUpLoad1" runat="server"
                                    Class="form-control-file mt-1"
                                    style="font-size:13px;" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="FileUpLoad1"
                                    ErrorMessage="Pilih file Excel terlebih dahulu."
                                    ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>

                        <!-- File Name -->
                        <div class="form-group row">
                            <asp:Label runat="server" ID="Label2" Text="Nama File:"
                                CssClass="col-auto col-form-label font-weight-bold text-nowrap" style="width: 140px;" />
                            <div class="col" style="max-width: 360px;">
                                <asp:TextBox runat="server" ID="txtEx" Class="form-control" />
                                <small class="form-text text-muted">Tanpa ekstensi .xlsx</small>
                            </div>
                        </div>

                        <!-- Date -->
                        <div class="form-group row">
                            <asp:Label runat="server" ID="Label1" Text="Tanggal:"
                                CssClass="col-auto col-form-label font-weight-bold text-nowrap" style="width: 140px;" />
                            <div class="col" style="max-width: 360px;">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtDate" Class="form-control"
                                        style="text-align:center;" AutoPostBack="false" />
                                    <div class="input-group-append">
                                        <span class="input-group-text">
                                            <i class="fas fa-calendar-alt"></i>
                                        </span>
                                    </div>
                                </div>
                                <ajaxToolkit:CalendarExtender ID="cleFrom" runat="server"
                                    TargetControlID="txtDate" Format="dd MMM yyyy" />
                            </div>
                        </div>

                        <!-- Import For -->
                        <div class="form-group row">
                            <asp:Label runat="server" ID="Label4" Text="Import untuk:"
                                CssClass="col-auto col-form-label font-weight-bold text-nowrap" style="width: 140px;" />
                            <div class="col" style="max-width: 360px;">
                                <asp:DropDownList ID="ddladjustnow" runat="server"
                                    AppendDataBoundItems="True" CssClass="form-control"
                                    AutoPostBack="false" EnableViewState="True" ViewStateMode="Enabled">
                                    <%--<asp:ListItem Value="1">Current Stock</asp:ListItem>--%>
                                    <asp:ListItem Value="2">Opening Stock</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- CC Name -->
                        <div class="form-group row">
                            <asp:Label runat="server" ID="Label3" Text="Cost Centre:"
                                CssClass="col-auto col-form-label font-weight-bold text-nowrap" style="width: 140px;" />
                            <div class="col" style="max-width: 360px;">
                                <asp:DropDownList ID="ddlCCName" runat="server"
                                    AppendDataBoundItems="false" CssClass="form-control"
                                    AutoPostBack="false" EnableViewState="True" ViewStateMode="Enabled" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="ddlCCName"
                                    ErrorMessage="Cost Centre harus dipilih."
                                    ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>

                    </div><!-- /.card-body -->

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnImport"
                            UseSubmitBehavior="true" OnClick="btnImport_Click"
                            Text="Import" CssClass="btn btn-success"
                            Style="font-weight:600; width:108px; margin-right:6px;" />
                        <asp:Button runat="server" ID="btnExport"
                            OnClick="btnExport_Click"
                            Text="Sample" CssClass="btn btn-primary"
                            Style="font-weight:600; width:108px; margin-right:6px;"
                            CausesValidation="false" />
                        <asp:Button runat="server" ID="btnCancel"
                            OnClick="btnCancel_Click"
                            Text="Cancel" CssClass="btn btn-secondary"
                            Style="font-weight:600; width:108px;"
                            CausesValidation="false" />
                    </div><!-- /.card-footer -->

                </div><!-- /.card -->
            </div><!-- /.col -->
        </div><!-- /.row -->

        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server" />
        </span>
    </div><!-- /.container-fluid -->

</asp:Content>
