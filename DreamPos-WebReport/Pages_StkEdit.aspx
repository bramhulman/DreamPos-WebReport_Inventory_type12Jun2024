<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_StkEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_StkEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
                <style>
                    .input-group>.form-control {
                        width: 800px
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
                </style>
                <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
                <%--<script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>--%>
                    <script src="js/jquery.ui.core.js" type="text/javascript"></script>
                    <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
                    <script src="js/jquery.ui.button.js" type="text/javascript"></script>
                    <script src="js/jquery.ui.position.js" type="text/javascript"></script>
                    <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script>
                    <script src="js/jquery.ui.combobox.js" type="text/javascript"></script>
                    <script type="text/javascript"
                        src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                    <script type="text/javascript">
                        function ShowProgress() {
                            setTimeout(function () {
                                var modal = $('<div />');
                                modal.addClass("modal");
                                $('body').append(modal);
                                var loading = $(".loading");
                                loading.show();
                            }, 200);
                        }
                        $('form').live("submit", function () {
                            ShowProgress();
                        });
                        $(document).ready(function () {
                            $('.btn').click(function () {
                                if (Page_IsValid) {
                                    ShowProgress();
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
                    </script>
                    <script type="text/javascript">
                        function RefreshUpdatePanel() {
                            __doPostBack('<%= txtNo.ClientID %>', '');
                        };
                    </script>
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>

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

                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Stock Master Edit</h1>
                            </div><!-- /.col -->
                            <asp:Label ID="lblMsg" runat="server" Visible="false">
                            </asp:Label>
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Stock Master Edit</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color: white;">

                    <!-- /.content-header -->
                    <%-- <h2>User Access</h2>--%>

                        <div class="row">
                            <div class="col-sm-6" style="background-color: white;">
                                <div class="form-horizontal" id="formload">
                                    <div class="card-body">
                                        <div class="form-group row col-sm-12">
                                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                                ID="lblitemno" Text="Number: "></asp:Label>
                                            <div class="input-group col-md-6">

                                                <asp:TextBox runat="server" ID="txtNo" Class="form-control"
                                                    AutoPostBack="true" OnTextChanged="txtNo_TextChanged" MaxLength="15"
                                                    Style="width: 800px; padding: 0.375rem 0.75rem;" />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                                    runat="server" ControlToValidate="txtNo" Enabled="False"
                                                    ErrorMessage="No Must Be Fill." ForeColor="Red">
                                                    </asp:RequiredFieldValidator>--%>
                                                    <asp:Label runat="server" ID="errNo" Visible="false"
                                                        Style="color: red;"></asp:Label>
                                            </div>


                                        </div>
                                        <div class="form-group row col-sm-12">
                                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                                ID="Label2" Text="Name: "></asp:Label>
                                            <div class="input-group col-md-6">

                                                <asp:TextBox runat="server" ID="txtName" Class="form-control"
                                                    MaxLength="30" />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                    runat="server" ControlToValidate="txtName"
                                                    ErrorMessage="Item Name Must Be Fill." ForeColor="Red">
                                                    </asp:RequiredFieldValidator>--%>

                                            </div>


                                        </div>
                                        <%--<div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                                ID="Label7" Text="CC: "></asp:label>
                                            <div class="input-group col-md-3">
                                                <asp:DropDownList ID="ddlcc" runat="server" CssClass="form-control"
                                                    AppendDataBoundItems="True" AutoPostBack="true"
                                                    EnableViewState="True" ViewStateMode="Enabled">
                                                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="ddlcc" ErrorMessage="Please Choice Cc"
                                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>


                                    </div>--%>
                                    <div class="form-group row col-sm-12">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label3" Text="Stock Group: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:DropDownList ID="DdlGrpStk" runat="server" AppendDataBoundItems="True"
                                                Style="width: 800pt; padding: 0.375rem 0.75rem;" AutoPostBack="True"
                                                EnableViewState="True" ViewStateMode="Enabled">
                                            </asp:DropDownList>



                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="DdlGrpStk" ErrorMessage="Choose Stock Group"
                                                InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                --%>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label4" Text="Unit 1: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox runat="server" ID="txtUnit1" MaxLength="5" Text="pcs"
                                                Class="form-control" />


                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label9" Text="Unit 2: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox runat="server" ID="txtUnit2" MaxLength="5" Text="pcs"
                                                Class="form-control" />


                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label10" Text="Unit 3: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox runat="server" ID="txtUnit3" MaxLength="5" Text="pcs"
                                                Class="form-control" />


                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label11" Text="Factor 1: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:right;" runat="server" ID="txtFac1" Text="1"
                                                Class="form-control" />
                                            <%-- <asp:RangeValidator runat="server" Type="Integer" 181: MinimumValue="1"
                                                MaximumValue="999999" ControlToValidate="txtFac1" 182:
                                                ErrorMessage="Value must be a whole number" Style="color:red;" />--%>
                                            <asp:Label runat="server" ID="errfac1" Visible="false" Style="color: red;">
                                            </asp:Label>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label12" Text="Factor 2: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:right;" runat="server" ID="txtFac2" Text="1"
                                                Class="form-control" />
                                            <%--<asp:RangeValidator runat="server" Type="Integer" 194: MinimumValue="1"
                                                MaximumValue="999999" ControlToValidate="txtFac2" 195:
                                                ErrorMessage="Value must be a whole number" Style="color:red;" />--%>
                                            <asp:Label runat="server" ID="errfac2" Visible="false" Style="color: red;">
                                            </asp:Label>

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" runat="server" id="rec_cost">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label123" Text="Warehouse cost: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:right;" runat="server" ID="txtcost" Text="0"
                                                Class="form-control" Enabled="False" />
                                            <asp:Label runat="server" CssClass="col-form-label" ID="lblcost"
                                                Text="(/ Unit 2)">
                                            </asp:Label>

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12" runat="server" id="f_cost" visible="false">
                                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap"
                                            ID="Label6" Text="Fix cost: "></asp:Label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:right;" runat="server" ID="txtfcost" Text="0"
                                                Class="form-control" Width="40%" />
                                            <asp:Label runat="server" CssClass="col-form-label" ID="lblfcost"
                                                Text="(/ Unit 2)">
                                            </asp:Label>

                                        </div>


                                    </div>

                                    <%-- <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                            ID="Label13" Text="Stock Limit: "></asp:label>
                                        <div class="input-group col-md-3">

                                            <asp:TextBox style="text-align:center;" runat="server"
                                                OnTextChanged="txtFac2_TextChanged" AutoPostBack="True" ID="txtStkLimit"
                                                Text="0" Class="form-control"
                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" />


                                        </div>
                                </div>--%>

                                <div class="form-group row col-md-12">
                                    <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-nowrap" ID="Label5"
                                        Text="Batch: "></asp:Label>
                                    <div class="form-check input-group col-md-6">

                                        <asp:CheckBox ID="chkbatch" runat="server" AutoPostBack="True"
                                            OnCheckedChanged="chkbatch_CheckedChanged"
                                            cssclass="form-control-check-input" type="checkbox" />
                                        <asp:Label runat="server" CssClass="form-check-label" for="chklink" ID="Label1"
                                            Text="Yes" style="padding-left:10px;"></asp:Label>
                                    </div>


                                </div>
                                <%--<div class="form-group row col-md-12">
                                    <asp:label runat="server" CssClass="col-sm-3 col-form-label text-nowrap" ID="Label6"
                                        Text="Recipe Link: "></asp:label>
                                    <div class="input-group col-md-6">

                                        <asp:DropDownList ID="DdlLink" Enabled="false" runat="server"
                                            AppendDataBoundItems="True" AutoPostBack="True"
                                            Style="width:800pt;padding: 0.375rem 0.75rem;" EnableViewState="True"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Value="0">--Select Receipe--</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>


                            </div> --%>




                            <div class="form-group row">
                                <div Class="col-md-1"></div>
                                <div class="col-md-offset-2 col-md-10">
                                    <table>
                                        <tr>
                                            <td>
                                                <button type="button" runat="server" class="btn btn-primary"
                                                    data-toggle="modal" id="btnaddcc" data-target="#ModalItem"
                                                    style="font-weight: 600; border-color:gray; width:120px;"
                                                    disabled="disabled">
                                                    Add Recipe
                                                </button>
                                            </td>
                                            <td>
                                                <button type="button" runat="server" class="btn btn-primary"
                                                    id="btnInsert" onserverclick="btnInsert_Click"
                                                    style="font-weight: 600; border-color:gray; width:120px;">
                                                    Add
                                                </button>
                                                <%--<asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click"
                                                    Text="Add" CssClass="btn btn-primary"
                                                    Style="font-weight: 600; border-color:gray; width:108px;" />--%>

                                            </td>
                                            <td>
                                                <button type="button" runat="server" class="btn btn-primary"
                                                    id="btnUpdate" onserverclick="btnUpdate_Click"
                                                    style="font-weight: 600; border-color:gray; width:120px;"
                                                    Visible="false">
                                                    Update
                                                </button>
                                                <%--<asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click"
                                                    Text="Update" CssClass="btn btn-primary"
                                                    Style="font-weight: 600; border-color:gray; width:108px;"
                                                    Visible="false" />--%>
                                            </td>
                                            <%-- <td>
                                                <asp:Button runat="server" ID="btnlimit" OnClick="btnlimit_Click"
                                                    Text="set Limit" CssClass="btn btn-primary"
                                                    Style="font-weight: 600; border-color:gray; width:108px;" />
                                                </td>--%>
                                                <td>
                                                    <button type="button" runat="server" class="btn btn-primary"
                                                        id="btnCancel" onserverclick="btnCancel_Click"
                                                        style="font-weight: 600; border-color:gray; width:108px;">
                                                        Cancel
                                                    </button>
                                                    <%--<asp:Button runat="server" ID="btnCancel"
                                                        OnClick="btnCancel_Click" Text="Cancel"
                                                        CssClass="btn btn-primary"
                                                        Style="font-weight: 600; border-color:gray; width:108px;"
                                                        CausesValidation="false" />--%>
                                                </td>
                                        </tr>
                                    </table>

                                </div>
                            </div>



                        </div>

                </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-horizontal" style="margin:0 20px; width:100%;overflow:scroll;">
                        <asp:ListView ID="lstViewRcp" runat="server" OnItemCommand="lstViewRcp_ItemCommand"
                            OnPagePropertiesChanging="OnPageRcpChanging">
                            <LayoutTemplate>
                                <div class="table-responsive">
                                    <table id="tblCostDetails" align="left"
                                        style="border-style: solid; width:100%; margin-bottom:0;"
                                        class="menutab table table-bordered">
                                        <thead>
                                            <tr role="row">

                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Recipe</b>
                                                </th>


                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Action</b>
                                                </th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </div>
                            </LayoutTemplate>


                            <ItemTemplate>

                                <td>
                                    <asp:Label ID="Label8" runat="server" Text=' <%# Eval("namercp")%>'></asp:Label>
                                </td>


                                <td style="width: 8%; text-align: center">

                                    <%--<asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                                        CommandArgument='<%#Eval("ccid").ToString%>' runat="server"
                                        ImageUrl="/images/edit.png" />--%>
                                    <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                                        ID="imgBtnDelete" CommandName="Del"
                                        CommandArgument='<%#Eval("rcpID").ToString %>' runat="server"
                                        ImageUrl="~/images/delete.png"></asp:ImageButton>
                                </td>




                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <%--< /table>

                            <br />--%>
                    </div>
                    <div class="table-responsive" style="margin:0 20px; width:100%;">
                        <table class="table" style="margin-bottom: 0;">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                                    <asp:DataPager ID="lstViewRcpPager1" runat="server" PagedControlID="lstViewRcp"
                                        PageSize="5">
                                        <Fields>
                                            <asp:NumericPagerField ButtonType="Link" />
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>
                </div>
                <!-- Modal CC -->
                <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem"
                            aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                            <div class="modal-dialog modal-dialog-scrollable" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="ModalStoreLabel">Select Recipe</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>

                                    <div class="row">
                                        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                                            <div class="card-body">




                                                <div class="form-group row">
                                                    <asp:Label runat="server" AssociatedControlID="txtSearchrcp"
                                                        CssClass="col-sm-2 col-form-label text-nowrap" Text="Recipe: ">
                                                    </asp:Label>

                                                    <div class="input-group-append">
                                                        <div class="input-group input-group-sm">
                                                            <asp:TextBox id="txtSearchrcp" style="width:auto;"
                                                                runat="server" CssClass="form-control"
                                                                placeholder="Search" type="search" aria-label="Search">
                                                            </asp:TextBox>


                                                            <button runat="server" id="btnSearchrcp"
                                                                onserverclick="Searchrcp_Click" class="btn btn-mini"
                                                                title="Search">
                                                                <i class="fas fa-search"></i>
                                                            </button>
                                                            <button runat="server" id="btnClearSearchrcp"
                                                                onserverclick="btnClearSearchrcp_Click"
                                                                class="btn btn-mini" title="Clear Search">
                                                                <i class="fas fa-times"></i>
                                                            </button>


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

                                                        <th scope="col">Recipe</th>
                                                        <%--<th scope="col">CC Stock</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>

                                                    <asp:ListView ID="LsvStk" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="CheckBox" runat="server">
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td hidden="hidden">
                                                                    <asp:Label ID="lblID" runat="server"
                                                                        Text=' <%# Eval("Id")%>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblNumber" runat="server"
                                                                        Text=' <%# Eval("code")%>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server"
                                                                        Text=' <%# Eval("name")%>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <%--<td>
                                                                    <asp:Label ID="Label1" runat="server"
                                                                        Text=' <%# Eval("stock")%>'></asp:Label>
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
                <%--modalcc--%>

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