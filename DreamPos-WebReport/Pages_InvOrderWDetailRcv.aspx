<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvOrderWDetailRcv.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvOrderWDetailRcv" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/inventory-mobile.css" />
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
            font-size: 0.85rem;
        }
        .table-custom tbody td {
            vertical-align: middle;
            font-size: 0.85rem;
            padding: 6px 10px;
        }
        .table-custom tbody tr:hover {
            background-color: #f1f7fd;
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
    </style>
    <script type="text/javascript">
        function ShowProgress() {
            var loading = $(".loading");
            if (loading.length) loading.show();
        }

        function HideProgress() {
            $(".loading").hide();
        }

        $(document).ready(function () {
            if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function (sender, args) {
                    HideProgress();
                });
            }
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

        function DisableButton() {
            var btn = document.getElementById("<%=btnFinish.ClientID %>");
            if (btn) btn.disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <!-- Loading Spinner -->
    <div id="spinner" class="loading">
        <div style="font-size: 3rem; color: #007bff; margin-bottom: 15px;">
            <i class="fas fa-circle-notch fa-spin"></i>
        </div>
        <div style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 1.25rem; color: #333; font-weight: 600; margin-bottom: 5px;">
            Processing...
        </div>
        <div style="font-family: 'Segoe UI', sans-serif; font-size: 0.9rem; color: #6c757d;">
            Please wait a moment.
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-2">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-truck-loading text-primary mr-2"></i>Receive Order Stock From Warehouse
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item active">Receive Order Stock</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid mb-4">
        <!-- Order Header Card -->
        <div class="card card-custom" id="formload">
            <div class="card-header">
                <i class="fas fa-file-invoice mr-2"></i>Order & Delivery Information
            </div>
            <div class="card-body p-3">
                <div class="form-group row align-items-center mb-2">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" ID="lblorderno" Text="Order No: "></asp:Label>
                    <div class="col-md-5 col-sm-8 d-flex align-items-center">
                        <asp:TextBox style="text-align:center;" runat="server" ID="txtID" CssClass="form-control form-control-sm font-weight-bold mr-2" MaxLength="30" ReadOnly="true" />
                        <asp:Label runat="server" ID="lblStatus" Text="" CssClass="badge badge-info p-2"></asp:Label>
                    </div>
                </div>

                <div class="form-group row align-items-center mb-2" id="pnlPickDel" runat="server" visible="false">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" Text="Delivery Note: "></asp:Label>
                    <div class="col-md-5 col-sm-8">
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlPendingDel" runat="server" CssClass="form-control form-control-sm"
                                AppendDataBoundItems="True" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPendingDel_SelectedIndexChanged">
                                <asp:ListItem Value="">-- Select delivery note --</asp:ListItem>
                            </asp:DropDownList>
                            <div class="input-group-append">
                                <button type="button" runat="server" id="btnPrintDel"
                                    onserverclick="btnPrintDel_ServerClick"
                                    class="btn btn-outline-secondary btn-sm font-weight-bold">
                                    <i class="fas fa-print mr-1"></i>Preview / Print
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row align-items-center mb-2">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" ID="Label2" Text="Warehouse / HO: "></asp:Label>
                    <div class="col-md-5 col-sm-8">
                        <asp:DropDownList ID="ddlWare" runat="server" CssClass="form-control form-control-sm"
                            AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True"
                            ViewStateMode="Enabled">
                            <asp:ListItem Value="0">----Select----</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="ddlWare" ErrorMessage="Please select warehouse"
                            ForeColor="Red" Display="Dynamic" Font-Size="Small"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group row align-items-center mb-2">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" ID="Label1" Text="Cost Centre: "></asp:Label>
                    <div class="col-md-5 col-sm-8">
                        <asp:DropDownList ID="ddlFrom" runat="server" CssClass="form-control form-control-sm"
                            AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True"
                            ViewStateMode="Enabled">
                            <asp:ListItem Value="0">----Select----</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="ddlFrom" ErrorMessage="Please select Cost Centre"
                            ForeColor="Red" Display="Dynamic" Font-Size="Small"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group row align-items-center mb-2" runat="server" id="Createdate" visible="false">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" ID="Label4" Text="Create Date: "></asp:Label>
                    <div class="col-md-4 col-sm-6">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                            </div>
                            <asp:TextBox style="text-align:center;" runat="server" ID="txtcreatedate1" CssClass="form-control form-control-sm" AutoPostBack="false" />
                            <ajaxToolkit:CalendarExtender ID="clecreatedate" runat="server"
                                TargetControlID="txtcreatedate1" Format="dd MMM yyyy">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                </div>

                <div class="form-group row align-items-center mb-2" runat="server" id="Deliverydate" visible="false">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" ID="Label3" Text="Delivery Date: "></asp:Label>
                    <div class="col-md-4 col-sm-6">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                            </div>
                            <asp:TextBox style="text-align:center;" runat="server" ID="txtexpD1" CssClass="form-control form-control-sm" AutoPostBack="false" />
                            <ajaxToolkit:CalendarExtender ID="cle" runat="server"
                                TargetControlID="txtexpD1" Format="dd MMM yyyy">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                </div>

                <div class="form-group row align-items-center mb-2" runat="server" id="receivedate" visible="false">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label font-weight-bold" ID="Label5" Text="Receive Date: "></asp:Label>
                    <div class="col-md-4 col-sm-6">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                            </div>
                            <asp:TextBox style="text-align:center;" runat="server" ID="txtrcvdate1" CssClass="form-control form-control-sm" AutoPostBack="false" />
                            <ajaxToolkit:CalendarExtender ID="clereceivedate" runat="server"
                                TargetControlID="txtrcvdate1" Format="dd MMM yyyy">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                </div>

                <!-- Action Buttons -->
                <div class="form-group row mt-3 pt-3 border-top mb-0">
                    <div class="col-sm-10 offset-sm-2 d-flex flex-wrap gap-2">
                        <button type="submit" runat="server" class="btn btn-primary btn-sm font-weight-bold mr-2 shadow-sm" id="btnFinish"
                            onserverclick="btnFinish_Click">
                            <i class="fas fa-check-circle mr-1"></i>Receive Delivery
                        </button>
                        <button type="button" runat="server" class="btn btn-info btn-sm font-weight-bold mr-2 shadow-sm" id="btnSetAll"
                            onserverclick="btnSetAll_Click">
                            <i class="fas fa-equals mr-1"></i>Receive = Delivery
                        </button>
                        <button type="button" runat="server" class="btn btn-secondary btn-sm font-weight-bold mr-2 shadow-sm" id="btnBack"
                            onserverclick="btnBack_Click">
                            <i class="fas fa-arrow-left mr-1"></i>Back
                        </button>
                        <button type="button" runat="server" class="btn btn-danger btn-sm font-weight-bold shadow-sm" id="btncancel"
                            visible="false" onserverclick="btncancel_ServerClick">
                            <i class="fas fa-times-circle mr-1"></i>Cancel Receive
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Detail Table Card -->
        <div class="card card-custom">
            <div class="card-header">
                <i class="fas fa-boxes mr-2"></i>Order & Receive Stock Items
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:GridView ID="DgvTransfer" runat="server" Width="100%" AutoGenerateColumns="false"
                        OnRowDataBound="RowDataBound" OnRowDeleting="DgvTransfer_RowDeleting"
                        OnRowUpdating="DgvTransfer_RowUpdating"
                        OnRowCancelingEdit="DgvTransfer_RowCancelingEdit" OnRowEditing="DgvTransfer_RowEditing"
                        HeaderStyle-BackColor="#1e3a5f" HeaderStyle-ForeColor="White"
                        CssClass="table table-bordered table-hover table-custom mb-0" CellPadding="5">
                        <Columns>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("wID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblIEditD" runat="server" Text='<%# Eval("wID") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStkID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStkEditID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Stock Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="100px" ItemStyle-CssClass="text-right">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("totalu2", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditUnit1" runat="server" Text='<%# Eval("totalu2", "{0:0.00}") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delivery Qty" HeaderStyle-Width="100px" ItemStyle-CssClass="text-right">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnit1Del" runat="server" Text='<%# Eval("U1Del", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditUnit1Del" runat="server" Text='<%# Eval("U1Del", "{0:0.00}") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Receive Qty" HeaderStyle-Width="110px" ItemStyle-CssClass="text-right">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnit1Rcv" runat="server" Text='<%# Eval("u1Rcv", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditUnit1Rcv" CssClass="form-control form-control-sm text-right font-weight-bold" BackColor="#eaf4ff" runat="server"
                                        Text='<%# Eval("u1Rcv", "{0:0.00}") %>' Width="85px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="80px" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U2_nm") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditU1Nm" runat="server" Text='<%# Eval("U2_nm") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="80px" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                        ImageUrl="~/Images/edit.png" ToolTip="Edit receive quantity" Style="vertical-align: middle;" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update"
                                        ImageUrl="~/Images/icon-update.png" Width="18px" Height="18px" ToolTip="Save changes" Style="vertical-align: middle; margin-right: 4px;" />
                                    <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                        ImageUrl="~/Images/icon-Cancel.png" Width="18px" Height="18px" ToolTip="Cancel edit" Style="vertical-align: middle;" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div class="mt-2">
            <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
