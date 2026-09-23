<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvRcvEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvRcvEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>
                <style type="text/css">
                    .loading-backdrop {
                        position: fixed;
                        top: 0;
                        left: 0;
                        background-color: black;
                        z-index: 9998;
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
                <script type="text/javascript">
                    var progressTimeout = null;
                    function ShowProgress() {
                        if (progressTimeout) return;
                        progressTimeout = setTimeout(function () {
                            var modal = $('<div />');
                            modal.addClass("loading-backdrop");
                            $('body').append(modal);
                            var loading = $(".loading");
                            loading.show();
                        }, 200);
                    }
                    function HideProgress() {
                        if (progressTimeout) {
                            clearTimeout(progressTimeout);
                            progressTimeout = null;
                        }
                        $(".loading-backdrop").remove();
                        $(".loading").hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        $('body').css('padding-right', '');
                    }
                    $('form').live("submit", function () {
                        ShowProgress();
                    });
                    //$(document).ready(function () {
                    //    $('.btn').click(function () {
                    //        if (Page_IsValid) {
                    //            $('#spinner').show();
                    //        }
                    //    });
                    //});
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
                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Order /Receive Detail</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Order /Receive Detail</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color:white;">

                    <!-- /.content-header -->
                    <%-- <h2>User Access</h2>--%>
                        <div id="spinner" class="modal" style="display: none">
                            <div class="center">
                                <img alt="" src="/images/loader.gif" />
                            </div>
                        </div>
                        <div class="card shadow-sm">
                            <div class="card-header">
                                <h3 class="card-title">Order Details</h3>
                            </div>
                            <div class="row">
                                <div class="form-horizontal" id="formload">
                                    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="card-body">
                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                                ID="lblorderno" Text="Receive No: "></asp:label>
                                            <div class="input-group col-md-6">

                                                <asp:TextBox style="text-align:left;" runat="server" ID="txtLinkid"
                                                    Class="form-control" MaxLength="30" ReadOnly="true" />

                                                <%--<asp:label runat="server" ID="lblStatus" Text=""
                                                    Style="padding:6px;" CssClass="col-sm-1 col-form-label text-nowrap">
                                                    </asp:label> --%>
                                            </div>
                                            <div class="col-auto my-1">
                                                <div class="form-check"
                                                    style="padding-left:unset; color:blue;font-style:italic;font-size:larger;font-weight:500">
                                                    <asp:label runat="server" ID="lblStatus" Text=""></asp:label>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                                ID="Label4" Text="DO: "></asp:label>
                                            <div class="input-group col-md-6">

                                                <asp:TextBox runat="server" ID="txtDO" Class="form-control"
                                                    MaxLength="15" />
                                            </div>
                                        </div>

                                        <div class="form-group row col-md-12" id="divPR" runat="server" visible="false" style="padding-left:0; margin-left:0;">
                                            <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                                ID="lblPR" Text="Purchase Request: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <asp:TextBox runat="server" ID="txtPRNo" Class="form-control" ReadOnly="true" Enabled="false" />
                                            </div>
                                        </div>

                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                                ID="Label1" Text="CC: "></asp:label>
                                            <div class="input-group col-md-6">
                                                <asp:DropDownList ID="ddlFrom"
                                                    OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged" runat="server"
                                                    CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                                                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                    ControlToValidate="ddlFrom" ErrorMessage="Please Choice Cc"
                                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>


                                        </div>
                                        <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                                ID="Label2" Text="Supplier: "></asp:label>
                                            <div class="input-group col-md-6" style="display:flex; align-items:center;">

                                                <asp:DropDownList ID="DdlSup"
                                                    OnSelectedIndexChanged="DdlSup_SelectedIndexChanged" runat="server"
                                                    CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled" style="flex:1; margin-right:10px;">
                                                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                                                </asp:DropDownList>
                                                <button type="button" runat="server" id="btnLastOrder"
                                                    onserverclick="btnLastOrder_ServerClick"
                                                    class="btn btn-outline-info btn-sm"
                                                    title="Tampilkan item dari order terakhir ke supplier ini"
                                                    style="flex:none; white-space:nowrap; font-weight:500;">
                                                    <i class="fas fa-history mr-1"></i> Last Order
                                                </button>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                    ControlToValidate="DdlSup" ErrorMessage="Please Choice supplier"
                                                    ForeColor="Red" Display="Dynamic" style="position:absolute; bottom:-20px; left:15px; font-size:12px;"></asp:RequiredFieldValidator>
                                            </div>


                                        </div>

                                        <%-- <div class="form-group row col-md-12" runat="server" id="expdel">
                                            <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                                ID="Label3" Text="Exp Date: "></asp:label>
                                            <div class="input-group col-md-6">

                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="far fa-calendar-alt"></i>
                                                        </span>
                                                    </div>
                                                    <ajaxToolkit:ToolkitScriptManager runat="server"
                                                        ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
                                                    <asp:TextBox style="text-align:left; width:auto;" runat="server"
                                                        ID="txtExp" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="cleExp" runat="server"
                                                        TargetControlID="txtExp" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                                </div>
                                            </div>


                                    </div> --%>
                                    <div class="form-group row col-md-12" runat="server" id="createdate"
                                        visible="false">
                                        <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                            ID="lblcreatedate" Text="Create Date: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <%--<ajaxToolkit:ToolkitScriptManager runat="server"
                                                    ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                                                    <asp:TextBox style="text-align:center; width:auto;" runat="server"
                                                        ID="txtcreatedate" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="CleCreate" runat="server"
                                                        TargetControlID="txtcreatedate" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" runat="server" id="Exdeldate">
                                        <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                            ID="lblexdeldate" Text="Expected Del: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <%--<ajaxToolkit:ToolkitScriptManager runat="server"
                                                    ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                                                    <asp:TextBox style="text-align:center; width:auto;" runat="server"
                                                        ID="txtexdel" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="CleExDel" runat="server"
                                                        TargetControlID="txtexdel" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row col-md-12" runat="server" id="finishdate"
                                        visible="false">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblfinishdate" Text="Finish Date: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <%--<ajaxToolkit:ToolkitScriptManager runat="server"
                                                    ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                                                    <asp:TextBox style="text-align:center; width:auto;" runat="server"
                                                        ID="txtfinishdate" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="CleFinish" runat="server"
                                                        TargetControlID="txtfinishdate" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" id="divTax" runat="server">
                                        <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                            ID="lblTax" Text="Tax: "></asp:label>
                                        <div class="input-group col-md-6" style="display:flex; align-items:center;">
                                            <asp:CheckBox ID="chkTax" runat="server" OnCheckedChanged="chkTax_CheckedChanged" AutoPostBack="true" style="margin-right:10px;" />
                                            <asp:Label ID="lblPercentage" runat="server" Text="Percentage " style="margin-right:10px;"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxPercentage" CssClass="form-control" style="width:80px; flex:none; margin-right:10px;" />
                                            <asp:TextBox runat="server" ID="txtTaxAmount" CssClass="form-control" style="flex:1; margin-right:10px;" />
                                            <button type="submit" runat="server" id="btnUpdateTax" onserverclick="btnUpdateTax_ServerClick" class="btn btn-primary btn-sm" style="flex:none;" title="Update Tax">
                                                <i class="fas fa-sync-alt"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-md-2 col-form-label text-nowrap"
                                            ID="lblNotes" Text="Notes: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:center;" runat="server" ID="txtNotes"
                                                Class="form-control" idth="400" TextMode="MultiLine" MaxLength="95"
                                                Height="100" />

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <div Class="col-md-2"></div>
                                        <div class="col-md-offset-2 col-md-10">

                                            <button type="button" runat="server" class="btn btn-primary" id="btnedit"
                                                onserverclick="btnedit_ServerClick" visible="false"
                                                style="font-weight: 500; min-width: 100px;">
                                                <i class="fas fa-edit mr-1"></i> Edit
                                            </button>

                                            <button type="button" runat="server" class="btn btn-primary"
                                                id="btnshow" onclick="openAddItemModal();"
                                                style="font-weight: 500; min-width: 100px;">
                                                <i class="fas fa-plus mr-1"></i> Add Item
                                            </button>
                                            <button type="submit" runat="server" class="btn btn-primary" id="btnSave"
                                                onserverclick="btnSave_Click"
                                                style="font-weight: 500; min-width: 100px;">
                                                <i class="fas fa-paper-plane mr-1"></i> Order
                                            </button>
                                            <button type="submit" runat="server" class="btn btn-primary" id="btnFinish"
                                                onserverclick="btnFinish_Click"
                                                style="font-weight: 500; min-width: 100px;" visible="false">
                                                <i class="fas fa-check mr-1"></i> Receive
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btnBack"
                                                onserverclick="btnBack_ServerClick"
                                                style="font-weight: 500; min-width: 100px;">
                                                <i class="fas fa-arrow-left mr-1"></i> Back
                                            </button>
                                            <button type="submit" runat="server" class="btn btn-primary" id="btncancel"
                                                onserverclick="btncancel_ServerClick"
                                                style="font-weight: 600; border-color:gray; width:108px;"
                                                visible="False">
                                                Cancel rec
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btnacc"
                                                onserverclick="btnacc_ServerClick"
                                                style="font-weight: 600; border-color:gray; width:108px;"
                                                visible="False">
                                                ACC
                                            </button>
                                            <%-- <button type="button" runat="server" class="btn btn-primary"
                                                id="btntest1" style="font-weight: 600; border-color:gray; width:108px;">
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

                                    <div class="row">
                                        <div class="form-horizontal" style="width:100%;">
                                            <asp:GridView ID="DgvTransfer" runat="server" Width="100%"
                                                AutoGenerateColumns="false"
                                                CssClass="table table-striped table-hover table-bordered"
                                                GridLines="None" ShowFooter="true">
                                                <Columns>


                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server"
                                                                Text='<%# Eval("id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:Label ID="lblEditID" runat="server"
                                                                Text='<%# Eval("id") %>'>
                                                            </asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>
                                                                <asp:TextBox ID="txtAddID" runat="server" Width="100px">
                                                                </asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStkID" runat="server"
                                                                Text='<%# Eval("stk_id") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:label ID="lblStkEditID" runat="server"
                                                                Text='<%# Eval("stk_id") %>'>
                                                            </asp:label>
                                                            </EditItemTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label5" runat="server"
                                                                Text='<%# Eval("stk_name") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:Label ID="lblEditName" runat="server"
                                                                Text='<%# Eval("stk_name") %>'>
                                                            </asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>
                                                                <asp:TextBox ID="txtAddName" runat="server"
                                                                    Width="100px"></asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit1" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblUnit1" runat="server"
                                                                Text='<%# Eval("U1","{0:#,0}") %>'></asp:Label>--%>
                                                                <asp:Label ID="lblUnit1" runat="server"
                                                                    Text='<%# Eval("U1") %>'>
                                                                </asp:Label>
                                                                <asp:TextBox ID="txtEditUnit1" BackColor="Aqua"
                                                                    runat="server" MaxLength="10"
                                                                    Text='<%# Eval("U1") %>' Width="40px"
                                                                    Visible="false">
                                                                </asp:TextBox>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>--%>
                                                            <%--<asp:TextBox ID="txtEditUnit1" BackColor="Aqua"
                                                                runat="server" Text='<%# Eval("U1","{0:#,0}") %>'
                                                                Width="40px"
                                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);">
                                                                </asp:TextBox>--%>
                                                                <%-- <asp:TextBox ID="txtEditUnit1" BackColor="Aqua"
                                                                    runat="server" MaxLength="10"
                                                                    Text='<%# Eval("U1") %>'></asp:TextBox>
                                                                    </EditItemTemplate>--%>
                                                                    <%-- <FooterTemplate>
                                                                        <asp:TextBox ID="txtAddUnit1" runat="server"
                                                                            Width="80px">
                                                                        </asp:TextBox>
                                                                        </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit Nm" HeaderStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblU1Nm" runat="server"
                                                                Text='<%# Eval("U1_nm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:Label ID="lblEditU1Nm" runat="server"
                                                                Text='<%# Eval("U1_nm") %>'>
                                                            </asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%-- <FooterTemplate>

                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit 2" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblUnit2" runat="server"
                                                                Text='<%# Eval("U2","{0:#,0}") %>'></asp:Label>--%>
                                                                <asp:Label ID="Label1" runat="server"
                                                                    Text='<%# Eval("U2") %>'>
                                                                </asp:Label>
                                                                <asp:TextBox ID="txtEditUnit2" BackColor="Aqua"
                                                                    runat="server" MaxLength="10"
                                                                    Text='<%# Eval("U2") %>' Width="40px"
                                                                    Visible="false">
                                                                </asp:TextBox>
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:TextBox ID="txtEditUnit2" BackColor="Aqua"
                                                                runat="server" MaxLength="10" Text='<%# Eval("U2") %>'>
                                                            </asp:TextBox>
                                                            </EditItemTemplate>--%>
                                                            <%-- <FooterTemplate>
                                                                <asp:TextBox ID="txtAddUnit2" runat="server"
                                                                    Width="80px"></asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit Nm" HeaderStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblU2Nm" runat="server"
                                                                Text='<%# Eval("U2_nm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:Label ID="lblEditU2Nm" runat="server"
                                                                Text='<%# Eval("U2_nm") %>'>
                                                            </asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>

                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Factor U1-U2"
                                                        HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfactor" runat="server"
                                                                Text='<%# Eval("factor1") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:Label ID="lblEditfactor" runat="server"
                                                                Text='<%# Eval("factor1") %>' Width="60px"></asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>
                                                                <asp:TextBox ID="txtAddfactor" runat="server"
                                                                    Width="80px">
                                                                </asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TotalReceive (Unit2)"
                                                        HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblttlStock" runat="server"
                                                                Text='<%# Eval("ttlStock") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:Label ID="lblttlStock" runat="server"
                                                                Text='<%# Eval("ttlStock") %>' Width="60px"></asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>
                                                                <asp:TextBox ID="txtttlStock" runat="server"
                                                                    Width="80px"></asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost/Unit1" HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblU1Cost" runat="server"
                                                                Text='<%# Eval("CostU1", "{0:N2}") %>'></asp:Label>
                                                            <asp:TextBox ID="txtEditU1Cost" BackColor="Aqua"
                                                                runat="server" MaxLength="10"
                                                                Text='<%# Eval("CostU1", "{0:N2}") %>' Width="90px"
                                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                                Visible="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditU1Cost" BackColor="Aqua"
                                                                runat="server" MaxLength="10"
                                                                Text='<%# Eval("CostU1", "{0:N2}") %>' Width="60px"
                                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);">
                                                            </asp:TextBox>
                                                            </EditItemTemplate>--%>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost/Unit2" HeaderStyle-Width="60px"
                                                        footertext="Tax:<br/>Grand Total:" footerstyle-font-bold="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCost" runat="server"
                                                                Text='<%# Eval("Cost", "{0:N2}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:Label ID="txtEditCost" runat="server"
                                                                Text='<%# Eval("Cost", "{0:N2}") %>' Width="60px"
                                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);">
                                                            </asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>
                                                                <asp:TextBox ID="txtttlStock" runat="server"
                                                                    Width="80px"></asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Cost" HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblttlCost" runat="server"
                                                                Text='<%# Eval("ttlCost", "{0:N2}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:Label ID="lblttlCost" runat="server"
                                                                Text='<%# Eval("ttlCost", "{0:N2}") %>' Width="60px">
                                                            </asp:Label>
                                                            </EditItemTemplate>--%>
                                                            <%--<FooterTemplate>
                                                                <asp:TextBox ID="txtttlStock" runat="server"
                                                                    Width="80px"></asp:TextBox>
                                                                </FooterTemplate>--%>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <%--<asp:ImageButton ID="imgbtnEdit" runat="server"
                                                                CommandName="Edit" ImageUrl="~/Images/edit.png" />
                                                            <asp:ImageButton ID="imgbtnDelete" runat="server"
                                                                CommandName="Delete" ImageUrl="~/Images/Delete.png"
                                                                OnClientClick="Confirm()" />--%>

                                                            <asp:ImageButton ID="imgbtnDelete" runat="server"
                                                                OnClick="imgbtnDelete_Click"
                                                                ImageUrl="~/Images/Delete.png"
                                                                OnClientClick="Confirm()" />
                                                        </ItemTemplate>
                                                        <%-- <EditItemTemplate>
                                                            <asp:ImageButton ID="imgbtnUpdate" runat="server"
                                                                CommandName="Update" ImageUrl="~/Images/icon-update.png"
                                                                Width="16px" Height="16px" />
                                                            <asp:ImageButton ID="imgbtnCancel" runat="server"
                                                                CommandName="Cancel" ImageUrl="~/Images/icon-Cancel.png"
                                                                Width="16px" Height="16px" />
                                                            </EditItemTemplate>--%>

                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                        <%-- <div class="table-responsive" style="margin:0 20px; width:100%;">
                                            <table class="table">
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                                                        <asp:DataPager ID="lvDataPager1" runat="server"
                                                            PagedControlID="lstViewDetails" PageSize="15">
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                        </div>
                </div>
                </div> <!-- End Card -->
                </div>


                <%-- <div class="card-body" style="overflow:scroll;">

                    </div>--%>
                    <!-- Modal Item -->
                    <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>


                            <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog"
                                aria-labelledby="ModalItem" aria-hidden="true" data-backdrop="false"
                                style="opacity:1;z-index:1050;">
                                <div class="modal-dialog modal-dialog-scrollable" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="ModalStoreLabel">Select Item</h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span>
                                            </button>
                                        </div>

                                        <div class="row">
                                            <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                                                <div class="card-body" style="padding-left:10%;padding-right:10%;">




                                                    <div class="form-group row">
                                                        <asp:Label runat="server" AssociatedControlID="txtSearch"
                                                            CssClass="col-sm-2 col-form-label text-nowrap"
                                                            Text="Item: ">
                                                        </asp:Label>

                                                        <div class="col-sm-10">
                                                            <div class="input-group input-group-sm">
                                                                <asp:TextBox ID="txtSearch" runat="server"
                                                                    CssClass="form-control" placeholder="Search..."
                                                                    type="search" aria-label="Search">
                                                                </asp:TextBox>
                                                                <div class="input-group-append">
                                                                    <button runat="server" id="btnSearch"
                                                                        onserverclick="btnSearch_Click"
                                                                        class="btn btn-default" title="Search">
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
                                                <table class="table">
                                                    <thead>
                                                        <tr>
                                                            <th>check</th>
                                                            <th scope="col" hidden="hidden">ID</th>

                                                            <th scope="col">Stock Name</th>
                                                            <th scope="col">Stock</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>

                                                        <asp:ListView ID="LsvStk" runat="server">
                                                            <LayoutTemplate>
                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="CheckBox1" runat="server">
                                                                        </asp:CheckBox>
                                                                    </td>
                                                                    <td hidden="hidden">
                                                                        <asp:Label ID="lblID" runat="server"
                                                                            Text=' <%# Eval("Id")%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblName" runat="server"
                                                                            Text=' <%# Eval("name")%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server"
                                                                            Text=' <%# Eval("ttlqty", "{0:0.##}") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>


                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary"
                                                data-dismiss="modal">Close</button>
                                            <button type="button" class="btn btn-primary" runat="server"
                                                onserverclick="RadButton1_ServerClick" id="RadButton1">Add</button>
                                            <%--<button id="btnAll1" type="button" runat="server"
                                                class="btn btn-primary"
                                                onserverclick="btnAll1_ServerClick">SelectAll</button>--%>
                                                <%--<button type="button" class="btn btn-primary" runat="server"
                                                    onserverclick="btnSubmit_Click">Select</button>
                                                    <button type="button" id="btnAll" runat="server"
                                                        class="btn btn-primary"
                                                        onserverclick="btnAll_Click">SelectAll</button>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <!-- Modal Last Order Suggestion -->
                    <asp:UpdatePanel ID="UpdatePanelLastOrder" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfCloseLastOrderModal" runat="server" Value="0" />
                            <div class="modal fade" id="ModalLastOrder" tabindex="-1" role="dialog"
                                aria-labelledby="ModalLastOrderLabel" aria-hidden="true" data-backdrop="false"
                                style="opacity:1; z-index:1060;">
                                <div class="modal-dialog modal-dialog-scrollable modal-lg" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header" style="background:#17a2b8; color:white;">
                                            <h5 class="modal-title" id="ModalLastOrderLabel">
                                                <i class="fas fa-history mr-2"></i>Last Order Suggestion
                                            </h5>
                                            <button type="button" class="close" data-dismiss="modal"
                                                aria-label="Close" style="color:white;">
                                                <span aria-hidden="true">&times;</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                            <asp:Label ID="lblLastOrderInfo" runat="server"
                                                CssClass="text-muted"
                                                style="font-size:0.85rem; display:block; margin-bottom:10px;"
                                                Text=""></asp:Label>
                                            <div class="table-responsive">
                                                <table class="table table-sm table-bordered table-hover">
                                                    <thead class="thead-light">
                                                        <tr>
                                                            <th style="width:40px; text-align:center;">&#10003;</th>
                                                            <th>Stock Name</th>
                                                            <th style="width:80px; text-align:center;">Unit1</th>
                                                            <th style="width:80px; text-align:center;">Unit2</th>
                                                            <th style="width:110px; text-align:right;">Cost/Unit1</th>
                                                            <th style="width:70px; text-align:center;">Satuan</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:ListView ID="LsvLastOrder" runat="server">
                                                            <LayoutTemplate>
                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="text-align:center;">
                                                                        <asp:CheckBox ID="chkLastOrder" runat="server" Checked="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblLOStkID" runat="server"
                                                                            Text='<%# Eval("stk_id") %>'
                                                                            style="display:none;"></asp:Label>
                                                                        <asp:Label ID="lblLOStkName" runat="server"
                                                                            Text='<%# Eval("stk_name") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:center;">
                                                                        <asp:Label ID="lblLOU1" runat="server"
                                                                            Text='<%# Eval("U1") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:center;">
                                                                        <asp:Label ID="lblLOU2" runat="server"
                                                                            Text='<%# Eval("U2") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:right;">
                                                                        <asp:Label ID="lblLOCostU1" runat="server"
                                                                            Text='<%# Eval("CostU1", "{0:N2}") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:center;">
                                                                        <asp:Label ID="lblLOU1Nm" runat="server"
                                                                            Text='<%# Eval("U1_nm") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <tr>
                                                                    <td colspan="6" style="text-align:center; color:#888; padding:20px;">
                                                                        <i class="fas fa-info-circle mr-1"></i>
                                                                        Tidak ada order sebelumnya untuk supplier ini.
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary"
                                                data-dismiss="modal">Close</button>
                                            <button type="submit" runat="server" id="btnAddLastOrder"
                                                onserverclick="btnAddLastOrder_ServerClick"
                                                class="btn btn-info" CausesValidation="false">
                                                <i class="fas fa-plus mr-1"></i> Add Selected Items
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <!-- Modal Store -->
                    <div style="font-family: Calibri;margin: 0 20px;">
                        <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>

                    </div>
                    <div class="loading">
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
                    </div>


                    <script type="text/javascript">
                        // Global state variable for modal
                        var wasModalOpen = false;
                        var wasLastOrderModalOpen = false;

                        // Function to handle Enter key on modal to prevent closure
                        function OnKeyPress(e) {
                            // Optional: logic if needed
                        }

                        // Function to handle opening the Add Item modal with validation
                        function openAddItemModal() {
                            var supplierVal = $('#<%= DdlSup.ClientID %>').val();
                            if (supplierVal === "0" || supplierVal === "") {
                                alert("Please select supplier first!");
                                return false;
                            }
                            var ccVal = $('#<%= ddlFrom.ClientID %>').val();
                            if (ccVal === "0" || ccVal === "") {
                                alert("Please select CC first!");
                                return false;
                            }
                            $('#ModalItem').modal('show');
                            setTimeout(function () {
                                var searchBox = $('#<%= txtSearch.ClientID %>');
                                searchBox.focus();
                                var val = searchBox.val();
                                searchBox.val('').val(val);
                            }, 100);
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

                        // Open Last Order modal
                        window.openLastOrderModal = function () {
                            $('#ModalLastOrder').modal('show');
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

                                prm.add_beginRequest(function (sender, args) {
                                    ShowProgress();
                                    
                                    var postBackElement = args.get_postBackElement();
                                    var isAddButton = postBackElement && postBackElement.id && postBackElement.id.indexOf("RadButton1") > -1;
                                    var isSearchBox = postBackElement && postBackElement.id && postBackElement.id.indexOf("txtSearch") > -1;
                                    var isClearSearch = postBackElement && postBackElement.id && postBackElement.id.indexOf("btnClearSearch") > -1;
                                    var isSearchBtn = postBackElement && postBackElement.id && postBackElement.id.indexOf("btnSearch") > -1;

                                    // Store modal state globally before async postback
                                    // Don't reopen if they clicked the Add button, they want to close it.
                                    if ($('#ModalItem').hasClass('show') && !isAddButton) {
                                        wasModalOpen = true;
                                    } else {
                                        wasModalOpen = false;
                                    }
                                    if ($('#ModalLastOrder').hasClass('show')) {
                                        wasLastOrderModalOpen = true;
                                    } else {
                                        wasLastOrderModalOpen = false;
                                    }

                                    // Disable save buttons to prevent double submission
                                    var btnSave = document.getElementById('<%= btnSave.ClientID %>');
                                    var btnFinish = document.getElementById('<%= btnFinish.ClientID %>');
                                    if (btnSave) {
                                        btnSave.disabled = true;
                                    }
                                    if (btnFinish) {
                                        btnFinish.disabled = true;
                                    }
                                });

                                prm.add_endRequest(function () {
                                    HideProgress();
                                    // Re-attach events because text box was replaced
                                    attachSearchEvents();

                                    // Check if modal should be closed
                                    var closeFlag = $('#<%= hfCloseLastOrderModal.ClientID %>').val();
                                    if (closeFlag === '1') {
                                        $('#ModalLastOrder').modal('hide');
                                        $('#<%= hfCloseLastOrderModal.ClientID %>').val('0');
                                        wasLastOrderModalOpen = false;
                                        return;
                                    }

                                    // Restore modal state if it was open
                                    if (wasModalOpen) {
                                        window.openModalItem();
                                        wasModalOpen = false; // Reset
                                    }
                                    if (wasLastOrderModalOpen) {
                                        window.openLastOrderModal();
                                        wasLastOrderModalOpen = false;
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



                    <!-- Datatables-->
                    <script src="Content/datatables.net/js/jquery.dataTables.min.js"></script>
                    <script src="Content/datatables.net-buttons-bs4/js/dataTables.bootstrap4.min.js"></script>

                    <script type="text/javascript">

                        $(document).ready(function () {
                            $('#<%= LsvStk.ClientID %>').DataTable({
                                "aLengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                                "iDisplayLength": 5,
                                "order": [[3, "asc"]],
                                stateSave: true,
                                stateSaveCallback: function (settings, data) {
                                    localStorage.setItem
                                        ('DataTables_' + settings.sInstance, JSON.stringify(data));
                                },
                                stateLoadCallback: function (settings) {
                                    return JSON.parse
                                        (localStorage.getItem('DataTables_' + settings.sInstance));
                                }
                            });
                        });



                    </script>

            </asp:Content>