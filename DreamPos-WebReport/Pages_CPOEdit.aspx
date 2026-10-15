<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_CPOEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_CPOEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <link rel="stylesheet" href="css/inventory-mobile.css" />
</asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>
                <style type="text/css">
                    .loading {
                        font-family: Arial;
                        font-size: 10pt;
                        border: 1px solid #67CFF5;
                        top: 0;
                        left: 0;
                        width: 100%;
                        height: 100%;
                        display: none;
                        position: fixed;
                        background-color: rgba(0, 0, 0, 0.4);
                        text-align: center;
                        z-index: 9999;
                    }

                    .loading>div {
                        background: white;
                        padding: 20px;
                        border-radius: 10px;
                        display: inline-block;
                        margin-top: 200px;
                        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
                    }
                </style>
                <script type="text/javascript">
                    function ShowProgress() {
                        var loading = $(".loading");
                        if (loading.length === 0) return;
                        loading.fadeIn(300);

                        // Center loading
                        var top = Math.max($(window).height() / 2 - loading.height() / 2, 0);
                        var left = Math.max($(window).width() / 2 - loading.width() / 2, 0);
                        loading.css({ top: top + 'px', left: left + 'px', position: 'fixed' });
                    }

                    function HideProgress() {
                        $('.loading').fadeOut(300);
                    }

                    // Sembunyikan loading saat page load selesai
                    $(window).on('load', HideProgress);

                    // Untuk UpdatePanel biasa (AsyncPostBack)
                    $(document).ready(function () {
                        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                            if (prm) {
                                prm.add_endRequest(HideProgress);
                            }
                        }
                    });

                    function startExport() {
                        ShowProgress();
                        setTimeout(function () {
                            HideProgress();
                        }, 8000);
                    }
                    //function ShowProgress() {
                    //    setTimeout(function () {
                    //        var modal = $('<div />');
                    //        modal.addClass("modal");
                    //        $('body').append(modal);
                    //        var loading = $(".loading");
                    //        loading.show();
                    //        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                    //        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                    //        loading.css({ top: top, left: left });
                    //    }, 200);
                    //}
                    //$('form').live("submit", function () {
                    //    ShowProgress();
                    //});
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
                    function scrollToUsage() {
                        document.getElementById("usagestockdiv")
                            .scrollIntoView({ behavior: "smooth" });
                    }
                </script>
                <script type="text/javascript">
                    function DisableButton() {

                        document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                    }
                    window.onbeforeunload = DisableButton;
                </script>
                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Pre-Order</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Pre-Order</li>
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
                        <div class="row">
                            <div class="form-horizontal" id="formload">
                                <div class="card-body">
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblorderno" Text="PO No: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:left;" runat="server" ID="txtLinkid"
                                                Class="form-control" MaxLength="30" ReadOnly="true" />

                                            <%--<asp:label runat="server" ID="lblStatus" Text="" Style="padding:6px;"
                                                CssClass="col-sm-1 col-form-label text-nowrap"></asp:label> --%>
                                        </div>
                                        <div class="col-auto my-1">
                                            <div class="form-check"
                                                style="padding-left:unset; color:blue;font-style:italic;font-size:larger;font-weight:500">
                                                <asp:label runat="server" ID="lblStatus" Text=""></asp:label>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label4" Text="Atas Nama: "></asp:label>
                                        <div class="input-group col-md-6">
                                            <asp:DropDownList ID="ddlCustomer"
                                                OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" runat="server"
                                                CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true"
                                                EnableViewState="True" ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:TextBox style="text-align:left;" runat="server" ID="txtDO"
                                                Class="form-control" MaxLength="15" />--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="ddlCustomer" ErrorMessage="Please Choice Customer"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>


                                    </div>

                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label1" Text="Outlet: "></asp:label>
                                        <div class="input-group col-md-6">
                                            <asp:DropDownList ID="ddlOutlet"
                                                OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged" runat="server"
                                                CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true"
                                                EnableViewState="True" ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="ddlOutlet" ErrorMessage="Please Choice Outlet"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>


                                    </div>
                                    <%-- <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                            ID="Label2" Text="Payment: "></asp:label>
                                        <div class="input-group col-md-3">

                                            <asp:DropDownList ID="DdlPayment"
                                                OnSelectedIndexChanged="DdlPayment_SelectedIndexChanged" runat="server"
                                                CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true"
                                                EnableViewState="True" ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="DdlPayment" ErrorMessage="Please Choice supplier"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>


                                </div>--%>


                                <div class="form-group row col-md-12" runat="server" id="transactiondate"
                                    visible="false">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                        ID="lbltransactiondate" Text="Trans Date: "></asp:label>
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
                                                    ID="txttransdate" Class="form-control" AutoPostBack="false" />
                                                <ajaxtoolkit:CalendarExtender ID="CleTrans" runat="server"
                                                    TargetControlID="txttransdate" Format="dd MMM yyyy">
                                                </ajaxtoolkit:CalendarExtender>
                                        </div>
                                    </div>


                                </div>
                                <div class="form-group row col-md-12" runat="server" id="Businessdate">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                        ID="lblbusidate" Text="Busidate: "></asp:label>
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
                                                    ID="txtbusidate" Class="form-control" AutoPostBack="false" />
                                                <ajaxtoolkit:CalendarExtender ID="CleBusidate" runat="server"
                                                    TargetControlID="txtbusidate" Format="dd MMM yyyy">
                                                </ajaxtoolkit:CalendarExtender>
                                        </div>
                                    </div>
                                </div>

                                <%-- <div class="form-group row col-md-12">
                                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                        ID="lblNotes" Text="Notes: "></asp:label>
                                    <div class="input-group col-md-3">

                                        <asp:TextBox style="text-align:center;" runat="server" ID="txtNotes"
                                            Class="form-control" idth="400" TextMode="MultiLine" MaxLength="95"
                                            Height="100" />

                                    </div>


                            </div>--%>
                            <div class="form-group row">
                                <div Class="col-md-2"></div>
                                <div class="col-md-offset-2 col-md-10">

                                    <button type="button" runat="server" class="btn btn-primary" id="btnedit"
                                        onserverclick="btnedit_ServerClick" visible="false"
                                        style="font-weight: 600; border-color:gray; width:108px;">
                                        edit
                                    </button>

                                    <button type="button" runat="server" class="btn btn-primary" data-toggle="modal"
                                        id="btnshow" data-target="#ModalItem"
                                        style="font-weight: 600; border-color:gray; width:108px;">
                                        Add Item
                                    </button>
                                    <button type="submit" runat="server" class="btn btn-primary" id="btnSave"
                                        onserverclick="btnSave_Click"
                                        style="font-weight: 600; border-color:gray; width:108px;">
                                        Order
                                    </button>
                                    <%--<%--<button type="submit" runat="server" class="btn btn-primary" id="btnFinish"
                                        onserverclick="btnFinish_Click"
                                        style="font-weight: 600; border-color:gray; width:108px;" visible="false">
                                        Receive
                                        </but--%>
                                        <button type="button" runat="server" class="btn btn-primary" id="btnusage"
                                            onserverclick="btnusage_ServerClick"
                                            onclientclick="scrollToUsage(); return false;"
                                            style="font-weight: 600; border-color:gray; width:108px;">
                                            usage
                                        </button>
                                        <button type="button" runat="server" class="btn btn-primary" id="btnBack"
                                            onserverclick="btnBack_ServerClick"
                                            style="font-weight: 600; border-color:gray; width:108px;">
                                            Back
                                        </button>
                                        <button type="submit" runat="server" class="btn btn-primary" id="btncancel"
                                            onserverclick="btncancel_ServerClick"
                                            style="font-weight: 600; border-color:gray; width:108px;" visible="False">
                                            Cancel rec
                                        </button>
                                        <button type="button" runat="server" class="btn btn-primary" id="btnacc"
                                            onserverclick="btnacc_ServerClick"
                                            style="font-weight: 600; border-color:gray; width:108px;" visible="False">
                                            ACC
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
                            <asp:Button ID="btnExportDetail" runat="server" Text="Export to Excel"
                                CssClass="btn btn-success" Visible="false"
                                style="font-weight: 600; border-color:gray; width:200px; margin:5px;"
                                OnClick="btnExportDetail_Click" OnClientClick="startExport(); return true;" />
                            <asp:GridView ID="DgvTransfer" runat="server" Width="100%" AutoGenerateColumns="false"
                                HeaderStyle-BackColor="#1e3a5f" HeaderStyle-ForeColor="White" CssClass="table table-bordered table-hover mb-0" CellPadding="5">
                                <Columns>


                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("smno") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:Label ID="lblEditID" runat="server" Text='<%# Eval("id") %>'>
                                            </asp:Label>
                                            </EditItemTemplate>--%>
                                            <%--<FooterTemplate>
                                                <asp:TextBox ID="txtAddID" runat="server" Width="100px"></asp:TextBox>
                                                </FooterTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item No" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemNo" runat="server" Text='<%# Eval("itemno") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <%-- <EditItemTemplate>
                                            <asp:label ID="lblStkEditID" runat="server" Text='<%# Eval("stk_id") %>'>
                                            </asp:label>
                                            </EditItemTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("itemname") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <%-- <EditItemTemplate>
                                            <asp:Label ID="lblEditName" runat="server" Text='<%# Eval("stk_name") %>'>
                                            </asp:Label>
                                            </EditItemTemplate>--%>
                                            <%--<FooterTemplate>
                                                <asp:TextBox ID="txtAddName" runat="server" Width="100px"></asp:TextBox>
                                                </FooterTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="qty" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblUnit1" runat="server"
                                                Text='<%# Eval("U1","{0:#,0}") %>'></asp:Label>--%>
                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="txtEditQty" BackColor="Aqua" runat="server"
                                                    MaxLength="10" Text='<%# Eval("Qty") %>' Visible="false">
                                                </asp:TextBox>
                                        </ItemTemplate>
                                        <%-- <EditItemTemplate>--%>
                                            <%--<asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server"
                                                Text='<%# Eval("U1","{0:#,0}") %>' Width="40px"
                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);">
                                                </asp:TextBox>--%>
                                                <%-- <asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server"
                                                    MaxLength="10" Text='<%# Eval("U1") %>'></asp:TextBox>
                                                    </EditItemTemplate>--%>
                                                    <%-- <FooterTemplate>
                                                        <asp:TextBox ID="txtAddUnit1" runat="server" Width="80px">
                                                        </asp:TextBox>
                                                        </FooterTemplate>--%>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                                ImageUrl="~/Images/edit.png" />
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete"
                                                ImageUrl="~/Images/Delete.png" OnClientClick="Confirm()" />--%>

                                            <asp:ImageButton ID="imgbtnDelete" runat="server"
                                                OnClick="imgbtnDelete_Click" ImageUrl="~/Images/Delete.png"
                                                OnClientClick="Confirm()" />
                                        </ItemTemplate>
                                        <%-- <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update"
                                                ImageUrl="~/Images/icon-update.png" Width="16px" Height="16px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                                ImageUrl="~/Images/icon-Cancel.png" Width="16px" Height="16px" />
                                            </EditItemTemplate>--%>

                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>

                        </div>

                    </div>
                    <div runat="server" id="usagestockdiv" class="form-horizontal" style="margin:20px 10px; width:98%;">
                        <asp:Label runat="server" ID="lblusage" Text="Usage Stock" Visible="false">

                        </asp:Label>
                        <asp:Button ID="btnExportUsage" runat="server" Text="Export to Excel" CssClass="btn btn-info"
                            style="font-weight: 600; border-color:gray; width:200px; margin:5px;" Visible="false"
                            OnClick="btnExportUsage_Click" OnClientClick="startExport(); return true;" />
                        <asp:DataGrid runat="server" ID="dg1" HeaderStyle-backcolor="#4b6c9e"
                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-BorderColor="White"
                            HeaderStyle-HorizontalAlign="Center" AllowSorting="true"
                            AlternatingItemStyle-BackColor="#ccffff" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-BackColor="WhiteSmoke" Width="100%" CssClass="jsgrid" AllowPaging="true"
                            PageSize="31" OnPageIndexChanged="dg1_PageIndexChanged" PagerStyle-Mode="NumericPages"
                            PagerStyle-Visible="false" PagerStyle-Position="Bottom" PagerStyle-HorizontalAlign="Center"
                            PagerStyle-NextPageText="Next" PagerStyle-PrevPageText="Prev">
                        </asp:DataGrid>
                    </div>




                    <table>
                        <tr>
                            <td style="text-align:center">
                                <table style="width:40%" border="0" runat="server" id="usagepage" visible="false">
                                    <tr>
                                        <td style="text-align:center">
                                            <asp:button runat="server" text="First" id="btnFirst">
                                            </asp:button>

                                        </td>
                                        <td style="text-align:center">

                                            <asp:button runat="server" text="Prev" id="btnPrev">
                                            </asp:button>
                                        </td>
                                        <td style="text-align:center">
                                            <asp:Label runat="server" ID="pagelbl" Text=" Page " Font-Bold="True"
                                                style="display:inline-block; width:108px;"></asp:Label>
                                        </td>
                                        <td style="text-align:center">
                                            <asp:button runat="server" text="Next" id="btnNext">
                                            </asp:button>

                                        </td>
                                        <td style="text-align:center">
                                            <asp:button runat="server" text="Last" id="btnLast">
                                            </asp:button>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </div>
                <!-- Modal Item -->
                <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem"
                            aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
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
                                            <div class="card-body">




                                                <div class="form-group row">
                                                    <asp:Label runat="server" AssociatedControlID="txtSearch"
                                                        CssClass="col-sm-2 col-form-label text-nowrap" Text="Item: ">
                                                    </asp:Label>

                                                    <div class="col-sm-10">
                                                        <div class="input-group">
                                                            <asp:TextBox id="txtSearch" runat="server"
                                                                Style="width:70%;" CssClass="form-control"
                                                                placeholder="Search" type="search" aria-label="Search">
                                                            </asp:TextBox>
                                                            <div class="input-group-append">
                                                                <button runat="server" id="btnSearch"
                                                                    onserverclick="btnSearch_Click"
                                                                    class="btn btn-primary" title="Search">
                                                                    <i class="fas fa-search"></i>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <%--<button runat="server" id="btnClear"
                                                            onserverclick="btnClearSearch_Click" class="btn btn-mini"
                                                            title="Clear Search">
                                                            <i class="fas fa-times"></i>
                                                            </button>--%>
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
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server">
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td hidden="hidden">
                                                                    <asp:Label ID="lblID" runat="server"
                                                                        Text=' <%# Eval("Itemno")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server"
                                                                        Text=' <%# Eval("Itemname")%>'></asp:Label>
                                                                </td>
                                                                <%-- <td>
                                                                    <asp:Label ID="Label1" runat="server"
                                                                        Text=' <%# Eval("ttlqty", "{0:0.##}") %>'>
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
                                            data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary" runat="server"
                                            onserverclick="RadButton1_ServerClick" id="RadButton1">Add</button>
                                        <%--<button id="btnAll1" type="button" runat="server" class="btn btn-primary"
                                            onserverclick="btnAll1_ServerClick">SelectAll</button>--%>
                                            <%--<button type="button" class="btn btn-primary" runat="server"
                                                onserverclick="btnSubmit_Click">Select</button>
                                                <button type="button" id="btnAll" runat="server" class="btn btn-primary"
                                                    onserverclick="btnAll_Click">SelectAll</button>--%>
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
                <div class="loading" style="text-align:center">

                    <div style="display:block;margin-left:auto;margin-right:auto;width:40%;">
                        <br /><br />
                        <asp:label runat="server" Text="Loading... Please wait..." ForeColor="Black" Font-Bold="True"
                            Font-Size="Larger"></asp:label><br /><br />

                        <img src="/images/loader.gif" alt="" />
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

                <!-- Datatables-->



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
