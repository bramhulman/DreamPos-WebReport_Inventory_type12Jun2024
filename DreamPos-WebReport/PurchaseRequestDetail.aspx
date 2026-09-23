<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="PurchaseRequestDetail.aspx.vb" Inherits="DreamPos_WebReport.PurchaseRequestDetail" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
                <!-- Select2 CSS from CDN -->
                <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
                <style type="text/css">
                    .select2-container .select2-selection--single {
                        height: 38px !important;
                        border: 1px solid #ced4da !important;
                        border-radius: 0.25rem !important;
                    }
                    .select2-container--default .select2-selection--single .select2-selection__rendered {
                        line-height: 36px !important;
                        color: #495057 !important;
                        padding-left: 12px !important;
                    }
                    .select2-container--default .select2-selection--single .select2-selection__arrow {
                        height: 36px !important;
                    }
                    .select2-dropdown {
                        border: 1px solid #ced4da !important;
                        box-shadow: 0 4px 6px rgba(0,0,0,0.1) !important;
                        border-radius: 0.25rem !important;
                    }
                </style>
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>
                <style type="text/css">
                    .modal {
                        position: fixed;
                        top: 0;
                        left: 0;
                        background-color: black;
                        z-index: 99;
                        opacity: 0.8;
                        filter: alpha(opacity=80);
                        -moz-opacity: 0.8;
                        min-height: 100%;
                        width: 100%;
                    }

                    .loading {
                        position: fixed;
                        top: 50% !important;
                        left: 50% !important;
                        transform: translate(-50%, -50%);
                        z-index: 9999;
                        background: #ffffff;
                        padding: 30px 40px;
                        border-radius: 12px;
                        box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);
                        text-align: center;
                        min-width: 300px;
                        display: none;
                        border: none;
                    }

                    .input-group>.form-control {
                        width: 85%;
                    }

                    .card-body {
                        padding: 3%;
                    }

                    .prd-badge-new {
                        background-color: #17a2b8;
                        color: white;
                        padding: 3px 10px;
                        border-radius: 4px;
                        font-size: 0.8rem;
                        font-weight: 600;
                    }

                    .prd-badge-edit {
                        background-color: #ffc107;
                        color: #333;
                        padding: 3px 10px;
                        border-radius: 4px;
                        font-size: 0.8rem;
                        font-weight: 600;
                    }

                    .prd-badge-approved {
                        background-color: #28a745;
                        color: white;
                        padding: 3px 10px;
                        border-radius: 4px;
                        font-size: 0.8rem;
                        font-weight: 600;
                    }

                    .prd-badge-rejected {
                        background-color: #dc3545;
                        color: white;
                        padding: 3px 10px;
                        border-radius: 4px;
                        font-size: 0.8rem;
                        font-weight: 600;
                    }

                    .grid-header-prd th {
                        background-color: #2c3e50 !important;
                        color: white !important;
                    }

                    .btn-action-row {
                        margin-right: 5px;
                    }

                    .qty-input {
                        width: 80px;
                        text-align: center;
                    }
                </style>
                <script type="text/javascript"
                    src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                <script type="text/javascript">
                    var progressTimeout;
                    function ShowProgress() {
                        if (progressTimeout) {
                            clearTimeout(progressTimeout);
                        }
                        progressTimeout = setTimeout(function () {
                            if ($('body > .modal').length === 0) {
                                var modal = $('<div />');
                                modal.addClass("modal");
                                $('body').append(modal);
                            }
                            var loading = $(".loading");
                            loading.show();
                        }, 200);
                    }
                    function HideProgress() {
                        if (progressTimeout) {
                            clearTimeout(progressTimeout);
                            progressTimeout = null;
                        }
                        $(".loading").hide();
                        $('body > .modal').remove();
                    }
                    $('form').live("submit", function () {
                        ShowProgress();
                    });
                    
                    // Automatically hide progress panel when async AJAX request completes
                    if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                            HideProgress();
                        });
                    }

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
                <script type="text/javascript">
                    function DisableButton() {
                        document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                    }
                    window.onbeforeunload = DisableButton;

                    function onCheckApprove(chk) {
                        var row = chk.closest('tr');
                        // Find textbox that contains txtEditAppQty in its ID
                        var txt = row.querySelector('input[id*="txtEditAppQty"]');
                        if (txt) {
                            txt.disabled = !chk.checked;
                            if (!chk.checked) {
                                // optional: set to 0 if rejected
                                // txt.value = '0';
                            }
                        }
                    }
                </script>
                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Purchase Request</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item"><a href="PurchaseRequest.aspx">Purchase Request</a></li>
                                    <li class="breadcrumb-item active">Detail</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color:white;">

                    <!-- /.content-header -->
                        <div id="spinner" class="loading">
                            <div style="font-size: 3rem; color: #007bff; margin-bottom: 15px;">
                                <i class="fas fa-circle-notch fa-spin"></i>
                            </div>
                            <div
                                style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 1.25rem; color: #333; font-weight: 600; margin-bottom: 5px;">
                                Processing...
                            </div>
                            <div style="font-family: 'Segoe UI', sans-serif; font-size: 0.9rem; color: #6c757d;">
                                Please wait a moment.
                            </div>
                        </div>
                        <div class="card shadow-sm">
                            <div class="card-header" style="background-color:#2c3e50; color:white;">
                                <h3 class="card-title" style="color:white;">
                                    <i class="fas fa-file-alt mr-2"></i>Purchase Request Detail
                                </h3>
                                <div class="card-tools">
                                    <asp:Label ID="lblStatusBadge" runat="server" Text="" CssClass="prd-badge-new"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-horizontal" id="formload">
                                    <div class="card-body">
                                        <%-- PR Number --%>
                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap font-weight-bold"
                                                ID="lblorderno" Text="PR No: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fas fa-hashtag"></i></span>
                                                </div>
                                                <asp:TextBox runat="server" ID="txtID" Class="form-control"
                                                    MaxLength="30" enabled="false"
                                                    placeholder="Auto Generated"
                                                    Style="padding: 0.375rem 0.75rem; background-color:#f8f9fa; font-weight:600; color:#2c3e50;" />
                                            </div>
                                        </div>

                                        <%-- Warehouse / HO (Destination) - hidden --%>

                                        <%-- Cost Centre (Requesting Outlet) --%>
                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap font-weight-bold"
                                                ID="Label1" Text="Cost Centre: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fas fa-store"></i></span>
                                                </div>
                                                <asp:DropDownList ID="ddlFrom"
                                                    OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged" runat="server"
                                                    CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                                                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                    ControlToValidate="ddlFrom" ErrorMessage="Please select Cost Centre"
                                                    ForeColor="Red" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <%-- Request Date (Create Date) --%>
                                        <div class="form-group row col-md-12" runat="server" id="Createdate"
                                            visible="true">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap font-weight-bold"
                                                ID="Label4" Text="Request Date: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="far fa-calendar-alt"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox style="text-align:center; width:auto"
                                                        runat="server" ID="txtcreatedate" Class="form-control"
                                                        AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="clecreatedate" runat="server"
                                                        TargetControlID="txtcreatedate" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- required Date --%>
                                        <div class="form-group row col-md-12" runat="server" id="requireddate">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap font-weight-bold"
                                                ID="Label3" Text="Required Date: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="far fa-calendar-alt"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox style="text-align:center; width:auto"
                                                        runat="server" ID="txtRequiredDate" Class="form-control"
                                                        AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="cle" runat="server"
                                                        TargetControlID="txtRequiredDate" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- Receive Date --%>
                                        <div class="form-group row col-md-12" runat="server" id="appdate"
                                            visible="false">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap font-weight-bold"
                                                ID="Label5" Text="Approved Date: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="far fa-calendar-alt"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox style="text-align:center; width:auto"
                                                        runat="server" ID="txtappdate" Class="form-control"
                                                        AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="cleappdate" runat="server"
                                                        TargetControlID="txtappdate" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- Notes / Remarks --%>
                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap font-weight-bold"
                                                ID="lblNotes" Text="Remarks: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <asp:TextBox style="text-align:left;" runat="server" ID="txtNotes"
                                                    Class="form-control" width="400" TextMode="MultiLine" MaxLength="95"
                                                    Height="80" placeholder="Enter notes or remarks for this purchase request..." />
                                            </div>
                                        </div>

                                        <%-- Action Buttons --%>
                                        <div class="form-group row">
                                            <div Class="col-md-2"></div>
                                            <div class="col-md-offset-2 col-md-10">
                                                <button type="button" runat="server" class="btn btn-warning btn-action-row"
                                                    id="btnedit" onserverclick="btnedit_ServerClick"
                                                    style="font-weight: 500; min-width: 100px;">
                                                    <i class="fas fa-edit mr-1"></i> Edit Qty
                                                </button>

                                                <button type="button" runat="server" class="btn btn-info btn-action-row"
                                                    data-toggle="modal" id="btnshow" data-target="#ModalItem"
                                                    style="font-weight: 500; min-width: 100px; display: none !important;">
                                                    <i class="fas fa-plus mr-1"></i> Add Item
                                                 </button>

                                                <button type="button" runat="server" class="btn btn-secondary btn-action-row"
                                                    id="btnSetAll" onserverclick="btnSetAll_Click"
                                                    style="font-weight: 500; min-width: 100px;" visible="false">
                                                    <i class="fas fa-sliders-h mr-1"></i> Set Stock Limit
                                                </button>

                                                <button type="submit" runat="server" class="btn btn-success btn-action-row"
                                                    id="btnSave" onserverclick="btnSave_Click"
                                                    style="font-weight: 500; min-width: 100px;">
                                                    <i class="fas fa-save mr-1"></i> Save PR
                                                </button>

                                                <button type="submit" runat="server" id="btnForceSave" onserverclick="btnForceSave_ServerClick" style="display:none;"></button>

                                                <button type="button" runat="server" class="btn btn-default btn-action-row"
                                                    id="btnBack" onserverclick="btnBack_ServerClick"
                                                    style="font-weight: 500; min-width: 100px; border: 1px solid #ccc;">
                                                    <i class="fas fa-arrow-left mr-1"></i> Back
                                                </button>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                            </div>


                            <%-- Grid View - Purchase Request Items --%>
                            <div class="card-body" style="overflow-x:auto;">
                                <asp:UpdatePanel ID="upGridView" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div style="margin-bottom: 10px; padding: 0 20px;">
                                    <asp:Label ID="lblItemCount" runat="server" Text="" CssClass="text-muted" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="form-horizontal" style="margin:0 20px; width:100%;">
                                        <asp:GridView ID="DgvTransfer" runat="server" Width="100%"
                                            AutoGenerateColumns="false"
                                            OnRowDataBound="DgvTransfer_RowDataBound"
                                            HeaderStyle-BackColor="#2c3e50"
                                            HeaderStyle-ForeColor="White"
                                            RowStyle-BackColor="#ffffff"
                                            AlternatingRowStyle-BackColor="#f8f9fa"
                                            CssClass="table table-bordered table-sm"
                                            CellPadding="4"
                                            ShowHeaderWhenEmpty="true">
                                            <Columns>

                                                <%-- Hidden: wID (PR Trans ID) --%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("pr_id") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- Hidden: stk_id --%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStkID" runat="server"
                                                            Text='<%# Eval("stk_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- No. Item --%>
                                                <asp:TemplateField HeaderText="No." HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStkno" runat="server"
                                                            Text='<%# Eval("pr_no") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- Item Name --%>
                                                <asp:TemplateField HeaderText="Item Name" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server"
                                                            Text='<%# Eval("stk_name") %>'></asp:Label>
                                                        <asp:DropDownList ID="ddlEditItem" runat="server" CssClass="form-control select2-item" 
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlEditItem_SelectedIndexChanged"
                                                            Visible="false">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- Stock at CC --%>
                                                <asp:TemplateField HeaderText="Stock (CC)" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStock" runat="server"
                                                            Text='<%# Eval("Stockcc", "{0:0.00}") %>'
                                                            ForeColor='<%# GetStockColor(Eval("Stockcc")) %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                               
                                                <%-- Stock Limit --%>
                                                <%--<asp:TemplateField HeaderText="Limit" HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllimit" runat="server"
                                                            Text='<%# Eval("stklimit", "{0:0.##}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>

                                                <%-- Request Quantity --%>
                                                <asp:TemplateField HeaderText="Qty (u1)" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%-- Label (tampil saat view mode) --%>
                                                        <asp:Label ID="lblEditUnit1" runat="server"
                                                            Text='<%# Eval("U1", "{0:0.##}") %>'
                                                            style="font-weight: 600; color: #2c3e50;">
                                                        </asp:Label>
                                                        <%-- TextBox (tampil saat edit mode) --%>
                                                        <asp:TextBox ID="txtEditUnit1" BackColor="#fff3cd"
                                                            runat="server" MaxLength="10"
                                                            CssClass="form-control qty-input"
                                                            Text='<%# Eval("U1", "{0:0.##}") %>'
                                                            Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <%-- Unit --%>
                                                <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblU1Nm" runat="server"
                                                            Text='<%# Eval("U1_nm") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Factor U1-U2" HeaderStyle-Width="0px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfactor" runat="server"
                                                            Text='<%# Eval("factor1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <%-- Request Quantity --%>
                                                <asp:TemplateField HeaderText="Qty (U2)" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%-- Label (tampil saat view mode) --%>
                                                        <asp:Label ID="lblEditUnit2" runat="server"
                                                            Text='<%# Eval("U2", "{0:0.##}") %>'
                                                            style="font-weight: 600; color: #2c3e50;">
                                                        </asp:Label>
                                                        <%-- TextBox (tampil saat edit mode) --%>
                                                        <asp:TextBox ID="txtEditUnit2" BackColor="#fff3cd"
                                                            runat="server" MaxLength="10"
                                                            CssClass="form-control qty-input"
                                                            Text='<%# Eval("U2", "{0:0.##}") %>'
                                                            Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <%-- Unit --%>
                                                <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblU2Nm" runat="server"
                                                            Text='<%# Eval("U2_nm") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Req Total" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%-- Label (tampil saat view mode) --%>
                                                    <asp:Label ID="lblttlReq" runat="server"
                                                        Text='<%# Eval("Total", "{0:0.##}") %>'
                                                        style="font-weight: 600; color: #2c3e50;">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <%-- Remarks Column --%><%-- item remarks --%>
                                            <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server"
                                                        Text='<%# Eval("remarks") %>'
                                                        style="font-size: 0.9rem; color: #7f8c8d;">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtEditRemarks" runat="server"
                                                        CssClass="form-control"
                                                        Text='<%# Eval("remarks") %>'
                                                        Visible="false" MaxLength="200"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                                <%-- Approve Column --%>
                                                <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkApprove" runat="server" Enabled="false" onclick="onCheckApprove(this)" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <%-- App Qty --%>
                                                <asp:TemplateField HeaderText="App Qty" HeaderStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppQty" runat="server"
                                                            Text='<%# Eval("app_qty", "{0:0.##}") %>'
                                                            style="font-weight: 600; color: #155724;">
                                                        </asp:Label>
                                                        <%-- TextBox (tampil saat edit mode) --%>
                                                        <asp:TextBox ID="txtEditAppQty" BackColor="#fff3cd"
                                                            runat="server" MaxLength="10"
                                                            CssClass="form-control qty-input"
                                                            Text='<%# Eval("app_qty", "{0:0.##}") %>'
                                                            Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <%-- Supplier DropDownList for HO --%>
                                                <asp:TemplateField HeaderText="Supplier" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Eval("supplier_name") %>' Visible="true"></asp:Label>
                                                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control" Visible="false">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <%-- Action column with separate buttons for store (delete) and HO (reject) --%>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <div style="display:flex; justify-content:center; align-items:center; gap:8px;">
                                                            <asp:ImageButton ID="imgbtnDelete" runat="server"
                                                                OnClick="imgbtnDelete_Click" ImageUrl="~/images/delete.png"
                                                                OnClientClick="Confirm()"
                                                                ToolTip="Delete Item"
                                                                Width="20px" Height="20px" />
                                                            <asp:ImageButton ID="imgbtnReject" runat="server"
                                                                OnClick="imgbtnReject_Click" ImageUrl="~/images/cancel.png"
                                                                OnClientClick="Confirm()"
                                                                ToolTip="Reject Item"
                                                                Width="20px" Height="20px" />
                                                            <asp:LinkButton ID="btnNotApprove" runat="server"
                                                                OnClick="btnNotApprove_Click"
                                                                CssClass="btn btn-sm btn-danger"
                                                                ToolTip="Not Approve"
                                                                Visible="false"
                                                                style="padding:2px 6px; font-size:0.78rem; display:flex; align-items:center; justify-content:center; height:20px;">
                                                                <i class="fas fa-ban"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>

                                         <%-- New Dynamic Add Item Button --%>
                                         <div style="margin-top: 15px; margin-bottom: 15px; text-align: left; padding: 0 10px;">
                                             <asp:Button ID="btnAddNewRow" runat="server" CssClass="btn btn-outline-primary"
                                                 OnClick="btnAddNewRow_Click" Text="+ Add Item"
                                                 Style="font-weight: 600; border-radius: 6px; padding: 6px 16px; border: 2px dashed #007bff;" />
                                         </div>

                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                            </div>
                        </div>
                        <!-- Modal: Add Item -->
                        <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server"
                            UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog"
                                    aria-labelledby="ModalItem" aria-hidden="true" data-backdrop="false"
                                    style="opacity:1;z-index:1050;">
                                    <div class="modal-dialog modal-dialog-scrollable" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header" style="background-color:#2c3e50; color:white;">
                                                <h5 class="modal-title" id="ModalStoreLabel">
                                                    <i class="fas fa-search mr-2"></i>Select Item for Purchase Request
                                                </h5>
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-label="Close" style="color:white;">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>

                                            <div class="row">
                                                <div class="form-horizontal" id="Div2" runat="server"
                                                    style="width:100%;">
                                                    <div class="card-body" style="padding-left:10%;padding-right:10%;">
                                                        <div class="form-group row">
                                                            <asp:Label runat="server" AssociatedControlID="txtSearch"
                                                                CssClass="col-sm-2 col-form-label text-nowrap"
                                                                Text="Search: "></asp:Label>

                                                            <div class="col-sm-10">
                                                                <div class="input-group input-group-sm">
                                                                    <asp:TextBox ID="txtSearch" runat="server"
                                                                        CssClass="form-control" placeholder="Type item name to search..."
                                                                        type="search" aria-label="Search">
                                                                    </asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <button runat="server" id="btnSearch"
                                                                            onserverclick="btnSearch_Click"
                                                                            class="btn btn-primary" title="Search">
                                                                            <i class="fas fa-search"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>


                                            </div>

                                            <div class="modal-body">
                                                <div class="table-responsive">
                                                    <table class="table table-striped table-sm">
                                                        <thead class="thead-dark">
                                                            <tr>
                                                                <th width="30px">Check</th>
                                                                <th scope="col" hidden="hidden">ID</th>
                                                                <th scope="col">Item Name</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>

                                                            <asp:ListView ID="LsvStk" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td class="text-center">
                                                                            <asp:CheckBox ID="CheckBox" runat="server">
                                                                            </asp:CheckBox>
                                                                        </td>
                                                                        <td hidden="hidden">
                                                                            <asp:Label ID="lblID" runat="server"
                                                                                Text='<%# Eval("Id")%>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblName" runat="server"
                                                                                Text='<%# Eval("name")%>'></asp:Label>
                                                                        </td>
                                                                       <%-- <td class="text-right">
                                                                            <asp:Label ID="Label1" runat="server"
                                                                                Text=' <%# Eval("stock", "{0:0.##}") %>'>
                                                                            </asp:Label>
                                                                        </td>--%>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>


                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>

                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary"
                                                    data-dismiss="modal">
                                                    <i class="fas fa-times mr-1"></i>Close
                                                </button>
                                                <button type="button" class="btn btn-primary" runat="server"
                                                    onserverclick="RadButton1_ServerClick" id="RadButton1">
                                                    <i class="fas fa-plus mr-1"></i>Add Selected Item(s)
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
            </div>

            </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Error / Status Label -->
            <div style="font-family: Calibri;margin: 0 20px;">
                <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium">
                </asp:Label>
                <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>

            </div>

            <div class="loading" style="text-align:center">

                <div style="display:block;margin-left:auto;margin-right:auto;width:40%;">
                    <br /><br />
                    <asp:label runat="server" Text="Loading... Please wait..." ForeColor="Black" Font-Bold="True"
                        Font-Size="Larger"></asp:label><br /><br />

                    <img src="/images/loader.gif" alt="" />
                </div>
            </div>

            </div>


                <script type="text/javascript">
                    // Global state variable for modal
                    var wasModalOpen = false;

                    // Function to handle Enter key on modal to prevent closure
                    function OnKeyPress(e) {
                        // Optional: logic if needed
                    }

                    // Global function called by Server-Side ScriptManager
                    window.openModalItem = function () {
                        $('#ModalItem').modal('show');
                        setTimeout(function () {
                            var searchBox = $('#<%= txtSearch.ClientID %>');
                            searchBox.focus();
                            // move cursor to end
                            var val = searchBox.val();
                            searchBox.val('').val(val);
                        }, 100);
                    };

                    // Debounce function to delay execution
                    function debounce(func, wait) {
                        var timeout;
                        return function () {
                            var context = this, args = arguments;
                            clearTimeout(timeout);
                            timeout = setTimeout(function () {
                                func.apply(context, args);
                            }, wait);
                        };
                    }

                    $(document).ready(function () {
                        if ($(window).width() >= 960) {
                            document.getElementById("formload").style.width = "100%";
                        } else {
                            document.getElementById("formload").style.width = "80%";
                        }

                        // Bind Search Event with Debounce
                        var $txtSearch = $('#<%= txtSearch.ClientID %>');

                        // Define search function
                        var doSearch = debounce(function () {
                            // Trigger postback
                            __doPostBack('<%= txtSearch.UniqueID %>', '');
                        }, 2000); // 2000ms delay

                        // Attach to input event (covers typing, pasting, etc.)
                        function attachSearchEvents() {
                            var $field = $('#<%= txtSearch.ClientID %>');
                            $field.off('input keydown'); // Start clean

                            $field.on('input', function (e) {
                                doSearch();
                            });

                            // Prevent native form submit on Enter in search box
                            $field.on('keydown', function (e) {
                                if (e.which === 13) {
                                    e.preventDefault();
                                    __doPostBack('<%= txtSearch.UniqueID %>', '');
                                    return false;
                                }
                            });
                        }

                        attachSearchEvents();


                        // Keep modal open after UpdatePanel async postback
                        try {
                            var prm = Sys.WebForms.PageRequestManager.getInstance();

                            prm.add_beginRequest(function () {
                                // Store modal state globally before async postback
                                if ($('#ModalItem').hasClass('show')) {
                                    wasModalOpen = true;
                                } else {
                                    wasModalOpen = false;
                                }
                            });

                            prm.add_endRequest(function () {
                                // Re-attach events because text box was replaced
                                attachSearchEvents();

                                // Restore modal state if it was open
                                if (wasModalOpen) {
                                    window.openModalItem();
                                    wasModalOpen = false; // Reset
                                }
                            });
                        } catch (e) {
                            console.error("Error initializing PageRequestManager:", e);
                        }
                    });
                </script>

                <!-- jQuery and Bootstrap JS files -->
                <script src="Content/js/jquery-3.3.1.min.js"></script>
                <script src="Content/js/popper.min.js"></script>
                <script src="Content/js/bootstrap.min.js"></script>

                <!-- Select2 JS -->
                <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
                <script type="text/javascript">
                    function initializeSelect2() {
                        if (window.jQuery && $.fn.select2) {
                            $('.select2-item').select2({
                                placeholder: "--- Select Item ---",
                                allowClear: true,
                                dropdownAutoWidth: true,
                                width: '100%'
                            });
                        }
                    }

                    $(document).ready(function () {
                        initializeSelect2();

                        // Automatically copy Req Total to App Qty when Approve is checked
                        $(document).on('change', 'input[type=checkbox][id*=chkApprove]', function() {
                            var $row = $(this).closest('tr');
                            var $txtAppQty = $row.find('input[type=text][id*=txtEditAppQty]');
                            var $lblttlReq = $row.find('span[id*=lblttlReq]');
                            
                            if (this.checked) {
                                var reqTotal = $lblttlReq.text() || '0';
                                $txtAppQty.val(reqTotal);
                            } else {
                                $txtAppQty.val('0');
                            }
                        });
                        
                        // Re-initialize select2 after UpdatePanel postbacks
                        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                            prm.add_endRequest(function () {
                                initializeSelect2();
                            });
                        }
                    });
                </script>

                <!-- Datatables-->
                <script src="Content/datatables.net/js/jquery.dataTables.min.js"></script>
                <script src="Content/datatables.net-buttons-bs4/js/dataTables.bootstrap4.min.js"></script>

            </asp:Content>
