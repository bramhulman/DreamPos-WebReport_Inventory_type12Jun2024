<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvOrderWDetailRcv.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvOrderWDetailRcv" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server"></asp:ScriptManager>
                <style>
                    .input-group>.form-control {
                        width: 800px;
                    }
                </style>
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
                </style>
                <script type="text/javascript"
                    src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                </script>
                <script type="text/javascript">
                    // Existing ShowProgress function
                    function ShowProgress() {
                        setTimeout(function () {
                            var modal = $('<div />');
                            modal.addClass("modal");
                            $('body').append(modal);
                            var loading = $(".loading");
                            loading.show();
                        }, 200);
                    }

                    function HideProgress() {
                        $(".modal").remove();
                        $(".loading").hide();
                    }

                    $('form').live("submit", function () {
                        ShowProgress();
                    });

                    // Handle UpdatePanel partial postbacks
                    if (typeof Sys != 'undefined') {
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        prm.add_endRequest(function (sender, args) {
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
                        document.getElementById("<%=btnFinish.ClientID %>").disabled = true;
                    }
                    window.onbeforeunload = DisableButton;
                </script>
                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Receive Order Stock From Warehouse</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Receive Order Stock From Warehouse</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color:white;">

                    <!-- /.content-header -->
                    <%-- <h2>User Access</h2>--%>
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
                        <div class="row">
                            <div class="form-horizontal" id="formload">
                                <div class="card-body">
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblorderno" Text="Order No: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:center;" runat="server" ID="txtID"
                                                Class="form-control" MaxLength="30" ReadOnly="true" />
                                            <asp:label runat="server" ID="lblStatus" Text="" Style="padding:6px;"
                                                CssClass="align-middle"></asp:label>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" id="pnlPickDel" runat="server" visible="false">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            Text="Delivery Note: "></asp:label>
                                        <div class="input-group col-md-6">
                                            <asp:DropDownList ID="ddlPendingDel" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlPendingDel_SelectedIndexChanged">
                                                <asp:ListItem Value="">-- Select delivery note --</asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="input-group-append">
                                                <button type="button" runat="server" id="btnPrintDel"
                                                    onserverclick="btnPrintDel_ServerClick"
                                                    class="btn btn-outline-secondary" style="font-weight:600;">
                                                    Preview / Print
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label2" Text="warehouse/Ho: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:DropDownList ID="ddlWare" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True"
                                                ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="ddlWare" ErrorMessage="Please Choice warehouse"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label1" Text="CC: "></asp:label>
                                        <div class="input-group col-md-6">
                                            <asp:DropDownList ID="ddlFrom" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True"
                                                ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="ddlFrom" ErrorMessage="Please Choice Cc"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" runat="server" id="Createdate"
                                        visible="false">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label4" Text="Create Date: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <%--<ajaxToolkit:ToolkitScriptManager runat="server"
                                                    ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
                                                    --%>
                                                    <asp:TextBox style="text-align:center; width:auto" runat="server"
                                                        ID="txtcreatedate1" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="clecreatedate" runat="server"
                                                        TargetControlID="txtcreatedate1" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" runat="server" id="Deliverydate"
                                        visible="false">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label3" Text="Delivery Date: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <%--<ajaxToolkit:ToolkitScriptManager runat="server"
                                                    ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
                                                    --%>
                                                    <asp:TextBox style="text-align:center; width:auto" runat="server"
                                                        ID="txtexpD1" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="cle" runat="server"
                                                        TargetControlID="txtexpD1" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                    </div>

                                    <div class="form-group row col-md-12" runat="server" id="receivedate"
                                        visible="false">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label5" Text="Receive Date: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <%--<ajaxToolkit:ToolkitScriptManager runat="server"
                                                    ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
                                                    --%>
                                                    <asp:TextBox style="text-align:center; width:auto" runat="server"
                                                        ID="txtrcvdate1" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="clereceivedate" runat="server"
                                                        TargetControlID="txtrcvdate1" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                    </div>
                                    <%-- <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                            ID="Label3" Text="Receive Date: "></asp:label>
                                        <div class="input-group col-md-3">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>

                                                <asp:TextBox style="text-align:center;" runat="server" ID="txtexpD"
                                                    Class="form-control" AutoPostBack="false" />
                                                <ajaxtoolkit:CalendarExtender ID="cle" runat="server"
                                                    TargetControlID="txtexpD" Format="dd MMM yyyy">
                                                </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                </div> --%>
                                <%-- <div class="form-group row col-md-12">
                                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                        ID="lblNotes" Text="Notes: "></asp:label>
                                    <div class="input-group col-md-3">

                                        <asp:TextBox style="text-align:center;" runat="server" ID="txtNotes"
                                            Class="form-control" idth="400" TextMode="MultiLine" MaxLength="150"
                                            Height="100" />

                                    </div>


                            </div>--%>
                            <div class="form-group row">
                                <div Class="col-md-2"></div>
                                <div class="col-md-offset-2 col-md-10">
                                    <%-- <button type="button" runat="server" class="btn btn-primary"
                                        data-toggle="modal" id="btnshow" data-target="#ModalItem"
                                        style="font-weight: 600; border-color:gray; width:108px;">
                                        Add Item
                                        </button>--%>
                                        <button type="submit" runat="server" class="btn btn-primary" id="btnFinish"
                                            onserverclick="btnFinish_Click"
                                            style="font-weight: 600; border-color:gray; width:120px;">
                                            Receive Delivery
                                        </button>
                                        <button type="button" runat="server" class="btn btn-primary" id="btnSetAll"
                                            onserverclick="btnSetAll_Click"
                                            style="font-weight: 600; border-color:gray; width:120px;">
                                            Receive = Delivery
                                        </button>
                                        <button type="button" runat="server" class="btn btn-primary" id="btnBack"
                                            onserverclick="btnBack_Click"
                                            style="font-weight: 600; border-color:gray; width:120px;">
                                            Back
                                        </button>
                                        <button type="button" runat="server" class="btn btn-primary" id="btncancel"
                                            visible="false" onserverclick="btncancel_ServerClick"
                                            style="font-weight: 600; border-color:gray; width:120px;">
                                            Cancel Receive
                                        </button>
                                        <%-- <button type="button" runat="server" class="btn btn-primary" id="btntest1"
                                            style="font-weight: 600; border-color:gray; width:108px;">
                                            Add test
                                            </button>--%>
                                            <%--<table>
                                                <tr>
                                                    <td>
                                                        <asp:Button type="button" runat="server" ID="btnShow2"
                                                            Text="Add Item" CssClass="btn btn-primary"
                                                            data-target="#ModalItem" data-toggle="modal"
                                                            Style="font-weight: 600; border-color:gray; width:108px;" />

                                                    </td>
                                                    <td>

                                                        <asp:Button type="button" runat="server" ID="btnFinish"
                                                            Text="Save Finish" CssClass="btn btn-primary"
                                                            Style="font-weight: 600; border-color:gray; width:108px;" />
                                                    </td>
                                                    <td>
                                                        <asp:Button type="button" runat="server" ID="btnBack"
                                                            Text="Back" CssClass="btn btn-primary"
                                                            Style="font-weight: 600; border-color:gray; width:108px;"
                                                            CausesValidation="false" />
                                                    </td>
                                                </tr>
                                                </table>--%>

                                </div>
                            </div>



                        </div>

                </div>
                </div>


                <div class="card-body" style="overflow:scroll;">
                    <div class="row">
                        <div class="form-horizontal" style="margin:0 20px; width:100%;">
                            <asp:GridView ID="DgvTransfer" runat="server" Width="100%" AutoGenerateColumns="false"
                                OnRowDataBound="RowDataBound" OnRowDeleting="DgvTransfer_RowDeleting"
                                OnRowUpdating="DgvTransfer_RowUpdating"
                                OnRowCancelingEdit="DgvTransfer_RowCancelingEdit" OnRowEditing="DgvTransfer_RowEditing"
                                HeaderStyle-BackColor="#4D4D4D" HeaderStyle-ForeColor="White" CellPadding="5">
                                <Columns>


                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("wID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblIEditD" runat="server" Text='<%# Eval("wID") %>'>
                                            </asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStkID" runat="server" Text='<%# Eval("stk_id") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblStkEditID" runat="server" Text='<%# Eval("stk_id") %>'>
                                            </asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("stk_name") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditName" runat="server" Text='<%# Eval("stk_name") %>'>
                                            </asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Order" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnit1" runat="server"
                                                Text='<%# Eval("totalu2", "{0:0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditUnit1" runat="server"
                                                Text='<%# Eval("totalu2", "{0:0.00}") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delivery" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnit1Del" runat="server"
                                                Text='<%# Eval("U1Del", "{0:0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditUnit1Del" runat="server"
                                                Text='<%# Eval("U1Del", "{0:0.00}") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receive" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnit1Rcv" runat="server"
                                                Text='<%# Eval("u1Rcv", "{0:0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditUnit1Rcv" BackColor="Aqua" runat="server"
                                                Text='<%# Eval("u1Rcv", "{0:0.00}") %>' Width="70px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U2_nm") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditU1Nm" runat="server" Text='<%# Eval("U2_nm") %>'>
                                            </asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                                ImageUrl="~/Images/edit.png" />

                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update"
                                                ImageUrl="~/Images/icon-update.png" Width="16px" Height="16px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                                ImageUrl="~/Images/icon-Cancel.png" Width="16px" Height="16px" />
                                        </EditItemTemplate>

                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>

                        </div>
                        <%-- <div class="table-responsive" style="margin:0 20px; width:100%;">
                            <table class="table">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                                        <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails"
                                            PageSize="15">
                                            <Fields>
                                                <asp:NumericPagerField ButtonType="Link" />
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                            </table>
                    </div>--%>

                </div>
                </div>

                <div style="font-family: Calibri; ">
                    <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>

                </div>

                </div>

                </div>


                <script>


                    function OnKeyPress(e) {
                        if (e.keyCode == "27") {
                            $find("ModalItem").hide();


                        }
                    }

                    function openModalItem() {
                        $('#ModalItem').modal({ show: true });
                        //document.getElementById("txtsearch").focus();
                    }

                    if ($(window).width() >= 960) {
                        document.getElementById("formload").style.width = "100%";
                    } else {
                        document.getElementById("formload").style.width = "80%";

                    }
                </script>
            </asp:Content>
