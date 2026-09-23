<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvReturnEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvReturnEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>
                <style>
                    .input-group>.form-control {
                        width: 800px;
                    }
                </style>

                <script type="text/javascript">
                    function ShowProgress() {
                        setTimeout(function () {
                            var modal = $('<div />');
                            modal.addClass("modal");
                            $('body').append(modal);
                            var loading = $(".loading");
                            if (loading.length > 0) {
                                loading.show();
                                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                                loading.css({ top: top, left: left });
                            }
                        }, 200);
                    }
                    $('form').submit(function () {
                        ShowProgress();
                    });
                    $(document).ready(function () {
                        $('.btn').click(function () {
                            if (typeof Page_IsValid == 'undefined' || Page_IsValid) {
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
                                <h1 class="m-0 text-dark">Inventory Return Edit</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Inventory Return Edit</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color:white;">

                    <!-- /.content-header -->
                    <%-- <h2>User Access</h2>--%>

                        <div class="row">
                            <div class="form-horizontal" id="formload">
                                <div class="card-body">
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblReturnNo" Text="Return No: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="" runat="server" ID="txtLinkid" Class="form-control"
                                                ReadOnly="true" />

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label1" Text="DO: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="" runat="server" ID="txtDO" Class="form-control" />

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label8" Text="CC: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:DropDownList ID="ddlFrom" runat="server" AppendDataBoundItems="True"
                                                CssClass="form-control" AutoPostBack="True" EnableViewState="True"
                                                ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="ddlFrom" ErrorMessage="Item Must Be Choice"
                                                InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                --%>

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label7" Text="Supplier: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:DropDownList ID="DdlSup" runat="server" AppendDataBoundItems="True"
                                                CssClass="form-control" AutoPostBack="True" EnableViewState="True"
                                                ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="DdlSup" ErrorMessage="Cost Centre Must Be Choice"
                                                InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                --%>

                                        </div>


                                    </div>


                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label3" Text="Inv Date: "></asp:label>
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
                                                        ID="txtinvdate" Class="form-control" AutoPostBack="false" />
                                                    <ajaxtoolkit:CalendarExtender ID="cle" runat="server"
                                                        TargetControlID="txtinvdate" Format="dd MMM yyyy">
                                                    </ajaxtoolkit:CalendarExtender>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="form-group row">
                                        <div Class="col-md-2"></div>
                                        <div class="col-md-offset-2 col-md-10">
                                            <button type="button" runat="server" class="btn btn-primary"
                                                data-toggle="modal" id="btnshow" data-target="#ModalItem"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Add Item
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btnSave"
                                                onserverclick="btnSave_Click"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Save Item
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btncancel"
                                                onserverclick="btnCancel_Click"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Back
                                            </button>


                                        </div>
                                    </div>



                                </div>

                            </div>
                        </div>
                        <div class="card-body" style="overflow:scroll;">
                            <div class="row">
                                <div class="form-horizontal" style="margin:0 20px; width:100%;">
                                    <asp:GridView ID="DgvTransfer" runat="server" Width="100%"
                                        AutoGenerateColumns="false" OnRowDeleting="DgvTransfer_RowDeleting"
                                        OnRowUpdating="DgvTransfer_RowUpdating"
                                        OnRowCancelingEdit="DgvTransfer_RowCancelingEdit"
                                        OnRowEditing="DgvTransfer_RowEditing" HeaderStyle-BackColor="#4D4D4D"
                                        HeaderStyle-ForeColor="White" CellPadding="5">
                                        <Columns>


                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblIEditD" runat="server" Text='<%# Eval("ID") %>'>
                                                    </asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStkID" runat="server"
                                                        Text='<%# Eval("stk_id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblStkEditID" runat="server"
                                                        Text='<%# Eval("stk_id") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server"
                                                        Text='<%# Eval("stk_name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditName" runat="server"
                                                        Text='<%# Eval("stk_name") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit 1" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnit1" runat="server"
                                                        Text='<%# Eval("U1", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server"
                                                        Text='<%# Eval("U1") %>' Width="40px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:TextBox ID="txtAddUnit1" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U1_nm") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditU1Nm" runat="server"
                                                        Text='<%# Eval("U1_nm") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>

                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit 2" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnit2" runat="server"
                                                        Text='<%# Eval("U2", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>

                                                    <asp:TextBox ID="txtEditUnit2" BackColor="Aqua" runat="server"
                                                        Text='<%# Eval("U2") %>' Width="40px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:TextBox ID="txtAddUnit2" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblU2Nm" runat="server" Text='<%# Eval("U2_nm") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditU2Nm" runat="server"
                                                        Text='<%# Eval("U2_nm") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>

                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Factor" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfactor" runat="server"
                                                        Text='<%# Eval("factor1", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditfactor" runat="server"
                                                        Text='<%# Eval("factor1", "{0:#,0}") %>' Width="60px">
                                                    </asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtAddfactor" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Return" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblttlStock" runat="server"
                                                        Text='<%# Eval("ttlStock", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblttlStock" runat="server"
                                                        Text='<%# Eval("ttlStock", "{0:#,0}") %>' Width="80px">
                                                    </asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtttlStock" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CC Stock" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblccStock" runat="server"
                                                        Text='<%# Eval("ttlqty", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblccStock" runat="server"
                                                        Text='<%# Eval("ttlqty", "{0:#,0}") %>' Width="80px">
                                                    </asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtttlStock" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Unit1 Cost" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblU1Cost" runat="server"
                                                        Text='<%# Eval("CostU1","{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditU1Cost" BackColor="Aqua" runat="server"
                                                        Text='<%# Eval("CostU1","{0:#,0}") %>' Width="60px"
                                                        onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);">
                                                    </asp:TextBox>
                                                </EditItemTemplate>

                                                </asp:TemplateField>--%>
                                                <%-- <asp:TemplateField HeaderText="Cost" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCost" runat="server"
                                                            Text='<%# Eval("Cost", "{0:#,0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="txtEditCost" runat="server"
                                                            Text='<%# Eval("Cost", "{0:#,0}") %>'></asp:Label>
                                                    </EditItemTemplate>

                                                    </asp:TemplateField>--%>
                                                    <%-- <asp:TemplateField HeaderText="Total Cost"
                                                        HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblttlCost" runat="server"
                                                                Text='<%# Eval("ttlCost", "{0:#,0}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblttlCost" runat="server"
                                                                Text='<%# Eval("ttlCost", "{0:#,0}") %>' Width="60px">
                                                            </asp:Label>
                                                        </EditItemTemplate>--%>
                                                        <%--<FooterTemplate>
                                                            <asp:TextBox ID="txtttlStock" runat="server" Width="80px">
                                                            </asp:TextBox>
                                                            </FooterTemplate>--%>
                                                            <%-- </asp:TemplateField>--%>

                                                                <asp:TemplateField HeaderText="Action"
                                                                    HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server"
                                                                            CommandName="Edit"
                                                                            ImageUrl="~/Images/edit.png" />
                                                                        <asp:ImageButton ID="imgbtnDelete"
                                                                            runat="server" CommandName="Delete"
                                                                            ImageUrl="~/Images/Delete.png"
                                                                            OnClientClick="Confirm()" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnUpdate"
                                                                            runat="server" CommandName="Update"
                                                                            ImageUrl="~/Images/icon-update.png"
                                                                            Width="16px" Height="16px" />
                                                                        <asp:ImageButton ID="imgbtnCancel"
                                                                            runat="server" CommandName="Cancel"
                                                                            ImageUrl="~/Images/icon-Cancel.png"
                                                                            Width="16px" Height="16px" />
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




                                                <div class="form-group row" style="padding: 0 1rem;">
                                                    <asp:Label runat="server" AssociatedControlID="txtSearch"
                                                        CssClass="col-sm-2 col-form-label text-nowrap" Text="Item: ">
                                                    </asp:Label>

                                                    <div class="col-sm-10">
                                                        <div class="input-group">
                                                            <asp:TextBox id="txtSearch" runat="server" Style="width:50%;"
                                                                CssClass="form-control" placeholder="Search"
                                                                type="search" aria-label="Search"></asp:TextBox>
                                                            <div class="input-group-btn">
                                                                <button runat="server" id="btnSearch"
                                                                    onserverclick="btnSearch_ServerClick"
                                                                    class="btn btn-default" title="Search"
                                                                    style="margin-left: -2px; border-radius: 0;">
                                                                    <i class="fas fa-search"></i>
                                                                </button>
                                                                <button runat="server" id="btnClear"
                                                                    onserverclick="btnClearSearch_Click"
                                                                    class="btn btn-danger" title="Clear Search"
                                                                    style="border-radius: 0 4px 4px 0;">
                                                                    <i class="fas fa-times"></i>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>



                                    </div>

                                    <div class="modal-body">
                                        <div class="table-responsive" style="max-height: 400px; overflow: auto;">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th>check</th>
                                                        <th scope="col" hidden="hidden">ID</th>

                                                        <th scope="col">Name</th>
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
                                                                        Text=' <%# Eval("Id")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server"
                                                                        Text=' <%# Eval("name")%>'></asp:Label>
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
                <div style="font-family: Calibri; ">
                    <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>

                </div>
                </div>


                <script>

                    if ($(window).width() >= 960) {
                        document.getElementById("formload").style.width = "100%";
                    } else {
                        document.getElementById("formload").style.width = "80%";

                    }

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